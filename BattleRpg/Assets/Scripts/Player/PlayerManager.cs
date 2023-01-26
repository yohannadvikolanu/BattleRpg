using System;
using System.Collections.Generic;
using BattleRpg.Hero;
using BattleRpg.Menu;
using BattleRpg.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BattleRpg.Player
{
    /// <summary>
    /// Represents the class that manages the Player's instance during runtime.
    /// </summary>
    public class PlayerManager : MonoBehaviour
    {
        private const string BattlesCompletedKey = "BattlesCompleted";

        // Using Singleton Pattern.
        private static PlayerManager instance;
        public static PlayerManager Instance { get { return instance; } }

        // In-Editor reference variables.
        [SerializeField]
        private string battleSceneName;
        [SerializeField]
        private string menuSceneName;

        // Public variables.
        public List<HeroType> BattleHeroList { get { return battleHeroList; } }

        // Private variables.
        private List<HeroType> battleHeroList;
        private MenuSceneManager menuSceneManager;

        private void Awake()
        {
            // Making sure the Player Manager gameobject isn't destroy across scenes.
            DontDestroyOnLoad(this.gameObject);

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

        private void OnEnable()
        {
            PlayerInventory.Instance.UpdateInventory();
        }

        /// <summary>
        /// Sets the selected heroes on the menu and starts a battle.
        /// </summary>
        /// <param name="heroList">List of the selected heroes.</param>
        public void SetHeroListAndStartBattle(List<HeroType> heroList)
        {
            battleHeroList = heroList;
            SceneManager.LoadSceneAsync(battleSceneName);
        }

        /// <summary>
        /// Performs the required logic for when a battle is finished.
        /// </summary>
        public void BattleCompleted()
        {
            // Checking we're tracking the number of battles the player has completed.
            string battlesCompletedString = PlayerPrefsUtility.GetInventoryItem(BattlesCompletedKey);
            
            if (battlesCompletedString == "")
            {
                // If we aren't, set up the inventory tracker for number of battles.
                Debug.Log("Battles Completed inventory item did not exist, creating one.");
                PlayerPrefsUtility.SetAndSaveInventoryItem(BattlesCompletedKey, "1");
            }
            else
            {
                // Updating the battles completed value if it is already being tracked.
                int battlesCompleted = Int32.Parse(battlesCompletedString);
                battlesCompleted += 1;
                Debug.Log(string.Format("Battles Completed so far: {0}", battlesCompleted));

                PlayerPrefsUtility.SetAndSaveInventoryItem(BattlesCompletedKey, battlesCompleted.ToString());

                // If the player's completed battles is a multiple of 5, we unlock a new character, if possible.
                if (battlesCompleted % 5 == 0 && PlayerInventory.Instance.unlockedList.Count < 10)
                {
                    // Checking whether we've already unlocked a new hero.
                    int requiredUnlockedHeroes = (battlesCompleted / 5) + 3;

                    Debug.Log("Required battles completed, unlocking new hero.");

                    if (PlayerInventory.Instance.unlockedList.Count < requiredUnlockedHeroes)
                    {
                        // If not, unlocking a random hero.
                        PlayerInventory.Instance.UnlockHero();
                    }
                }
            }

            // Loading back to the menu scene and updating the inventory so that the menu scene is up to date.
            SceneManager.LoadSceneAsync(menuSceneName);
            PlayerInventory.Instance.UpdateInventory();
        }
    }
}