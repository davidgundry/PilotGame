
namespace menu.inlevel
{
    public class PauseMenu : InGameMenu
    {
        public PauseMenu()
            : base("Paused", "press 'p' to resume", true, new ButtonType[3] { ButtonType.Menu, ButtonType.Resume, ButtonType.Retry })
        {
            
        }

    }
}