using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleRpg.Menu
{
    public class MenuScene : MonoBehaviour
    {
        [SerializeField]
        private Camera mainCamera;

        [SerializeField]
        private List<Hero.Hero> heroList = new List<Hero.Hero>();

        // Update is called once per frame
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

                    if (hero != null)
                    {
                        List<Hero.Hero> list = heroList.Where(selectedHero => selectedHero.Selected).ToList();

                        if (hero.Selected)
                        {
                            hero.UpdateSelectedState(false);
                        }
                        else
                        {
                            if (list.Count < 3)
                            {
                                hero.UpdateSelectedState(true);
                            }
                        }
                    }
                }
            }
        }
    }
}