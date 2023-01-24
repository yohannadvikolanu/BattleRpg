using System;
using System.Collections.Generic;
using System.Linq;
using BattleRpg.Hero;
using BattleRpg.Utilities;
using UnityEngine;

namespace BattleRpg.Player
{
    public class PlayerInventory : MonoBehaviour
    {
        private const string BattlesCompletedKey = "BattlesCompleted";

        [SerializeField]
        private List<Hero.Hero> heroList = new List<Hero.Hero>();

        private void Start()
        {
            List<Hero.Hero> unlockedList = heroList.Where(item => item.IsUnlocked()).ToList();
            List<Hero.Hero> lockedList = heroList.Where(item => !item.IsUnlocked()).ToList();

            if (unlockedList.Count < 3)
            {
                Debug.Log("No heroes unlocked in inventory. Unlocking 3 random ones.");
                for (int i = 0; i < 3; i++)
                {
                    Hero.Hero item = lockedList.ElementAt(UnityEngine.Random.Range(0, lockedList.Count));
                    item.UnlockHero();
                    lockedList.Remove(item);
                }
            }

            string battlesCompletedString = PlayerPrefsUtility.GetInventoryItem(BattlesCompletedKey);
            
            if (battlesCompletedString == "")
            {
                Debug.Log("Battles Completed inventory item did not exist, creating one.");
                PlayerPrefsUtility.SetAndSaveInventoryItem(BattlesCompletedKey, "0");
            }
            else
            {
                int battlesCompleted = Int32.Parse(battlesCompletedString);

                if (battlesCompleted % 5 == 0)
                {
                    int requiredUnlockedHeroes = (battlesCompleted / 5) + 3;

                    if (unlockedList.Count < requiredUnlockedHeroes)
                    {
                        lockedList.ElementAt(UnityEngine.Random.Range(0, lockedList.Count)).UnlockHero();
                    }
                }
            }
        }

        public Hero.Hero GetHeroType(HeroType heroType)
        {
            return heroList.First(item => item.GetHeroType() == heroType);
        }
    }
}