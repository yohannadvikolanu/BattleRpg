using System.Collections.Generic;
using BattleRpg.Player;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

namespace BattleRpg.Battle
{
    public class BattleSceneManager : MonoBehaviour
    {
        private const string PlayerTurnText = "YOUR TURN";
        private const string EnemyTurnText = "ENEMY'S TURN";

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
            SetupHeroes();
            SetupEnemy();
            isPlayerTurn = true;
            turnUi.SetActive(true);
            turnText.text = PlayerTurnText;
        }

        private void Update()
        {
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
                        currentHero = alivebattleHeroes.First(item => item.GetHeroName() == hit.collider.name);
                        currentHeroOriginalPosition = currentHero.transform.position;
                        heroAttackReferencePosition = battleEnemy.transform.position + (Vector3.left * 2);
                        isPlayerMoving = true;
                    }
                }

                if (isPlayerTurn && isPlayerMoving && currentHero != null && currentHero.transform.position != heroAttackReferencePosition)
                {
                    PerformAnimation(currentHero.transform, heroAttackReferencePosition);
                }
                else if (isPlayerTurn && isPlayerMoving && currentHero != null && currentHero.transform.position == heroAttackReferencePosition)
                {
                    PerformHeroAttack();
                }
                else if (!isPlayerTurn && isPlayerMoving && currentHero != null && currentHero.transform.position != currentHeroOriginalPosition)
                {
                    PerformAnimation(currentHero.transform, currentHeroOriginalPosition);
                }
                else if (!isPlayerTurn && isPlayerMoving && currentHero != null && currentHero.transform.position == currentHeroOriginalPosition)
                {
                    isPlayerMoving = false;
                    isEnemyTurn = true;
                    turnText.text = EnemyTurnText;
                }


                // Enemy move logic starts here
                if (isEnemyTurn && !isEnemyMoving && targetHero == null)
                {
                    targetHero = alivebattleHeroes[UnityEngine.Random.Range(0, alivebattleHeroes.Count)];
                    enemyAttackReferencePosition = targetHero.transform.position + (Vector3.right * 2);
                    isEnemyMoving = true;
                }
                else if (isEnemyTurn && targetHero != null && battleEnemy.transform.position != enemyAttackReferencePosition)
                {
                    PerformAnimation(battleEnemy.transform, enemyAttackReferencePosition);
                }
                else if (isEnemyTurn && targetHero != null && battleEnemy.transform.position == enemyAttackReferencePosition)
                {
                    PerformEnemyAttack();
                    isEnemyTurn = false;
                }
                else if (!isEnemyTurn && isEnemyMoving && battleEnemy.transform.position != enemyPrefabReferencePosition.position)
                {
                    PerformAnimation(battleEnemy.transform, enemyPrefabReferencePosition.position);
                }
                else if (!isEnemyTurn && isEnemyMoving && battleEnemy.transform.position == enemyPrefabReferencePosition.position)
                {
                    isEnemyMoving = false;
                    isPlayerTurn = true;
                    turnText.text = PlayerTurnText;
                }
            }

            if (alivebattleHeroes.Count > 0)
            {
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

            if (battleEnemy.GetCurrentHealth() <= 0.0f && !battleEndUi.activeSelf)
            {
                battleStatusText.text = "YOU WON!";
                battleEndUi.SetActive(true);
                turnUi.SetActive(true);
                isWin = true;
                battleEnemy.SetDeathState();
            }

            if (alivebattleHeroes.Count == 0)
            {
                battleStatusText.text = "YOU LOST!";
                battleEndUi.SetActive(true);
                turnUi.SetActive(false);
            }
        }

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

        private void PerformHeroAttack()
        {
            battleEnemy.DecreaseHealth(currentHero.GetCurrentAttackPower());
            isPlayerTurn = false;
        }

        private void PerformEnemyAttack()
        {
            targetHero.DecreaseHealth(battleEnemy.GetCurrentAttackPower());
            targetHero = null;
        }

        private void PerformAnimation(Transform lerpObject, Vector3 targetPosition)
        {
            lerpObject.position = Vector3.Lerp(lerpObject.position, targetPosition, Time.deltaTime * 10);
        }

        private void EndBattle()
        {
            if (isWin)
            {
                alivebattleHeroes.ForEach(item => PlayerInventory.Instance.AddExperiencePoint(item.GetHeroType()));
            }

            PlayerManager.Instance.BattleCompleted();
        }
    }
}