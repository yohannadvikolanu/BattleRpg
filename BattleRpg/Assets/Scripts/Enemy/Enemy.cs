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
            enemyAttributes = new EnemyAttributes();

            enemyAttributes.Level = (averageHeroExp / LevelMetric);
            enemyAttributes.Health = BaseHealth + (BaseHealth * enemyAttributes.Level * LevelUpPercentage);
            enemyAttributes.AttackPower = BaseAttackPower + (BaseAttackPower * enemyAttributes.Level * LevelUpPercentage);

            Debug.Log(string.Format("Character loaded with attributes - Health: {0} Attack Power: {1} Level: {2}", 
                enemyAttributes.Health,
                enemyAttributes.AttackPower,
                enemyAttributes.Level
            ));
        }
    }
}