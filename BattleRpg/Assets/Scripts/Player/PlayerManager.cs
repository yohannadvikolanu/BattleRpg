using System;
using System.Collections.Generic;
using System.Linq;
using BattleRpg.Hero;
using BattleRpg.Menu;
using BattleRpg.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BattleRpg.Player
{
    public class PlayerManager : MonoBehaviour
    {
        private const string BattlesCompletedKey = "BattlesCompleted";
        private static PlayerManager instance;
        public static PlayerManager Instance { get { return instance; } }

        [SerializeField]
        private string battleSceneName;
        [SerializeField]
        private string menuSceneName;

        public List<HeroType> BattleHeroList { get { return battleHeroList; } }
        private List<HeroType> battleHeroList;

        private MenuSceneManager menuSceneManager;

        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
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

        public void SetHeroListAndStartBattle(List<HeroType> heroList)
        {
            battleHeroList = heroList;
            SceneManager.LoadSceneAsync(battleSceneName);
        }

        public void BattleCompleted()
        {
            string battlesCompletedString = PlayerPrefsUtility.GetInventoryItem(BattlesCompletedKey);
            
            if (battlesCompletedString == "")
            {
                Debug.Log("Battles Completed inventory item did not exist, creating one.");
                PlayerPrefsUtility.SetAndSaveInventoryItem(BattlesCompletedKey, "0");
            }
            else
            {
                int battlesCompleted = Int32.Parse(battlesCompletedString);
                battlesCompleted += 1;
                Debug.Log(string.Format("Battles Completed so far: {0}", battlesCompleted));

                PlayerPrefsUtility.SetAndSaveInventoryItem("BattlesCompleted", battlesCompleted.ToString());

                if (battlesCompleted % 5 == 0 && PlayerInventory.Instance.unlockedList.Count < 10)
                {
                    int requiredUnlockedHeroes = (battlesCompleted / 5) + 3;

                    Debug.Log("Required battles completed, unlocking new hero.");

                    if (PlayerInventory.Instance.unlockedList.Count < requiredUnlockedHeroes)
                    {
                        PlayerInventory.Instance.UnlockHero();
                    }
                }
            }

            SceneManager.LoadSceneAsync(menuSceneName);
            PlayerInventory.Instance.UpdateInventory();
        }
    }
}