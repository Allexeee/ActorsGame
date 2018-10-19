using UnityEngine;
using Homebrew;

namespace MyProject.Map
{
    [System.Serializable]
    public class ComponentHouse : IComponent
    {
        public Color ColorHouse = Color.blue;   // Цвет здания
    }
}