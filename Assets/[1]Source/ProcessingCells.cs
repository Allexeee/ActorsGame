using UnityEngine;
using Homebrew;
using System.Collections.Generic;

namespace MyProject.Map
{
    // Обработчик клеток
    public class ProcessingCells : ProcessingBase
    {
        private Group<ComponentCell> groupCells; // Группа клеток

        private int height = 5;         // Высота игрового поля
        private int width = 5;          // Ширина игрового поля

        public ProcessingCells()
        {
            // Подписываемся на событие по добавлению новых участников группы
            groupCells.Added += GroupCellsOnAdd;

            // Генерация карты
            Toolbox.Get<FactoryMap>().Spawn(width, height);
        }

        private void GroupCellsOnAdd(int entity)
        {
            // Получаем нужный компонент
            var cCells = entity.ComponentCell();

            // Устанавливаем цвет по дефолту
            cCells.MeshRenderer.material.color = cCells.ColorDefault;
        }
    }
}