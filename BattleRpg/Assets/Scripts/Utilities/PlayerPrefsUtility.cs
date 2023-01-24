using System;
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

        [MenuItem("Utilities/Add 5 Battles")]
        static void Add5Battles()
        {
            string battlesCompletedString = GetInventoryItem("BattlesCompleted");
            int battlesCompleted = Int32.Parse(battlesCompletedString);
            battlesCompleted += 5;
            Debug.Log(string.Format("Battles Completed so far: {0}", battlesCompleted));
            PlayerPrefsUtility.SetAndSaveInventoryItem("BattlesCompleted", battlesCompleted.ToString());
        }

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