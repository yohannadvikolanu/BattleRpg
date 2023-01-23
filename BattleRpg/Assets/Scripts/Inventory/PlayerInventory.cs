using System.Collections.Generic;
using System.Linq;
using BattleRpg.Hero;
using UnityEngine;

namespace BattleRpg.Player.Inventory
{
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> heroPrefabs = new List<GameObject>();

        private List<Hero.Hero> heroList = new List<Hero.Hero>();

        private void Start()
        {
            heroPrefabs.ForEach(item => heroList.Add(item.GetComponent<Hero.Hero>()));
        }

        public Hero.Hero GetHeroType(HeroType heroType)
        {
            return heroList.First(item => item.GetHeroType() == heroType);
        }
    }
}