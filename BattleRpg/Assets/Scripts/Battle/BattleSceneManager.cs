using System.Collections.Generic;
using BattleRpg.Player;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

namespace BattleRpg.Battle
{
    /// <summary>
    /// Represents the class that manages the battle scene.
    /// </summary>
    public class BattleSceneManager : MonoBehaviour
    {
        // Const variables.
        private const string PlayerTurnText = "YOUR TURN";
        private const string EnemyTurnText = "ENEMY'S TURN";

        // In-Editor reference variables.
        [SerializeField]
        private Camera mainCamera = null;
        [SerializeField]
        private List<GameObject> heroPrefabs = new List<GameObject>();
        [SerializeField]
        private GameObject enemyPrefab = null;

        [SerializeField]
        private Transform firstHeroReferencePosition;
        [SerializeField]
        private Transform secondHeroReferencePosition;
        [SerializeField]
        private Transform thirdHeroReferencePosition;
        [SerializeField]
        private Transform enemyPrefabReferencePosition;

        [SerializeField]
        private GameObject battleEndUi = null;
        [SerializeField]
        private Button battleEndPopup = null;
        [SerializeField]
        private TMP_Text battleStatusText = null;
        [SerializeField]
        private GameObject turnUi = null;
        [SerializeField]
        private TMP_Text turnText = null;

        // Private variables.
        private List<Hero.Hero> alivebattleHeroes = new List<Hero.Hero>();
        private List<Hero.Hero> deadBattleHeroes = new List<Hero.Hero>();
        private Enemy.Enemy battleEnemy = null;
        private Hero.Hero targetHero = null;
        private Hero.Hero currentHero = null;
        private Vector3 currentHeroOriginalPosition;
        private Vector3 enemyAttackReferencePosition;
        private Vector3 heroAttackReferencePosition;

        private bool isEnemyTurn = false;
        private bool isEnemyMoving = false;
        private bool isPlayerTurn = false;
        private bool isPlayerMoving = false;
        private bool performEnemyHurtAnimation = false;
        private bool performHeroHurtAnimation = false;
        private bool isWin = false;

        private void Awake()
        {
            battleEndUi.SetActive(false);
        }

        private void OnEnable()
        {
            battleEndPopup.onClick.AddListener(EndBattle);
        }

        private void OnDisable()
        {
            battleEndPopup.onClick.RemoveListener(EndBattle);
        }

        private void Start()
        {
            // Setting up scene for battle.
            SetupHeroes();
            SetupEnemy();
            isPlayerTurn = true;
            turnUi.SetActive(true);
            turnText.text = PlayerTurnText;
        }

        private void Update()
        {
            // Making sure the battle hasn't ended.
            if (!battleEndUi.activeSelf)
            {
                // Player attack logic starts here
                if (Input.GetButtonDown("Fire1") && !isEnemyTurn && !isEnemyMoving)
                {
                    // Try a raycast from screenpoint to check whether we hit anything.
                    Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    // If we did hit something, check which collider we hit.
                    if (Physics.Raycast(ray, out hit))
                    {
                        // Selecting the correct hero as the "current hero"
                        // And setting up the required variables to perform the attack animation
                        currentHero = alivebattleHeroes.First(item => item.GetHeroName() == hit.collider.name);
                        currentHeroOriginalPosition = currentHero.transform.position;
                        heroAttackReferencePosition = battleEnemy.transform.position + (Vector3.left * 2);
                        isPlayerMoving = true;
                    }
                }

                // Hero attack animation logic tree
                if (isPlayerTurn && isPlayerMoving && currentHero.transform.position != heroAttackReferencePosition)
                {
                    // Move the current hero the attack position
                    PerformAnimation(currentHero.transform, heroAttackReferencePosition);
                }
                else if (isPlayerTurn && isPlayerMoving && currentHero.transform.position == heroAttackReferencePosition)
                {
                    // Once move is done, perform the attack
                    PerformHeroAttack();
                    performEnemyHurtAnimation = true;
                }
                else if (!isPlayerTurn && isPlayerMoving && currentHero.transform.position != currentHeroOriginalPosition)
                {
                    // Return the current hero to the original position
                    PerformAnimation(currentHero.transform, currentHeroOriginalPosition);
                }
                else if (!isPlayerTurn && isPlayerMoving && currentHero.transform.position == currentHeroOriginalPosition)
                {
                    // Complete player turn and start enemy turn
                    isPlayerMoving = false;
                    isEnemyTurn = true;
                    turnText.text = EnemyTurnText;
                }

                if (performEnemyHurtAnimation && battleEnemy.transform.localScale != Vector3.one)
                {
                    PerformHurtAnimation(battleEnemy.transform, Vector3.one);
                }
                else if (performEnemyHurtAnimation && battleEnemy.transform.localScale == Vector3.one)
                {
                    performEnemyHurtAnimation = false;
                }
                else if (!performEnemyHurtAnimation && battleEnemy.transform.localScale != Vector3.one * 2.0f)
                {
                    PerformHurtAnimation(battleEnemy.transform, Vector3.one * 2.0f);
                }

                if (targetHero != null)
                {
                    if (performHeroHurtAnimation && targetHero.transform.localScale != Vector3.one * 0.5f)
                    {
                        PerformHurtAnimation(targetHero.transform, Vector3.one * 0.5f);
                    }
                    else if (performHeroHurtAnimation && targetHero.transform.localScale == Vector3.one * 0.5f)
                    {
                        performHeroHurtAnimation = false;
                    }
                    else if (!performHeroHurtAnimation && targetHero.transform.localScale != Vector3.one)
                    {
                        PerformHurtAnimation(targetHero.transform, Vector3.one);
                    }
                }

                // Enemy attack animation logic tree
                if (isEnemyTurn && !isEnemyMoving)
                {
                    // If it's the enemy's turn, set up required variables if they haven't been already
                    targetHero = alivebattleHeroes[UnityEngine.Random.Range(0, alivebattleHeroes.Count)];
                    enemyAttackReferencePosition = targetHero.transform.position + (Vector3.right * 2);
                    isEnemyMoving = true;
                }
                else if (isEnemyTurn && isEnemyMoving && battleEnemy.transform.position != enemyAttackReferencePosition)
                {
                    // Move the enemy to the attack position
                    PerformAnimation(battleEnemy.transform, enemyAttackReferencePosition);
                }
                else if (isEnemyTurn && isEnemyMoving && battleEnemy.transform.position == enemyAttackReferencePosition)
                {
                    // Perform the attack logic on the selected hero
                    PerformEnemyAttack();
                    performHeroHurtAnimation = true;
                }
                else if (!isEnemyTurn && isEnemyMoving && battleEnemy.transform.position != enemyPrefabReferencePosition.position)
                {
                    // Return the enemy back to it's original position
                    PerformAnimation(battleEnemy.transform, enemyPrefabReferencePosition.position);
                }
                else if (!isEnemyTurn && isEnemyMoving && battleEnemy.transform.position == enemyPrefabReferencePosition.position)
                {
                    // Complete the enemy turn and start player turn
                    isEnemyMoving = false;
                    isPlayerTurn = true;
                    turnText.text = PlayerTurnText;
                }
            }

            // Checking if we have alive heroes
            if (alivebattleHeroes.Count > 0)
            {
                // Checking whether the alive heroes are actually still alive
                // If not, set them to the death state
                for (int i = 0; i < alivebattleHeroes.Count; i++)
                {
                    if(alivebattleHeroes[i].GetCurrentHealth() <= 0.0f)
                    {
                        alivebattleHeroes[i].SetDeathState();
                        deadBattleHeroes.Add(alivebattleHeroes[i]);
                        alivebattleHeroes.Remove(alivebattleHeroes[i]);
                        return;
                    }
                }
            }

            // Checking if the enemy is dead and battle is still active
            if (battleEnemy.GetCurrentHealth() <= 0.0f && !battleEndUi.activeSelf)
            {
                // Set the battle complete state
                battleStatusText.text = "YOU WON!";
                battleEndUi.SetActive(true);
                turnUi.SetActive(true);
                isWin = true;
                battleEnemy.SetDeathState();
            }

            // Checking if don't have any more alive heroes
            if (alivebattleHeroes.Count == 0)
            {
                // Set the battle complete state
                battleStatusText.text = "YOU LOST!";
                battleEndUi.SetActive(true);
                turnUi.SetActive(false);
            }
        }

        /// <summary>
        /// Sets up the selected heroes for battle.
        /// </summary>
        private void SetupHeroes()
        {
            for (int i = 0; i < PlayerManager.Instance.BattleHeroList.Count; i++)
            {
                heroPrefabs.ForEach(item =>
                {
                    if (item.GetComponent<Hero.Hero>().GetHeroType() == PlayerManager.Instance.BattleHeroList[i])
                    {
                        Hero.Hero newHero = Instantiate(item, PrefabPosition(i), Quaternion.identity).GetComponent<Hero.Hero>();
                        newHero.SetupHero(true);
                        newHero.name = newHero.GetHeroName();
                        alivebattleHeroes.Add(newHero);
                    }
                });
            }
        }

        /// <summary>
        /// Sets up the enemy object.
        /// </summary>
        private void SetupEnemy()
        {
            int totalExperiencePoints = 0;

            for (int i = 0; i < alivebattleHeroes.Count; i++)
            {
                totalExperiencePoints += alivebattleHeroes[i].GetCurrentExperiencePoints();
            }

            totalExperiencePoints = totalExperiencePoints / alivebattleHeroes.Count;

            Enemy.Enemy newEnemy = Instantiate(enemyPrefab, enemyPrefabReferencePosition.position, Quaternion.identity).GetComponent<Enemy.Enemy>();
            newEnemy.SetupEnemy(totalExperiencePoints);
            battleEnemy = newEnemy;
        }

        private Vector3 PrefabPosition(int heroNumber)
        {
            switch (heroNumber)
            {
                case 0:
                    return firstHeroReferencePosition.position;
                
                case 1:
                    return secondHeroReferencePosition.position;

                case 2:
                    return thirdHeroReferencePosition.position;

                default:
                    return Vector3.zero;
            }
        }

        /// <summary>
        /// Peform an attack for the current hero.
        /// </summary>
        private void PerformHeroAttack()
        {
            battleEnemy.DecreaseHealth(currentHero.GetCurrentAttackPower());
            isPlayerTurn = false;
        }

        /// <summary>
        /// Perform an attack for the enemy.
        /// </summary>
        private void PerformEnemyAttack()
        {
            targetHero.DecreaseHealth(battleEnemy.GetCurrentAttackPower());
            isEnemyTurn = false;
        }

        /// <summary>
        /// Used for animating the hero and enemy objects around the battle scene for visual depiction of the battle.
        /// </summary>
        /// <param name="lerpObject">The object to lerp.</param>
        /// <param name="targetPosition">The target to lerp to.</param>
        private void PerformAnimation(Transform lerpObject, Vector3 targetPosition)
        {
            lerpObject.position = Vector3.Lerp(lerpObject.position, targetPosition, Time.deltaTime * 10);
        }

        private void PerformHurtAnimation(Transform lerpObject, Vector3 targetScale)
        {
            lerpObject.localScale = Vector3.Lerp(lerpObject.localScale, targetScale, Time.deltaTime * 30);
        }

        /// <summary>
        /// Peforms required logic at the end of a battle and transitions back to the menu scene.
        /// </summary>
        private void EndBattle()
        {
            // If it was a win, add an experience point to each alive hero.
            if (isWin)
            {
                alivebattleHeroes.ForEach(item => PlayerInventory.Instance.AddExperiencePoint(item.GetHeroType()));
            }

            PlayerManager.Instance.BattleCompleted();
        }
    }
}