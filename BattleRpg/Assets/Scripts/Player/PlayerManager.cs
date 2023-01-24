using System.Collections.Generic;
using UnityEngine;

namespace BattleRpg.Player.Manager
{
    public class PlayerManager : MonoBehaviour
    {
        private List<Hero.Hero> battleHeroList;

        public void SetHeroListAndStartBattle(List<Hero.Hero> heroList)
        {
            battleHeroList = heroList;
        }
    }
}