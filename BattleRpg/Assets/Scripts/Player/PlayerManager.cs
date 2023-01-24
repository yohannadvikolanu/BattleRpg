using System.Collections.Generic;
using UnityEngine;

namespace BattleRpg.Player
{
    public class PlayerManager : MonoBehaviour
    {
        private List<Hero.Hero> battleHeroList;

        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        public void SetHeroListAndStartBattle(List<Hero.Hero> heroList)
        {
            battleHeroList = heroList;
        }
    }
}