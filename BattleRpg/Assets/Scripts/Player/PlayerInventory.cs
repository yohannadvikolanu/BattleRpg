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
        [SerializeField]
        private List<HeroType> heroList = new List<HeroType>();

        public List<HeroType> unlockedList = new List<HeroType>();

        public void UpdateInventory()
        {
            heroList.ForEach(item =>
            {
                string itemString = PlayerPrefsUtility.GetInventoryItem(item.ToString());

                if (itemString != "")
                {
                    unlockedList.Add(item);
                }
            });

            if (unlockedList.Count < 3)
            {
                 Debug.Log("No heroes unlocked in inventory. Unlocking 3 random ones.");
                for (int i = 0; i < 3; i++)
                {
                    UnlockHero();
                }
            }
        }

        public void UnlockHero()
        {
            HeroType heroType;
            do
            {
                heroType = heroList.ElementAt(UnityEngine.Random.Range(0, heroList.Count));
            } while (unlockedList.Any(item => item == heroType));

            PlayerPrefsUtility.SetAndSaveInventoryItem(heroType.ToString(), "0");
            unlockedList.Add(heroType);
            Debug.Log(string.Format("Unlocked hero: {0}", heroType));
        }

        public void AddExperiencePoint(HeroType heroType)
        {
            string currentExperiencePoints = PlayerPrefsUtility.GetInventoryItem(heroType.ToString());
            int updatedExperiencePoints = Int32.Parse(currentExperiencePoints) + 1;
            PlayerPrefsUtility.SetAndSaveInventoryItem(heroType.ToString(), updatedExperiencePoints.ToString());
        }
    }
}