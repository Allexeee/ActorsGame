//   Project : Battlecruiser3.0
//  Contacts : Pixeye - info@pixeye.games 
//      Date : 8/16/2018


namespace Homebrew
{
    public static class Tag
    {
        [TagField(categoryName = "ButtonEvent")] public const int ButtonLeftClick = 1;
        [TagField(categoryName = "ButtonEvent")] public const int ButtonRightClick = 2;

        public const int Tool = 10;
        public const int ToolActive = 11;

        public const int ComponentCell = 20;
        public const int ComponentHouse = 21;
        public const int ComponentRoad = 22;
    }
}