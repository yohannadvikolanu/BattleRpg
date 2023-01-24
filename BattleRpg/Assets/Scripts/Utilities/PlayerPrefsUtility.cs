using UnityEngine;

namespace BattleRpg.Utilities
{
    public class PlayerPrefsUtility
    {
        public static string GetInventoryItem(string key)
        {
            return PlayerPrefs.GetString(key);
        }

        public static void SetAndSaveInventoryItem(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
            PlayerPrefs.Save();
        }
    }
}