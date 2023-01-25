using System.Collections.Generic;
using BattleRpg.Player;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using System.Collections;

namespace BattleRpg.Battle
{
    public class BattleSceneManager : MonoBehaviour
    {
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

        private List<Hero.Hero> alivebattleHeroes = new List<Hero.Hero>();
        private List<Hero.Hero> deadBattleHeroes = new List<Hero.Hero>();
        private Enemy.Enemy battleEnemy = null;
        private Hero.Hero targetHero = null;

        private bool isEnemyTurn = false;
        private bool isEnemyMoving = false;
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
        }

        private void Update()
        {
            if (Input.GetButtonDown("Fire1") && !isEnemyTurn && !isEnemyMoving)
            {
                // Try a raycast from screenpoint to check whether we hit anything.
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                // If we did hit something, check which collider we hit.
                if (Physics.Raycast(ray, out hit))
                {
                    Hero.Hero currentHero = alivebattleHeroes.First(item => item.GetHeroName() == hit.collider.name);
                    PerformHeroAttack(currentHero);
                    isEnemyTurn = true;
                }
            }

            if (isEnemyTurn && !isEnemyMoving && targetHero == null)
            {
                targetHero = alivebattleHeroes[UnityEngine.Random.Range(0, alivebattleHeroes.Count)];
                isEnemyMoving = true;
            }
            else if (isEnemyTurn && targetHero != null && battleEnemy.transform.position != targetHero.transform.position)
            {
                PerformAnimation(battleEnemy.transform, targetHero.transform.position);
            }
            else if (isEnemyTurn && targetHero != null && battleEnemy.transform.position == targetHero.transform.position)
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
            }

            if (battleEnemy.GetCurrentHealth() <= 0.0f && !battleEndUi.activeSelf)
            {
                battleStatusText.text = "YOU WON!";
                battleEndUi.SetActive(true);
                isWin = true;
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

            if (alivebattleHeroes.Count == 0)
            {
                battleStatusText.text = "YOU LOST!";
                battleEndUi.SetActive(true);
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

        private void PerformHeroAttack(Hero.Hero currentHero)
        {
            battleEnemy.DecreaseHealth(currentHero.GetCurrentAttackPower());
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