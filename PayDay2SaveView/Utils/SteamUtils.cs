using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Win32;

// ReSharper disable MemberCanBeMadeStatic.Local

namespace PayDay2SaveView.Utils
{
    public class SteamUtils
    {
        private string GetSteamFolder()
        {
            // HKEY_CURRENT_USER\SOFTWARE\Valve\Steam\SteamPath
            return (string)Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Valve\Steam", "SteamPath", null);
        }

        private string GetSteamUserdataDirectory()
        {
            return Path.Combine(GetSteamFolder(), "userdata");
        }

        public string GetSteamUser()
        {
            var steamFolder = GetSteamFolder();
            if (string.IsNullOrEmpty(steamFolder))
                throw new Exception("Steam nicht gefunden :(");

            // HKEY_CURRENT_USER\SOFTWARE\Valve\Steam\Users
            var hcu = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Valve\Steam\Users");
            var uids = hcu.GetSubKeyNames();
            if (uids.Length > 1)
                throw new NotImplementedException();
            return uids.First();
        }

        private string GetSteamUserDirectory(long steamUserId)
        {
            return Path.Combine(GetSteamUserdataDirectory(), steamUserId.ToString());
        }

        public string GetGameDirectory(long steamUserId, int gameId)
        {
            return Path.Combine(GetSteamUserDirectory(steamUserId), gameId.ToString());
        }

        public IEnumerable<long> GetSteamUsersWithGame(int gameId)
        {
            foreach (var path in Directory.GetDirectories(GetSteamUserdataDirectory()))
            {
                long steamUserId;
                if (!long.TryParse(Path.GetFileName(path), out steamUserId))
                    continue;
                if (UserHasGame(steamUserId, gameId))
                    yield return steamUserId;
            }
        }

        private bool UserHasGame(long steamUserId, int gameId)
        {
            return Directory.Exists(Path.Combine(GetSteamUserdataDirectory(), steamUserId.ToString(), gameId.ToString()));
        }
    }
}