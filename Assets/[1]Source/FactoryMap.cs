using UnityEngine;
using Homebrew;

namespace MyProject.Map
{
    // Фабрика для создания игрового поля
    [CreateAssetMenu(fileName = "FactoryMap", menuName = "Factory/FactoryMap")]
    public class FactoryMap : Factory
    {
        [SerializeField]
        private GameObject prefabCell;  // Префаб клетки

        public void Spawn(int width, int height)
        {
            float offsetX = width / 2;
            float offsetZ = height / 2;

            for (int z = 0; z < height; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    // Создание игровой клетки
                    var actor = this.Populate<Actor>(Pool.Entities, prefabCell, new Vector3(x - offsetX, 0, z - offsetZ));
                    actor.selfTransform.name = "Cell z:" + z + " x:" + x;
                }
            }
        }
    }
}