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
                var cCell = entity.ComponentCell();
                var cHouse = entity.ComponentHouse();

                cCell.MeshRenderer.material.color = cHouse.ColorHouse;
            };
        }
    }
}