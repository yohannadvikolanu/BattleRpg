using UnityEngine;
using UnityEngine.UI;

namespace BattleRpg.Enemy
{
    public class Enemy : MonoBehaviour
    {
        private const float BaseHealth = 100;
        private const float BaseAttackPower = 7.5f;
        private const int LevelMetric = 3;
        private const float LevelUpPercentage = 0.1f;

        private IEnemyAttributes enemyAttributes;
        private GameObject enemyUiCanvas;
        private Slider healthBar;

        private void Awake()
        {
            healthBar = GetComponentInChildren<Slider>();
            enemyUiCanvas = GetComponentInChildren<Canvas>().gameObject;
            enemyUiCanvas.SetActive(false);
        }

        public void SetupEnemy(int averageHeroExp)
        {
            enemyAttributes = new EnemyAttributes();

            enemyAttributes.Level = (averageHeroExp / LevelMetric);
            enemyAttributes.Health = BaseHealth + (BaseHealth * enemyAttributes.Level * LevelUpPercentage);
            enemyAttributes.AttackPower = BaseAttackPower + (BaseAttackPower * enemyAttributes.Level * LevelUpPercentage);

            Debug.Log(string.Format("Enemy loaded with attributes - Health: {0} Attack Power: {1} Level: {2}", 
                enemyAttributes.Health,
                enemyAttributes.AttackPower,
                enemyAttributes.Level
            ));

            enemyUiCanvas.SetActive(true);
            healthBar.value = 1.0f;
        }

        public float GetCurrentHealth()
        {
            return enemyAttributes.Health;
        }

        public float GetCurrentAttackPower()
        {
            return 100;
        }

        public void DecreaseHealth(float value)
        {
            enemyAttributes.Health -= value;
            Debug.Log("Enemy health decreased by" + value);
            Debug.Log("The health is now at " + enemyAttributes.Health);
            healthBar.value -= value / 100;
        }
    }
}