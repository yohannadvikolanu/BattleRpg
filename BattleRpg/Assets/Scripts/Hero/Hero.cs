using System;
using BattleRpg.Utilities;
using UnityEngine;
using UnityEngine.UI;

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
        private GameObject heroUiCanvas;
        private Slider healthBar;

        public bool Selected { get { return selected; } }
        private bool selected = false;

        private void Awake()
        {
            healthBar = GetComponentInChildren<Slider>();
            heroRenderer = gameObject.GetComponent<Renderer>();
            heroRenderer.material = lockedMaterial;

            heroCollider = gameObject.GetComponent<Collider>();
            heroCollider.enabled = false;

            heroUiCanvas = GetComponentInChildren<Canvas>().gameObject;
            heroUiCanvas.SetActive(false);
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

        public void SetupHero(bool isForBattle)
        {
            heroAttributes = new HeroAttributes();
            string experiencePointsString = PlayerPrefsUtility.GetInventoryItem(heroType.ToString());

            heroAttributes.ExperiencePoints = Int32.Parse(experiencePointsString);
            heroAttributes.Level = (heroAttributes.ExperiencePoints / LevelMetric);
            heroAttributes.Health = BaseHealth + (BaseHealth * heroAttributes.Level * LevelUpPercentage);
            heroAttributes.AttackPower = BaseAttackPower + (BaseAttackPower * heroAttributes.Level * LevelUpPercentage);

            Debug.Log(string.Format("Hero loaded with attributes - HeroType: {0} Health: {1} Attack Power: {2} Current Exp: {3} Level: {4}",
                heroType,
                heroAttributes.Health,
                heroAttributes.AttackPower,
                heroAttributes.ExperiencePoints,
                heroAttributes.Level
            ));

            if (isForBattle)
            {
                heroRenderer.material = selectedMaterial;
                heroCollider.enabled = true;
                heroUiCanvas.SetActive(true);
                healthBar.value = 1.0f;
            }
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

            SetupHero(false);
        }

        public void DecreaseHealth(float value)
        {
            heroAttributes.Health -= value;
            Debug.Log("Hero health decreased by" + value);
            Debug.Log("The health is now at " + heroAttributes.Health);
            healthBar.value -= value / 100;
        }

        public void SetDeathState()
        {
            heroRenderer.material = lockedMaterial;
            heroCollider.enabled = false;   
        }
    }
}