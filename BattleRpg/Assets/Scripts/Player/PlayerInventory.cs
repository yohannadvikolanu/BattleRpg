using System;
using System.Collections.Generic;
using System.Linq;
using BattleRpg.Hero;
using BattleRpg.Utilities;
using UnityEngine;

namespace BattleRpg.Player
{
    /// <summary>
    /// Class that represents the player's inventory.
    /// </summary>
    public class PlayerInventory : MonoBehaviour
    {
        // Using Singleton Pattern.
        private static PlayerInventory instance;
        public static PlayerInventory Instance { get { return instance; } }

        // In-Editor reference variables.
        [SerializeField]
        private List<HeroType> heroList = new List<HeroType>();

        // Public variables.
        public List<HeroType> unlockedList = new List<HeroType>();

        private void Awake()
        {
            // Setting up the singleton.
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                instance = this;
            }
        }

        /// <summary>
        /// Called to make sure the inventory is up-to-speed with the values in the PlayerPrefs.
        /// </summary>
        public void UpdateInventory()
        {
            unlockedList.Clear();

            // Refreshing inventory.
            heroList.ForEach(item =>
            {
                string itemString = PlayerPrefsUtility.GetInventoryItem(item.ToString());

                if (itemString != "")
                {
                    unlockedList.Add(item);
                }
            });

            // On first boot, we unlock 3 random heroes.
            if (unlockedList.Count < 3)
            {
                 Debug.Log("No heroes unlocked in inventory. Unlocking 3 random ones.");
                for (int i = 0; i < 3; i++)
                {
                    UnlockHero();
                }
            }
        }

        /// <summary>
        /// Allows player to unlock a random hero and store this in the Player Prefs.
        /// </summary>
        public void UnlockHero()
        {
            // Picking a new hero to unlock.
            HeroType heroType;
            do
            {
                heroType = heroList.ElementAt(UnityEngine.Random.Range(0, heroList.Count));
            } while (unlockedList.Any(item => item == heroType));

            // Saving to player prefs and setting the hero's exp to 0.
            PlayerPrefsUtility.SetAndSaveInventoryItem(heroType.ToString(), "0");
            unlockedList.Add(heroType);
            Debug.Log(string.Format("Unlocked hero: {0}", heroType));
        }

        /// <summary>
        /// Allows to add an experience point to an unlocked hero.
        /// </summary>
        /// <param name="heroType">Specify which hero we need to update.</param>
        public void AddExperiencePoint(HeroType heroType)
        {
            string currentExperiencePoints = PlayerPrefsUtility.GetInventoryItem(heroType.ToString());
            int updatedExperiencePoints = Int32.Parse(currentExperiencePoints) + 1;
            PlayerPrefsUtility.SetAndSaveInventoryItem(heroType.ToString(), updatedExperiencePoints.ToString());
        }
    }
}