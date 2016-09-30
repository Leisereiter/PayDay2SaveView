using System;
using System.IO;
using System.Linq;
using Microsoft.Win32;

// ReSharper disable MemberCanBeMadeStatic.Local

namespace PayDay2SaveView
{
    public class SteamUtils
    {
        private string GetSteamFolder()
        {
            // HKEY_CURRENT_USER\SOFTWARE\Valve\Steam\SteamPath
            return (string)Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Valve\Steam", "SteamPath", null);
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

        public string GetSteamUserDirectory(string steamUserId)
        {
            return Path.Combine(GetSteamFolder(), "userdata", steamUserId);
        }

        public string GetGameDirectory(string steamUserId, int gameId)
        {
            return Path.Combine(GetSteamUserDirectory(steamUserId), gameId.ToString());
        }
    }
}