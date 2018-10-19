using UnityEngine;
using Homebrew;

namespace MyProject.Map
{
    public class ProcessingHouses : ProcessingBase
    {
        private Group<ComponentCell, ComponentHouse> groupHouses;

        public ProcessingHouses()
        {
            groupHouses.OnAdded += i => { groupHouses.component[i].MeshRenderer.material.color = groupHouses.component2[i].ColorHouse; };
        }
    }
}