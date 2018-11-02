using UnityEngine;
using Homebrew;

namespace MyProject.Map
{
    // Компонент дороги
    [System.Serializable]
    [RequireTags(Tag.ComponentRoad)]
    public class ComponentRoad : IComponent
    {
        public Color ColorRoad = Color.green;
    }

    // Методы, необходимые для получения компонента
    public static partial class Game
    {
        public static ComponentRoad ComponentRoad(this int entity)
        {
            return Storage<ComponentRoad>.Instance.components[entity];
        }

        public static bool TryGetComponentRoad(this int entity, out ComponentRoad component)
        {
            component = Storage<ComponentRoad>.Instance.TryGet(entity);
            return component != null;
        }
    }

}