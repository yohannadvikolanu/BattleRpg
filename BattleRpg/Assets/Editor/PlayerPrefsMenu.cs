using System;
using UnityEditor;
using UnityEngine;
using BattleRpg.Utilities;

namespace BattleRpg.Editor.Menu
{
    public class PlayerPrefsMenu
    {
        [MenuItem("Utilities/Delete All Player Prefs")]
        static void DeleteAllPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }

        [MenuItem("Utilities/Add 5 Battles")]
        static void Add5Battles()
        {
            string battlesCompletedString = PlayerPrefsUtility.GetInventoryItem("BattlesCompleted");
            int battlesCompleted = Int32.Parse(battlesCompletedString);
            battlesCompleted += 5;
            Debug.Log(string.Format("Battles Completed so far: {0}", battlesCompleted));
            PlayerPrefsUtility.SetAndSaveInventoryItem("BattlesCompleted", battlesCompleted.ToString());
        }
    }
}