using UnityEngine;
using Homebrew;

namespace MyProject.Map
{
    [System.Serializable]
    public class ComponentCell : IComponent, ISetup, System.IDisposable
    {
        public Color ColorDefault = Color.gray;     // Цвет клетки по дефолту

        public MeshRenderer MeshRenderer { get; private set; }  // Кеш компонента

        // Инициализация переменных
        public void Setup(Actor actor)
        {
            MeshRenderer = actor.Get<MeshRenderer>("view");
        }

        // Очистка переменных ссылочного типа
        public void Dispose()
        {
            MeshRenderer = null;
        }
    }
}