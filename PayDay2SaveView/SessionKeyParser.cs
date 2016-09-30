using System;
using System.Collections.Generic;
using System.Linq;

namespace PayDay2SaveView
{
    public class SessionKeyParser
    {
        private readonly Stack<string> _tokens;

        public SessionKeyParser(string key)
        {
            _tokens = new Stack<string>(key.Split('_'));
        }

        // jewelry_store_overkill_failed_dropin
        public SessionState ReadSessionState()
        {
            // started | started_dropin | completed | completed_dropin | dropin | failed | failed_dropin

            var token = _tokens.Pop();

            switch (token)
            {
                case "started":
                    return SessionState.Started;

                case "completed":
                    return SessionState.Completed;

                case "failed":
                    return SessionState.Failed;

                case "dropin":
                    var nextToken = _tokens.Pop();

                    if (nextToken == "started")
                        return SessionState.StartedDropin;

                    if (nextToken == "failed")
                        return SessionState.FailedDropin;

                    if (nextToken == "completed")
                        return SessionState.CompletedDropin;

                    _tokens.Push(nextToken);
                    return SessionState.Dropin;

                default:
                    throw new InvalidOperationException($"Unexpected token {token}");
            }
        }

        public Difficulty ReadDifficulty()
        {
            // => networkaccountsteam.lua
            // easy | normal | hard | overkill | overkill 145 | overkill_290

            var token = _tokens.Pop();
            switch (token)
            {
                case "easy":
                    return Difficulty.Easy;

                case "normal":
                    return Difficulty.Normal;

                case "hard":
                    return Difficulty.Hard;

                case "overkill":
                    return Difficulty.Overkill;

                case "145":
                    Expect("overkill", _tokens.Pop());
                    return Difficulty.Overkill145;

                case "290":
                    Expect("overkill", _tokens.Pop());
                    return Difficulty.Overkill290;

                default:
                    throw new InvalidOperationException($"Unexpected token {token}");
            }
        }

        public string ReadJobId()
        {
            return string.Join("_", _tokens.Reverse());
        }

        private static void Expect(string expected, string actual)
        {
            if (expected != actual)
                throw new InvalidOperationException($"Expected token '{expected}' given '{actual}'");
        }
    }
}