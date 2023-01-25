using UnityEngine;
using UnityEngine.UI;

namespace BattleRpg.Enemy
{
    /// <summary>
    /// Represents the Unity object for the enemy.
    /// </summary>
    public class Enemy : MonoBehaviour
    {
        // Const variables.
        private const float BaseHealth = 100;
        private const float BaseAttackPower = 7.5f;
        private const int LevelMetric = 3;
        private const float LevelUpPercentage = 0.1f;

        // In-Editor reference variables.
        [SerializeField]
        private Material lockedMaterial = null;

        // Private variables.
        private IEnemyAttributes enemyAttributes;
        private GameObject enemyUiCanvas;
        private Slider healthBar;
        private Renderer enemyRenderer = null;

        private void Awake()
        {
            // Gathering the required components and setting them to the appropriate states.
            healthBar = GetComponentInChildren<Slider>();
            enemyUiCanvas = GetComponentInChildren<Canvas>().gameObject;
            enemyUiCanvas.SetActive(false);

            enemyRenderer = gameObject.GetComponent<Renderer>();
        }

        /// <summary>
        /// Setting up the enemy object for battle.
        /// </summary>
        /// <param name="averageHeroExp">Value specifying the average hero exp.</param>
        public void SetupEnemy(int averageHeroExp)
        {
            // Using the average hero exp. to setup the enemy.
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

        // Accesors.
        public float GetCurrentHealth()
        {
            return enemyAttributes.Health;
        }

        public float GetCurrentAttackPower()
        {
            return enemyAttributes.AttackPower;
        }

        /// <summary>
        /// Allows decreasing the enemy's health during battle.
        /// </summary>
        /// <param name="value">Value to decrease the health by.</param>
        public void DecreaseHealth(float value)
        {
            enemyAttributes.Health -= value;
            Debug.Log("Enemy health decreased by" + value);
            Debug.Log("The health is now at " + enemyAttributes.Health);
            healthBar.value -= value / 100;
        }

        /// <summary>
        /// Setting the enemy to visually appear in death state.
        /// </summary>
        public void SetDeathState()
        {
            enemyRenderer.material = lockedMaterial;   
        }
    }
}