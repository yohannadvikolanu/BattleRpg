using System.Collections.Generic;
using System.Linq;
using BattleRpg.Player;
using BattleRpg.Hero;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BattleRpg.Menu
{
    /// <summary>
    /// Represents the class that manages the menu scene.
    /// </summary>
    public class MenuSceneManager : MonoBehaviour
    {
        // Const variables.
        private const float HoldTimeThreshold = 2.0f;

        // In-Editor reference variables.
        [SerializeField]
        private Camera mainCamera = null;

        [SerializeField]
        private List<Hero.Hero> heroList = new List<Hero.Hero>();

        [SerializeField]
        private Button startBattleButton = null;
        [SerializeField]
        private Button statsPanel = null;
        [SerializeField]
        private TMP_Text nameText = null;
        [SerializeField]
        private TMP_Text levelText = null;
        [SerializeField]
        private TMP_Text attackPowerText = null;
        [SerializeField]
        private TMP_Text experiencePointsText = null;

        // Private variables.
        private List<HeroType> selectedHeroes = new List<HeroType>();
        private Hero.Hero currentlyPressedHero;
        private float holdTime = 0.0f;

        private void Start()
        {
            SetupScene();
        }

        private void OnEnable()
        {
            statsPanel.onClick.AddListener(DismissStatsPanel);
            startBattleButton.onClick.AddListener(OnStartBattleButtonClicked);
        }

        private void OnDisable()
        {
            statsPanel.onClick.RemoveListener(DismissStatsPanel);
            startBattleButton.onClick.RemoveListener(OnStartBattleButtonClicked);
        }

        private void Update()
        {
            // Logic to handle Long Press occurences.
            if (currentlyPressedHero != null)
            {
                holdTime += Time.deltaTime;

                if (holdTime > HoldTimeThreshold && !statsPanel.gameObject.activeSelf)
                {
                    HandleLongPress(currentlyPressedHero);
                }
            }

            // Selecting a hero based on the raycast.
            if (Input.GetButtonDown("Fire1"))
            {
                if (currentlyPressedHero == null && !statsPanel.gameObject.activeSelf)
                {
                    // Try a raycast from screenpoint to check whether we hit anything.
                    Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    // If we did hit something, check which collider we hit.
                    if (Physics.Raycast(ray, out hit))
                    {
                        currentlyPressedHero = heroList.First(item => item.GetHeroName() == hit.collider.name);
                    }
                }
            }

            // Checking whether this was a long press.
            if (Input.GetButtonUp("Fire1"))
            {
                if (holdTime <= HoldTimeThreshold)
                {
                    if (currentlyPressedHero != null)
                    {
                        if (currentlyPressedHero.Selected)
                        {
                            currentlyPressedHero.UpdateSelectedState(false);
                            selectedHeroes.Remove(currentlyPressedHero.GetHeroType());
                        }
                        else
                        {
                            if (selectedHeroes.Count < 3)
                            {
                                currentlyPressedHero.UpdateSelectedState(true);
                                selectedHeroes.Add(currentlyPressedHero.GetHeroType());
                            }
                        }
                    }
                }

                holdTime = 0.0f;
                currentlyPressedHero = null;
            }

            // Updating UI based on selected number of heroes.
            if (selectedHeroes.Count == 3 && !startBattleButton.interactable)
            {
                startBattleButton.interactable = true;
            }
            
            if (selectedHeroes.Count < 3 && startBattleButton.interactable)
            {
                startBattleButton.interactable = false;
            }
        }

        /// <summary>
        /// Opens the pop-up with the specific hero stats.
        /// </summary>
        /// <param name="hero">The hero we need to display stats for.</param>
        private void HandleLongPress(Hero.Hero hero)
        {
            nameText.text = hero.GetHeroName();
            levelText.text = hero.GetCurrentLevel().ToString();
            attackPowerText.text = hero.GetCurrentAttackPower().ToString();
            experiencePointsText.text = hero.GetCurrentExperiencePoints().ToString();
            statsPanel.gameObject.SetActive(true);
        }

        /// <summary>
        /// Triggers a start battle when the button is pressed.
        /// </summary>
        private void OnStartBattleButtonClicked()
        {
            PlayerManager.Instance.SetHeroListAndStartBattle(selectedHeroes);
        }

        /// <summary>
        /// Hides the stats panel.
        /// </summary>
        private void DismissStatsPanel()
        {
            statsPanel.gameObject.SetActive(false);
        }

        /// <summary>
        /// Sets up the menu scene based on the inventory.
        /// </summary>
        private void SetupScene()
        {
            for (int i = 0; i < heroList.Count; i++)
            {
                PlayerInventory.Instance.unlockedList.ForEach(item =>
                {
                    if (item == heroList[i].GetHeroType())
                    {
                        heroList[i].SetUnlockedState();
                    }
                });
            }
        }
    }
}