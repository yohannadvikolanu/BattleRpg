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
        private HeroType heroType;

        private IHeroAttributes heroAttributes;

        private void Awake()
        {
            int currentExp = 0;
            // TODO : Check if character exists in storage and load exp.

            int level = (currentExp / LevelMetric);
            float health = BaseHealth + (BaseHealth * level * LevelUpPercentage);

            float attackPower = BaseAttackPower + (BaseAttackPower * level * LevelUpPercentage);

            heroAttributes = new HeroAttributes(heroType, health, attackPower, currentExp, level);

            Debug.Log(string.Format("Character loaded with attributes - HeroType: {0} Health: {1} Attack Power: {2} Current Exp: {3} Level: {4}", heroType, health, attackPower, currentExp, level));
        }

        public string GetHeroName()
        {
            return heroAttributes.HeroType.ToString();
        }

        public HeroType GetHeroType()
        {
            return heroAttributes.HeroType;
        }

        public float GetCurrentHealth()
        {
            return heroAttributes.Health;
        }

        public int GetCurrentExperiencePoints()
        {
            return heroAttributes.ExperiencePoints;
        }

        public float GetCurrentAttackPower()
        {
            return heroAttributes.AttackPower;
        }
        
        public int GetCurrentLevel()
        {
            return heroAttributes.Level;
        }

        public void DecreaseHealth(float value)
        {
            heroAttributes.Health = heroAttributes.Health - value;
        }

        public void AddExperiencePoints(int value)
        {
            heroAttributes.ExperiencePoints = heroAttributes.ExperiencePoints + value;
        }
    }
}