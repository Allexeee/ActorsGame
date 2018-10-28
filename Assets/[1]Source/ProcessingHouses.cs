using UnityEngine;
using Homebrew;

namespace MyProject.Map
{
    // Обработчик клеток-зданий
    public class ProcessingHouses : ProcessingBase
    {
        private Group<ComponentCell, ComponentHouse> groupHouses;

        public ProcessingHouses()
        {
            groupHouses.Added += entity =>
            {
                entity.ComponentCell().MeshRenderer.material.color = entity.ComponentHouse().ColorHouse;
            };
        }
    }
}