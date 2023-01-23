using System.Collections.Generic;
using System.Linq;
using BattleRpg.Hero;
using UnityEngine;

namespace BattleRpg.Player.Inventory
{
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField]
        private List<Hero.Hero> heroList = new List<Hero.Hero>();

        private void Start()
        {
            List<Hero.Hero> unlockedList = heroList.Where(item => item.IsUnlocked()).ToList();

            if (unlockedList.Count < 3)
            {
                Debug.Log("No heroes unlocked in inventory. Unlocking 3 random ones.");
                heroList.ElementAt(0).UnlockHero();
                heroList.ElementAt(1).UnlockHero();
                heroList.ElementAt(2).UnlockHero();
            }            
        }

        public Hero.Hero GetHeroType(HeroType heroType)
        {
            return heroList.First(item => item.GetHeroType() == heroType);
        }
    }
}