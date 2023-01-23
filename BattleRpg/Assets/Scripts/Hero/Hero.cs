using BattleRpg.Utilities;
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
            HeroDataModel heroData;
            string heroDataString = PlayerPrefsUtility.GetInventoryHero(heroType.ToString());

            if (heroDataString == "")
            {
                Debug.Log("Hero did not exist in inventory, creating one.");
                heroData = new HeroDataModel { ExperiencePoints = 0, Unlocked = false };
                heroDataString = JsonUtility.ToJson(heroData);
                PlayerPrefsUtility.SetAndSaveInventoryHero(heroType.ToString(), heroDataString);
            }
            else
            {
                heroData = JsonUtility.FromJson<HeroDataModel>(heroDataString);
            }

            int level = 0;
            float health = 0.0f;
            float attackPower = 0.0f;

            if (heroData.Unlocked)
            {
                level = (heroData.ExperiencePoints / LevelMetric);
                health = BaseHealth + (BaseHealth * level * LevelUpPercentage);
                attackPower = BaseAttackPower + (BaseAttackPower * level * LevelUpPercentage);
            }

            heroAttributes = new HeroAttributes(heroType, health, attackPower, heroData.ExperiencePoints, level, heroData.Unlocked);

            Debug.Log(string.Format("Character loaded with attributes - HeroType: {0} Health: {1} Attack Power: {2} Current Exp: {3} Level: {4}", heroType, health, attackPower, heroData.ExperiencePoints, level));
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