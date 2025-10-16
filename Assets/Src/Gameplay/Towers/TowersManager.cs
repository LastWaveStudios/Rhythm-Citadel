using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Gameplay.Towers
{
    public class TowersManager : Utilities.Singleton<TowersManager>
    {
        // used like a dictionary by keys 0-5 id of input group
        private List<TowersGroup> towersGroups;

        // Maybe use the keyCode for know the group in this same, logic or if not the keyCode,
        // one Id for the tower types or for the patterns that are in use
        public void AddTower(ATower tower)
        {

        }

        public void RemoveTower(ATower tower)
        {

        }

        // Subscribe to the inputs with the inputSystem and call, the correspondingGroup base on that key,
        // maybe add the keyCode or action or something to the group, but probably is better use the dictionary on the TowersGroups
    }
}