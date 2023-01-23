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

        static void SaveAndSetInventory(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
            PlayerPrefs.Save();
        }

        static string GetString(string key)
        {
            return PlayerPrefs.GetString(key);
        }
    }
}