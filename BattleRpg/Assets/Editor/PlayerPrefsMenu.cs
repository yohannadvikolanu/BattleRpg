using System;
using UnityEditor;
using UnityEngine;
using BattleRpg.Utilities;

namespace BattleRpg.Editor.Menu
{
    public class PlayerPrefsMenu
    {
        /// <summary>
        /// Utility method to clear player prefs from the editor menus.
        /// </summary>
        [MenuItem("Utilities/Delete All Player Prefs")]
        static void DeleteAllPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }

        /// <summary>
        /// Utility method to add 5 battles to the player's inventory, to make testing quicker.
        /// </summary>
        [MenuItem("Utilities/Add 5 Battles")]
        static void Add5Battles()
        {
            string battlesCompletedString = PlayerPrefsUtility.GetInventoryItem("BattlesCompleted");
            int battlesCompleted = 0;

            if (battlesCompletedString != "")
            {
                battlesCompleted = Int32.Parse(battlesCompletedString);
            }
            else
            {
                Debug.Log("Battles Completed inventory item did not exist, creating one.");
            }
            battlesCompleted += 5;
            Debug.Log(string.Format("Battles Completed so far: {0}", battlesCompleted));
            
            PlayerPrefsUtility.SetAndSaveInventoryItem("BattlesCompleted", battlesCompleted.ToString());
        }
    }
}