using System.Collections.Generic;
using System.Linq;
using BattleRpg.Hero;
using UnityEngine;
using UnityEngine.UI;

namespace BattleRpg.Menu
{
    public class MenuScene : MonoBehaviour
    {
        [SerializeField]
        private Camera mainCamera;

        [SerializeField]
        private List<Hero.Hero> heroList = new List<Hero.Hero>();

        [SerializeField]
        private Button startBattle = null;

        private List<HeroType> selectedHeroes = new List<HeroType>();

        private void Update()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                // Try a raycast from screenpoint to check whether we hit anything.
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                // If we did hit something, check which collider we hit.
                if (Physics.Raycast(ray, out hit))
                {
                    Hero.Hero hero = heroList.First(item => item.GetHeroName() == hit.collider.name);

                    if (hero != null && hero.IsUnlocked())
                    {
                        if (hero.Selected)
                        {
                            hero.UpdateSelectedState(false);
                            selectedHeroes.Remove(hero.GetHeroType());
                        }
                        else
                        {
                            if (selectedHeroes.Count < 3)
                            {
                                hero.UpdateSelectedState(true);
                                selectedHeroes.Add(hero.GetHeroType());
                            }
                        }
                    }
                }
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
    }
}