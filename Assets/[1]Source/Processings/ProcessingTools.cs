using UnityEngine;
using Homebrew;

namespace MyProject.UI
{
    // Обработчик инструментов
    public class ProcessingTools : ProcessingBase
    {
        [GroupBy(Tag.ButtonLeftClick, Tag.Tool)]
        private Group<ComponentButton, ComponentToggle> groupButtonsToggle; // Инструменты с кнопкой-переключателем

        public ProcessingTools()
        {
            groupButtonsToggle.Added += entity =>
            {
                var cButton = entity.ComponentButton();
                var cToggle = entity.ComponentToggle();

                entity.Remove(Tag.ButtonLeftClick);

                if (cToggle.ToggleStateTo() == 1)
                    entity.Add(Tag.ToolActive);
                else
                    entity.Remove(Tag.ToolActive);

                cButton.TextButton.text = cToggle.TextState[cToggle.CurrentState];
            };
        }
    }
}