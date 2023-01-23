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
        private Vector3 maxScale = new Vector3(1.2f, 1.2f, 1.2f);
        private Vector3 minScale = new Vector3(0.7f, 0.7f, 0.7f);

        [SerializeField]
        private HeroType heroType;
        [SerializeField]
        private Material selectedMaterial;
        [SerializeField]
        private Material unselectedMaterial;

        private IHeroAttributes heroAttributes;

        public bool Selected { get { return selected; } }
        private bool selected = false;

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

        private void Update()
        {
            if (selected)
            {
                this.transform.localScale = Vector3.Lerp (this.transform.localScale, maxScale, Time.deltaTime * 10);
            }
            else
            {
                this.transform.localScale = Vector3.Lerp (this.transform.localScale, minScale, Time.deltaTime * 10);
            }
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

        public void UpdateSelectedState(bool updateSelected)
        {
            if (updateSelected)
            {
                this.GetComponent<Renderer>().material = selectedMaterial;
                selected = true;
            }
            else
            {
                this.GetComponent<Renderer>().material = unselectedMaterial;
                selected = false;
            }
        }
    }
}