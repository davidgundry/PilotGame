namespace menu.inlevel
{
    /// <summary>
    /// Contains all of the information for the InGameMenuBehaviour to create a particular type of in-game menu.
    /// </summary>
    public class InGameMenu
    {
        public readonly string title;
        public readonly string subtitle;
        public readonly bool headerVisible;
        public readonly ButtonType[] availableButtons;

        public InGameMenu(string title, string subtitle, bool headerVisible, ButtonType[] availableButtons)
        {
            this.title = title;
            if (subtitle != null)
                this.subtitle = subtitle.ToLower();
            this.headerVisible = headerVisible;
            this.availableButtons = availableButtons;
        }

    }
}
