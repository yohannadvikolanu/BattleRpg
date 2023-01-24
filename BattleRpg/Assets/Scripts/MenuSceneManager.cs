using System.Collections.Generic;
using System.Linq;
using BattleRpg.Hero;
using BattleRpg.Player.Manager;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BattleRpg.Menu
{
    public class MenuSceneManager : MonoBehaviour
    {
        private const float HoldTimeThreshold = 2.0f;

        [SerializeField]
        private PlayerManager playerManager = null;

        [SerializeField]
        private Camera mainCamera = null;

        [SerializeField]
        private List<Hero.Hero> heroList = new List<Hero.Hero>();

        [SerializeField]
        private Button startBattle = null;
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

        private List<HeroType> selectedHeroes = new List<HeroType>();
        private Hero.Hero currentlyPressedHero;
        private float holdTime = 0.0f;

        private void OnEnable()
        {
            statsPanel.onClick.AddListener(DismissStatsPanel);
        }

        private void OnDisable()
        {
            statsPanel.onClick.RemoveListener(DismissStatsPanel);
        }

        private void Update()
        {
            if (currentlyPressedHero != null)
            {
                holdTime += Time.deltaTime;

                if (holdTime > HoldTimeThreshold && !statsPanel.gameObject.activeSelf)
                {
                    HandleLongPress(currentlyPressedHero);
                }
            }

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
            if (Input.GetButtonUp("Fire1"))
            {
                if (holdTime <= HoldTimeThreshold)
                {
                    if (currentlyPressedHero != null && currentlyPressedHero.IsUnlocked())
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

            if (selectedHeroes.Count == 3 && !startBattle.interactable)
            {
                startBattle.interactable = true;
            }
            else
            {
                if (startBattle.interactable)
                {
                    startBattle.interactable = false;
                }
            }
        }

        private void HandleLongPress(Hero.Hero hero)
        {
            nameText.text = hero.GetHeroName();
            levelText.text = hero.GetCurrentLevel().ToString();
            attackPowerText.text = hero.GetCurrentAttackPower().ToString();
            experiencePointsText.text = hero.GetCurrentExperiencePoints().ToString();
            statsPanel.gameObject.SetActive(true);
        }

        public void DismissStatsPanel()
        {
            statsPanel.gameObject.SetActive(false);
        }
    }
}