using player.data;
namespace menu.inlevel
{
    public class LevelFailedMenu : InGameMenu
    {
        public LevelFailedMenu(PlayerLevelData playerLevelData)
            : base("Level Failed", LevelResultToString(playerLevelData.LevelResult), true, new ButtonType[2] { ButtonType.Menu, ButtonType.Retry })
        {

        }


        private static string LevelResultToString(LevelResult result)
        {
            switch (result)
            {
                case LevelResult.Crash:
                    return "Crashed";
                case LevelResult.FellOffBottom:
                    return "Disappeared without trace";
                case LevelResult.Sunk:
                    return "Landed in the drink";
            }
            return "";
        }

    }
}