using UnityEngine;
using UnityEngine.UI;
using Homebrew;
using System.Collections.Generic;

namespace MyProject.UI
{
    // Компонент переключатель состояний. 
    // Каждому состоянию соответвует текст из списка
    [System.Serializable]
    public class ComponentToggle : IComponent
    {
        public int CurrentState = 0;
        public List<string> TextState = new List<string>()
        {
            "Режим строительства: Выкл",
            "Режим строительства: Вкл"
        };
    }

    // Методы, необходимые для получения компонента
    public static partial class Game
    {
        public static ComponentToggle ComponentToggle(this int entity)
        {
            return Storage<ComponentToggle>.Instance.components[entity];
        }

        public static bool TryGetComponentToggle(this int entity, out ComponentToggle component)
        {
            component = Storage<ComponentToggle>.Instance.TryGet(entity);
            return component != null;
        }

        public static int ToggleStateTo(this ComponentToggle cToggle, int i = 1)
        {
            int targetState = cToggle.CurrentState + i;
            
            if (targetState >= cToggle.TextState.Count)
            {
                targetState = 0;
            }
            if (targetState < 0)
            {
                targetState = cToggle.TextState.Count;
            }
            
            return cToggle.CurrentState = targetState;
        }
    }
}