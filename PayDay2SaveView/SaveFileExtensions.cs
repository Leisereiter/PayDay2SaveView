using System.Collections.Generic;
using PaydaySaveEditor.PD2;

namespace PayDay2SaveView
{
    public static class SaveFileExtensions
    {
        public static Dictionary<object, object> GetPlayedSessions(this SaveFile saveFile)
        {
            var gameData = saveFile.GameData;
            var statisticsManager = (Dictionary<object, object>)gameData["StatisticsManager"];
            var sessions = (Dictionary<object, object>)statisticsManager["sessions"];
            var jobs = (Dictionary<object, object>)sessions["jobs"];
#if DEBUG
            jobs["foo_normal_completed"] = 1;
            jobs["foo_hard_completed"] = 23;
            jobs["bar_sm_wish_started"] = 3;
#endif
            return jobs;
        }
    }
}