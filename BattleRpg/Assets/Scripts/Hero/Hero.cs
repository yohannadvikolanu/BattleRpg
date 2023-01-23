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
        [SerializeField]
        private Material lockedMaterial;

        private IHeroAttributes heroAttributes;

        public bool Selected { get { return selected; } }
        private bool selected = false;

        private void Awake()
        {
            heroAttributes = new HeroAttributes(heroType);
            string heroDataString = PlayerPrefsUtility.GetInventoryHero(heroType.ToString());

            if (heroDataString == "")
            {
                Debug.Log("Hero did not exist in inventory, creating one.");
                heroDataString = JsonUtility.ToJson(new HeroDataModel { ExperiencePoints = 0, Unlocked = false });
                PlayerPrefsUtility.SetAndSaveInventoryHero(heroType.ToString(), heroDataString);
            }

            SetupHero(JsonUtility.FromJson<HeroDataModel>(heroDataString));
        }

        private void Update()
        {
            if (heroAttributes.Unlocked)
            {
                if (selected)
                {
                    this.transform.localScale = Vector3.Lerp(this.transform.localScale, maxScale, Time.deltaTime * 10);
                }
                else
                {
                    this.transform.localScale = Vector3.Lerp(this.transform.localScale, minScale, Time.deltaTime * 10);
                }
            }
        }

        private void SetupHero(HeroDataModel heroData)
        {
            if (heroAttributes.Unlocked)
            {
                heroAttributes.ExperiencePoints = heroData.ExperiencePoints;
                heroAttributes.Level = (heroData.ExperiencePoints / LevelMetric);
                heroAttributes.Health = BaseHealth + (BaseHealth * heroAttributes.Level * LevelUpPercentage);
                heroAttributes.AttackPower = BaseAttackPower + (BaseAttackPower * heroAttributes.Level * LevelUpPercentage);

                Debug.Log(string.Format("Character loaded with attributes - HeroType: {0} Health: {1} Attack Power: {2} Current Exp: {3} Level: {4}", 
                    heroAttributes.HeroType,
                    heroAttributes.Health,
                    heroAttributes.AttackPower,
                    heroData.ExperiencePoints,
                    heroAttributes.Level
                ));

                this.GetComponent<Renderer>().material = unselectedMaterial;

                return;
            }
            else
            {
                this.GetComponent<Renderer>().material = lockedMaterial;
            }


            Debug.Log(string.Format("Character {0} has not yet been unlocked.", heroAttributes.HeroType.ToString()));
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

        public bool IsUnlocked()
        {
            return heroAttributes.Unlocked;
        }

        public void UnlockHero()
        {
            heroAttributes.Unlocked = true;
            SetupHero(new HeroDataModel { ExperiencePoints = heroAttributes.ExperiencePoints, Unlocked = heroAttributes.Unlocked });

            string heroDataString = JsonUtility.ToJson(new HeroDataModel { ExperiencePoints = heroAttributes.ExperiencePoints, Unlocked = heroAttributes.Unlocked });
            PlayerPrefsUtility.SetAndSaveInventoryHero(heroType.ToString(), heroDataString);       
        }
    }
}