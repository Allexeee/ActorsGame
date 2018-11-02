using UnityEngine;
using Homebrew;
using System;

namespace MyProject.UI
{
    // Компонент, который добавляется кнопке
    public class MonoComponentButton : MonoCached
    {
        [FoldoutGroup("Setup")]
        [TagFilter(typeof(Tag))] public int tag;
        protected override void Setup()
        {
            ComponentButton cButton;
            if (entityParent.TryGetComponentButton(out cButton))
                cButton.Button.onClick.AddListener(OnClick);
        }

        public void OnClick()
        {
            entityParent.Add(tag);
        }
    }
}