using UnityEngine;

namespace BattleRpg.Utilities
{
    /// <summary>
    /// Represents the class that allows access to PlayerPrefs and provides functionality to update them.
    /// </summary>
    public class PlayerPrefsUtility
    {
        /// <summary>
        /// Gets the specified inventory item from the player prefs.
        /// </summary>
        /// <param name="key">Key of the item.</param>
        /// <returns>String stored in player prefs.</returns>
        public static string GetInventoryItem(string key)
        {
            return PlayerPrefs.GetString(key);
        }

        /// <summary>
        /// Sets and saves the specified inventory item to the player prefs.
        /// </summary>
        /// <param name="key">Key of the item.</param>
        /// <param name="value">String Value to be stored for the item.</param>
        public static void SetAndSaveInventoryItem(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
            PlayerPrefs.Save();
        }
    }
}