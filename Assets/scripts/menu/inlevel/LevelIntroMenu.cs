using level.data;
namespace menu.inlevel
{
    public class LevelIntroMenu : InGameMenu
    {
        public LevelIntroMenu(LevelData levelData)
            : base(levelData.name, levelData.subtitle, false, null)
        {

        }
    }
}