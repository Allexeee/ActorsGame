using UnityEngine;
using Homebrew;
using System.Collections.Generic;

namespace MyProject.Map
{
    // Фабрика для создания игрового поля
    [CreateAssetMenu(fileName = "FactoryMap", menuName = "Factory/FactoryMap")]
    public class FactoryMap : Factory
    {
        [SerializeField]
        private GameObject prefabCell;  // Префаб клетки

        public void Spawn(int width, int height, int countHouse = 2)
        {
            float offsetX = width / 2;
            float offsetZ = height / 2;

            // List<Point> indexHouses = RandomPoint(height, width, countHouse);
            List<Position> indexHouses = new List<Position>(){
                new Position(0,0),
                new Position(3,3)
            };

            for (int z = 0; z < height; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    // Создание игровой клетки
                    var actor = this.Populate<Actor>(Pool.Entities, prefabCell, new Vector3(x - offsetX, 0, z - offsetZ));
                    actor.selfTransform.name = "Cell z:" + z + " x:" + x;

                    if (indexHouses.Count <= 0)
                        continue;

                    Position point = new Position(z, x);
                    if (indexHouses.Contains(point))
                    {
                        // Добавление компонента к сущности
                        var composer = new EntityComposer(actor.entity, 1);
                        composer.Add<ComponentHouse>();

                        composer.Deploy();
                    }
                }
            }
        }

        // Создание списка случайных клеток
        private List<Position> RandomPoint(int height, int width, int count)
        {
            List<Position> points = new List<Position>(count);

            for (int i = 0; i < count; i++)
            {
                int rX = Random.Range(0, width);
                int rY = Random.Range(0, height);

                points.Add(new Position(rX, rY));
            }
            return points;
        }
    }

// Структура позиции клетки в XZ плоскости
    public struct Position
    {
        public readonly int X;
        public readonly int Z;
        public Position(int x, int z)
        {
            X = x;
            Z = z;
        }
    }
}