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
        [SerializeField]
        private Material selectedMaterial;
        [SerializeField]
        private Material unselectedMaterial;
        [SerializeField]
        private Material lockedMaterial;

        private Vector3 maxScale = new Vector3(1.2f, 1.2f, 1.2f);
        private Vector3 minScale = new Vector3(0.7f, 0.7f, 0.7f);
        private Renderer heroRenderer = null;
        private Collider heroCollider = null;
        private IHeroAttributes heroAttributes;

        public bool Selected { get { return selected; } }
        private bool selected = false;

        private void Awake()
        {
            heroRenderer = gameObject.GetComponent<Renderer>();
            heroRenderer.material = lockedMaterial;

            heroCollider = gameObject.GetComponent<Collider>();
            heroCollider.enabled = false;
        }

        private void Update()
        {
            if (heroCollider.enabled)
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

        public void SetupHero(IHeroAttributes attributes)
        {
            // heroAttributes.ExperiencePoints = attributes.ExperiencePoints;
            // heroAttributes.Level = (attributes.ExperiencePoints / LevelMetric);
            // heroAttributes.Health = BaseHealth + (BaseHealth * heroAttributes.Level * LevelUpPercentage);
            // heroAttributes.AttackPower = BaseAttackPower + (BaseAttackPower * heroAttributes.Level * LevelUpPercentage);

            // Debug.Log(string.Format("Character loaded with attributes - HeroType: {0} Health: {1} Attack Power: {2} Current Exp: {3} Level: {4}", 
            //     heroAttributes.HeroType,
            //     heroAttributes.Health,
            //     heroAttributes.AttackPower,
            //     heroAttributes.ExperiencePoints,
            //     heroAttributes.Level
            // ));
        }

        public string GetHeroName()
        {
            return heroType.ToString();
        }

        public HeroType GetHeroType()
        {
            return heroType;
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

        public void SetUnlockedState()
        {
            heroRenderer.material = unselectedMaterial;
            heroCollider.enabled = true;
        }
    }
}