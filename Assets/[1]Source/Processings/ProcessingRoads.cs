using UnityEngine;
using Homebrew;
using MyProject.UI;
using MyProject;

namespace MyProject.Map
{
    // Обработчик дорог
    public class ProcessingRoads : ProcessingBase, ITick
    {
        [GroupBy(Tag.Tool, Tag.ToolActive)]
        private Group<ComponentModeBuildRoad> groupToolBuildRoad; // Группа инструментов строительства дорог

        private Group<ComponentCell, ComponentRoad> groupRoad; // Группа дорог

        public ProcessingRoads()
        {
            // При добавлении меняем цвет
            groupRoad.Added += entity =>
            {
                var cCell = entity.ComponentCell();
                var cHouse = entity.ComponentRoad();

                cCell.MeshRenderer.material.color = cHouse.ColorRoad;
            };
        }

        public void Tick()
        {
            // Если кнопка мыши не нажата и нет активного инструмента
            if (!(Input.GetMouseButton(0) & groupToolBuildRoad.length > 0))
                return;

            // Луч из указателя мыши в сцену игры
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // Количество объектов на пути луча
            var amount = Phys.Raycast(ray);
            for (int i = 0; i < amount; i++)
            {
                // Перебираем все RaycastHit попавшие в область рейкаста.
                var entity = Phys.hits[i].GetEntity();

                // Если сущность содержит хоть один из тегов, то игнорируем
                if(entity.HasAny(Tag.ComponentHouse, Tag.ComponentRoad))
                    continue;

                // Добавление компонента к сущности
                var composer = new EntityComposer(entity, 1);
                composer.Add<ComponentRoad>();

                composer.Deploy();
            }
        }
    }
}