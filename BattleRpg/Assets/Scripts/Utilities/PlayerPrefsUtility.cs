using BattleRpg.Hero;
using UnityEditor;
using UnityEngine;

namespace BattleRpg.Utilities
{
    public class PlayerPrefsUtility
    {
        [MenuItem("Utilities/Delete All Player Prefs")]
        static void DeleteAllPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }

        public static void SetAndSaveInventoryHero(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
            PlayerPrefs.Save();
        }

        public static string GetInventoryHero(string key)
        {
            return PlayerPrefs.GetString(key);
        }
    }
}