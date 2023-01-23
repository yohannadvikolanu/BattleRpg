using UnityEngine;

namespace BattleRpg.Enemy
{
    public class Enemy : MonoBehaviour
    {
        public int averageHeroExp = 0;

        private const float BaseHealth = 100;
        private const float BaseAttackPower = 7.5f;
        private const int LevelMetric = 3;
        private const float LevelUpPercentage = 0.1f;

        private IEnemyAttributes enemyAttributes;

        private void Awake()
        {
            // TODO: Calculate level by using average of all 3 characters and their exp.
            int level = (averageHeroExp / LevelMetric);
            float health = BaseHealth + (BaseHealth * level * LevelUpPercentage);

            float attackPower = BaseAttackPower + (BaseAttackPower * level * LevelUpPercentage);

            enemyAttributes = new EnemyAttributes(health, attackPower, level);

            Debug.Log(string.Format("Character loaded with attributes - Health: {0} Attack Power: {1} Level: {2}", health, attackPower, level));
        }
    }
}