using UnityEngine;
using Homebrew;

namespace MyProject.Map
{
    // Компонент здания
    [System.Serializable]
    public class ComponentHouse : IComponent
    {
        public Color ColorHouse = Color.blue;   // Цвет здания
    }

    // Методы, необходимые для получения компонента
    public static partial class Game
    {
        public static ComponentHouse ComponentHouse(this int entity)
        {
            return Storage<ComponentHouse>.Instance.components[entity];
        }

        public static bool TryGetComponentHouse(this int entity, out ComponentHouse component)
        {
            component = Storage<ComponentHouse>.Instance.TryGet(entity);
            return component != null;
        }
    }
}