using UnityEngine;

namespace BattleRpg.Hero
{
    public class Hero : MonoBehaviour
    {
        private const float BaseHealth = 100;
        private const float BaseAttackPower = 10;
        private const int LevelMetric = 5;
        private const float LevelUpPercentage = 0.1f;

        [SerializeField]
        private string heroName;

        private IHeroAttributes heroAttributes;

        private void Awake()
        {
            int currentExp = 50;
            // TODO : Check if character exists in storage and load exp.

            int level = (currentExp / LevelMetric);
            float health = BaseHealth + (BaseHealth * level * LevelUpPercentage);

            float attackPower = BaseAttackPower + (BaseAttackPower * level * LevelUpPercentage);

            heroAttributes = new HeroAttributes(heroName, health, attackPower, currentExp, level);

            Debug.Log(string.Format("Character loaded with attributes - Name: {0} Health: {1} Attack Power: {2} Current Exp: {3} Level: {4}", heroName, health, attackPower, currentExp, level));
        }
    }
}