using System;
using BattleRpg.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace BattleRpg.Hero
{
    /// <summary>
    /// Represents the Unity object for the hero.
    /// </summary>
    public class Hero : MonoBehaviour
    {
        // Const variables.
        private const float BaseHealth = 100;
        private const float BaseAttackPower = 10;

        // Defines what a "level up" is
        private const int LevelMetric = 5;      
        // Defines the value for the percentage increase in stats per level up.
        private const float LevelUpPercentage = 0.1f; 

        // In-Editor reference variables.
        [SerializeField]
        private HeroType heroType;
        [SerializeField]
        private Material selectedMaterial;
        [SerializeField]
        private Material unselectedMaterial;
        [SerializeField]
        private Material lockedMaterial;

        // Private variables.
        private Vector3 maxScale = new Vector3(1.2f, 1.2f, 1.2f);
        private Vector3 minScale = new Vector3(0.7f, 0.7f, 0.7f);
        private Renderer heroRenderer = null;
        private Collider heroCollider = null;
        private IHeroAttributes heroAttributes;
        private GameObject heroUiCanvas;
        private Slider healthBar;
        private bool isInBattle = false;
        private bool selected = false;

        // Public variables.
        public bool Selected { get { return selected; } }

        private void Awake()
        {
            // Gathering the required components and setting them to the appropriate states.
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
            // If the hero is unlocked.
            if (!isInBattle && heroCollider.enabled)
            {
                // Setting the scale to visualise selection on the menu screen.
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

        /// <summary>
        /// Setting up the hero object for the menu or battle.
        /// </summary>
        /// <param name="isForBattle">Specify whether the setup is for a battle.</param>
        public void SetupHero(bool isForBattle)
        {
            // Retrieving the exp. for the hero from the inventory.
            heroAttributes = new HeroAttributes();
            string experiencePointsString = PlayerPrefsUtility.GetInventoryItem(heroType.ToString());

            // Setting up the hero attributes based on the exp.
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

            // Setting up the hero object for battle, if required.
            if (isForBattle)
            {
                heroRenderer.material = selectedMaterial;
                heroCollider.enabled = true;
                heroUiCanvas.SetActive(true);
                healthBar.value = 1.0f;
                isInBattle = true;
            }
        }

        // Accesors.
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

        /// <summary>
        /// Allows the hero object selection state to be updated.
        /// </summary>
        /// <param name="isSelected">Specifying the state we require.</param>
        public void UpdateSelectedState(bool isSelected)
        {
            if (isSelected)
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

        /// <summary>
        /// Sets the hero object to be unlocked "visually"
        /// </summary>
        public void SetUnlockedState()
        {
            heroRenderer.material = unselectedMaterial;
            heroCollider.enabled = true;

            // Setting up the hero attributes if the hero is unlocked.
            SetupHero(false);
        }

        /// <summary>
        /// Allows decreasing the hero's health during battle.
        /// </summary>
        /// <param name="value">Value to decrease the health by.</param>
        public void DecreaseHealth(float value)
        {
            heroAttributes.Health -= value;
            Debug.Log("Hero health decreased by" + value);
            Debug.Log("The health is now at " + heroAttributes.Health);
            healthBar.value -= value / 100;
        }

        /// <summary>
        /// Setting the hero to visually appear in death state.
        /// </summary>
        public void SetDeathState()
        {
            heroRenderer.material = lockedMaterial;
            heroCollider.enabled = false;   
        }
    }
}