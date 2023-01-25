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

        private List<Hero.Hero> battleHeroes = new List<Hero.Hero>();
        private Enemy.Enemy battleEnemy = null;

        private bool isEnemyTurn = false;

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
            if (Input.GetButtonDown("Fire1") && !isEnemyTurn)
            {
                // Try a raycast from screenpoint to check whether we hit anything.
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                // If we did hit something, check which collider we hit.
                if (Physics.Raycast(ray, out hit))
                {
                    Hero.Hero currentHero = battleHeroes.First(item => item.GetHeroName() == hit.collider.name);
                    PerformHeroAttack(currentHero);
                    isEnemyTurn = true;
                }
            }

            if (isEnemyTurn)
            {
                PerformEnemyAttack();
            }

            if (battleEnemy.GetCurrentHealth() <= 0.0f && !battleEndUi.activeSelf)
            {
                battleStatusText.text = "YOU WON!";
                battleEndUi.SetActive(true);
            }

            if (battleHeroes.Count > 0)
            {
                for (int i = 0; i < battleHeroes.Count; i++)
                {
                    if(battleHeroes[i].GetCurrentHealth() <= 0.0f)
                    {
                        battleHeroes[i].SetDeathState();
                        battleHeroes.Remove(battleHeroes[i]);
                        return;
                    }
                }
            }

            if (battleHeroes.Count == 0)
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
                        battleHeroes.Add(newHero);
                    }
                });
            }
        }

        private void SetupEnemy()
        {
            int totalExperiencePoints = 0;

            for (int i = 0; i < battleHeroes.Count; i++)
            {
                totalExperiencePoints += battleHeroes[i].GetCurrentExperiencePoints();
            }

            totalExperiencePoints = totalExperiencePoints / battleHeroes.Count;

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
            battleHeroes[UnityEngine.Random.Range(0, battleHeroes.Count)].DecreaseHealth(battleEnemy.GetCurrentAttackPower());
            isEnemyTurn = false;
        }

        private void EndBattle()
        {
            // TODO : Add logic to go back to the main scene
        }
    }
}