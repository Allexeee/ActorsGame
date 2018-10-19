using UnityEngine;
using Homebrew;
using MyProject.Map;

namespace MyProject
{
    public class StarterLevel_0 : Starter
    {
        protected override void Setup()
        {
            // Добавляем нужные процессинги
            Toolbox.Add<ProcessingCells>();
        }
    }
}