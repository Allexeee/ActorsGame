using UnityEngine;
using Homebrew;

namespace MyProject.Map
{
    [System.Serializable]
    [RequireTags(Tag.ComponentCell)]
    public class ComponentCell : IComponent, ISetup, System.IDisposable
    {
        public Color ColorDefault = Color.gray;     // Цвет клетки по дефолту

        public MeshRenderer MeshRenderer;  // Кеш компонента

        // Инициализация переменных
        public void Setup(Actor actor)
        {
            // Получает ссылку на нужный компонент из дочернего объекта view
            MeshRenderer = actor.entity.Get<MeshRenderer>("view");
        }

        // Очистка переменных ссылочного типа
        public void Dispose()
        {
            MeshRenderer = null;
        }
    }

    // Методы, необходимые для получения компонента
    public static partial class Game
    {
        public static ComponentCell ComponentCell(this int entity)
        {
            return Storage<ComponentCell>.Instance.components[entity];
        }

        public static bool TryGetComponentCell(this int entity, out ComponentCell component)
        {
            component = Storage<ComponentCell>.Instance.TryGet(entity);
            return component != null;
        }
    }
}