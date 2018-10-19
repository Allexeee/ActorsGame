using UnityEngine;
using Homebrew;
using System.Collections.Generic;

namespace MyProject.Map
{
    // Обработчик игровой карты
    public class ProcessingCells : ProcessingBase
    {
        private Group<ComponentCell> groupCells; // Группа клеток

        private int height = 5;         // Высота игрового поля
        private int width = 5;          // Ширина игрового поля

        public ProcessingCells()
        {
            // Подписываемся на событие по добавлению новых участников группы
            groupCells.OnAdded += OnAddCell;

            // Генерация карты
            Toolbox.Get<FactoryMap>().Spawn(width, height);
        }

        private void OnAddCell(int i)
        {
            var cCells = groupCells.component[i];

            // Устанавливаем цвет по дефолту
            cCells.MeshRenderer.material.color = cCells.ColorDefault;
        }
    }
}