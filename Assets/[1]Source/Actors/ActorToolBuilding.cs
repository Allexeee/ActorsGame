using UnityEngine;
using Homebrew;

namespace MyProject.UI
{
    // Актор инструмента "Строительство дороги"
    public class ActorToolBuilding : Actor
    {
        [FoldoutGroup("Setup")] public ComponentButton componentButton;
        [FoldoutGroup("Setup")] public ComponentToggle componentToggle;
        [FoldoutGroup("Setup")] public ComponentModeBuildRoad componentBuildRoad;

        protected override void Setup()
        {
            Add(componentButton);
            Add(componentToggle);
            Add(componentBuildRoad);

            // Добавляем тег
            Add(Tag.Tool);
        }
    }
}