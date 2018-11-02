using UnityEngine;
using UnityEngine.UI;
using Homebrew;

namespace MyProject.UI
{
    // Компонент обычной кнопки
    [System.Serializable]
    public class ComponentButton : IComponent, ISetup, System.IDisposable
    {
        [HideInInspector]
        public Button Button;
        [HideInInspector]
        public Text TextButton;

        public void Setup(Actor actor)
        {
            // Ссылка на кнопку из дочернего объекта Button
            Button = actor.entity.Get<Button>("Button");
            // Ссылка на текст кнопки из дочернего объекта Button/Text
            TextButton = actor.entity.Get<Text>("Button/Text");
        }

        // Очищение памяти
        public void Dispose()
        {
            Button = null;
            TextButton = null;
        }
    }

    // Методы, необходимые для получения компонента
    public static partial class Game
    {
        public static ComponentButton ComponentButton(this int entity)
        {
            return Storage<ComponentButton>.Instance.components[entity];
        }

        public static bool TryGetComponentButton(this int entity, out ComponentButton component)
        {
            component = Storage<ComponentButton>.Instance.TryGet(entity);
            return component != null;
        }
    }
}