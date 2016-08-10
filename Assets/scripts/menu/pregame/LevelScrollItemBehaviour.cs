using UnityEngine;
using UnityEngine.UI;
using menu;
using player.data;

namespace menu.pregame
{
    public class LevelScrollItemBehaviour : MonoBehaviour {

        public Text name;
        public StarBoxBehaviour starBox;
        public GameObject padlock;
        private int levelID;

        private string Name
        {
            get
            { 
                if (name != null) 
                    return name.text;
                else
                    throw new System.MemberAccessException("Text name not assigned in inspector");
            } 
            set
            {
                if (name != null) 
                    name.text = value;
                else
                    throw new System.MemberAccessException("Text name not assigned in inspector");
            }
        }

        public void Create(int levelID, LevelListData levelListData)
        {
            this.levelID = levelID;
            if (levelListData.Locked)
            {
                Name = "";
                CreatePadlock();
                GetComponent<Button>().interactable = false;
            }
            else
            {
                Name = levelListData.name;
                CreateStarBox(levelListData);
            }
        }

        private void CreatePadlock()
        {
            starBox.gameObject.SetActive(false);
            padlock.SetActive(true);
        }

        private void CreateStarBox(LevelListData levelListData)
        {
            padlock.SetActive(false);
            if (starBox != null)
            {
                starBox.gameObject.SetActive(true);
                if (levelListData.PlayerLevelRecord != null)
                    starBox.Refresh(levelListData.PlayerLevelRecord.starScore);
                else
                    starBox.Refresh(StarScore.scores[0]);
                starBox.transform.localScale = new Vector2(0.75f, 0.75f);
            }
            else
                throw new System.MemberAccessException("StarBoxBehaviour starBox not assigned in inspector");
        }

        public void OnClick()
        {
            GameController gameController = GameObject.FindObjectOfType<GameController>();
            UIPanTransition uiPanTransition = GameObject.FindObjectOfType<UIPanTransition>();
            gameController.SelectedLevelID = levelID;
            uiPanTransition.TransitionTo(2);
            //Application.LoadLevel("details-menu");
        }

    }
}