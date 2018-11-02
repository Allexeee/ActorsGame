using UnityEngine;
using Homebrew;

namespace MyProject.Map
{
    public class ActorCell : Actor
    {
        [FoldoutGroup("Setup")] public ComponentCell componentCell;

        protected override void Setup()
        {
            Add(componentCell);
        }
    }
}