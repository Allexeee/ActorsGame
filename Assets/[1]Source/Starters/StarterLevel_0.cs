using UnityEngine;
using Homebrew;
using MyProject.Map;
using MyProject.UI;

namespace MyProject
{
    public class StarterLevel_0 : Starter
    {
        protected override void Setup()
        {
            // Добавляем нужные процессинги
            Toolbox.Add<ProcessingCells>();
            Toolbox.Add<ProcessingHouses>();
            Toolbox.Add<ProcessingTools>();
            Toolbox.Add<ProcessingRoads>();
        }
    }
}