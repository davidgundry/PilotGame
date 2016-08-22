using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using menu;
using questionnaire.data;
using experiment;

namespace questionnaire
{
    public class QuestionnaireBehaviour : MonoBehaviour
    {
        public GameObject questionnairePanePrefab;
        public UIPane startingPane;
        public UIPane endingPane;
        private List<UIPane> paneList;

        private ExperimentController experimentController;

        void Start()
        {
            experimentController = GameObject.FindObjectOfType<ExperimentController>();
            Create();
        }

        void Create()
        {
            UIPanTransition panTransition = GetComponent<UIPanTransition>();
            paneList = new List<UIPane>();

            QuestionData[] questionData = new QuestionData[41]
            {
                new QuestionData("What is your gender?", new GenderInputData("Gender")),
                new QuestionData("What is your age?", new NumberInputData("Age","Age...")),
                new QuestionData("Were there other people nearby when you were playing?", new  YesNoData()),
                new QuestionData("Were you listening to music or other audio (besides in-game audio) while playing?", new  YesNoData()),
                new QuestionData("How noisy would you say that your environment was?", new LikertData(5,"No Noise","Lots of Noise")),

                new QuestionData("How familiar are you with similar games?", new LikertData(5,"Not at all","Very familiar")),
                
                new QuestionData("How worried were you about other people overhearing you when you were playing the game?", new LikertData(5,"Not at all","Very worried")),
                new QuestionData("How loud were you when playing the game?", new LikertData(5,"Very quiet","Very loud")),
                new QuestionData("To what extent were you worried about what you said being recorded?", new LikertData(5,"Not at all","Very worried")),
                new QuestionData("To what extent were you speaking naturally?", new LikertData(5,"Not at all","Very much so")),

                // IEQ
                new QuestionData("To what extent did the game hold your attention?", new LikertData(5,"Not at all","A lot")),
                new QuestionData("To what extent did you feel you were focused on the game?", new LikertData(5,"Not at all","A lot")),
                new QuestionData("How much effort did you put into playing the game?", new LikertData(5,"Not at all","A lot")),
                new QuestionData("Did you feel that you were trying you best?", new LikertData(5,"Not at all","Very much so")),
                new QuestionData("To what extent did you lose track of time?", new LikertData(5,"Not at all","A lot")),

                new QuestionData("To what extent did you feel consciously aware of being in the real world whilst playing?", new LikertData(5,"Not at all","Very much so")),
                new QuestionData("To what extent did you forget about your everyday concerns?", new LikertData(5,"Not at all","A lot")),
                new QuestionData("To what extent were you aware of yourself in your surroundings?", new LikertData(5,"Not at all","Very aware")),
                new QuestionData("To what extent did you notice events taking place around you?", new LikertData(5,"Not at all","A lot")),
                new QuestionData("Did you feel the urge at any point to stop playing and see what was happening around you?", new LikertData(5,"Not at all","Very much so")),

                new QuestionData("To what extent did you feel that you were interacting with the game environment?", new LikertData(5,"Not at all","Very much so")),
                new QuestionData("To what extent did you feel as though you were separated from your real-world environment?", new LikertData(5,"Not at all","Very much so")),
                new QuestionData("To what extent did you feel that the game was something you were experiencing, rather than something you were just doing? ", new LikertData(5,"Not at all","Very much so")),
                new QuestionData("To what extent was your sense of being in the game environment stronger than your sense of being in the real world?", new LikertData(5,"Not at all","Very much so")),
                new QuestionData("At any point did you find yourself become so involved that you were unaware you were even using controls?", new LikertData(5,"Not at all","Very much so")),

                new QuestionData("To what extent did you feel as though you were moving through the game according to you own will?", new LikertData(5,"Not at all","Very much so")),
                new QuestionData("To what extent did you find the game challenging?", new LikertData(5,"Not at all","Very difficult")),
                new QuestionData("Were there any times during the game in which you just wanted to give up?", new LikertData(5,"Not at all","A lot")),
                new QuestionData("To what extent did you feel motivated while playing?", new LikertData(5,"Not at all","A lot")),
                new QuestionData("To what extent did you find the game easy?", new LikertData(5,"Not at all","Very much so")),

                new QuestionData("To what extent did you feel like you were making progress towards the end of the game?", new LikertData(5,"Not at all","A lot")),
                new QuestionData("How well do you think you performed in the game?", new LikertData(5,"Very poor","Very well")),
                new QuestionData("To what extent did you feel emotionally attached to the game", new LikertData(5,"Not at all","Very much so")),
                new QuestionData("To what extent were you interested in seeing how the game’s events would progress?", new LikertData(5,"Not at all","A lot")),
                new QuestionData("How much did you want to “win” the game?", new LikertData(5,"Not at all","Very much so")),

                new QuestionData("Were you in suspense about whether or not you would win or lose the game?", new LikertData(5,"Not at all","Very much so")),
                new QuestionData("At any point did you find yourself become so involved that you wanted to speak to the game directly?", new LikertData(5,"Not at all","A lot")),
                new QuestionData("To what extent did you enjoy the graphics and the imagery", new LikertData(5,"Not at all","A lot")),
                new QuestionData("How much would you say you enjoyed playing the game?", new LikertData(5,"Not at all","A lot")),
                new QuestionData("When interrupted, were you disappointed that the game was over?", new LikertData(5,"Not at all","Very much so")),

                new QuestionData("Would you like to play the game again", new LikertData(5,"Definitely not","Definitely yes")),
            };


            int questionsPerPage = 3;
            paneList.Add(startingPane);
            startingPane.PaneNumber = 0;
            int paneIndex = 1;
            for (int i = 0; i < questionData.Length; i += questionsPerPage)
            {
                int questionThisPage = Mathf.Min(questionsPerPage, questionData.Length - i);
                QuestionData[] questionSubset = new QuestionData[questionThisPage];
                System.Array.Copy(questionData, i, questionSubset, 0, questionThisPage);
                QuestionnairePane newPane = CreatePane(paneIndex, questionSubset);
                paneList.Add(newPane);
                paneIndex++;
            }
            paneList.Add(endingPane);
            endingPane.PaneNumber = paneIndex;
            panTransition.Create(paneList.ToArray());
        }

        private QuestionnairePane CreatePane(int paneIndex, QuestionData[] questionData)
        {
            GameObject pane = GameObject.Instantiate(questionnairePanePrefab);
            pane.transform.SetParent(this.transform,false);
            pane.transform.position = Vector3.zero;
            QuestionnairePane questionnairePane = pane.GetComponent<QuestionnairePane>();
            questionnairePane.Create(questionData);

            questionnairePane.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, UIPanTransition.paneWidth * paneIndex, UIPanTransition.paneWidth);
            questionnairePane.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 1226);
            questionnairePane.transform.localScale = Vector3.one;
            questionnairePane.PaneNumber = paneIndex;

            return questionnairePane;
        }

        public void SubmitButton()
        {
            SaveQuestionnaireData();
            Application.LoadLevel("experiment-end");
        }

        private void SaveQuestionnaireData()
        {
            if (experimentController != null)
            {
                foreach (UIPane pane in paneList)
                {
                    QuestionnairePane qPane = pane as QuestionnairePane;
                    if (qPane != null)
                    {
                        foreach (QuestionBoxBehaviour box in qPane.QuestionBoxes)
                        {
                            experimentController.Telemetry.AddOrUpdateUserDataKeyValue(box.QuestionLabel, box.AnswerSpace.Answer());
                        }
                    }
                }
                experimentController.Telemetry.UploadOrSaveCurrentUserData();
            }
        }
    }
}