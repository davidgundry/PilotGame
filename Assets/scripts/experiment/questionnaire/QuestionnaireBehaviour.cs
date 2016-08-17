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

            QuestionData[] questionData = new QuestionData[21]
            {
                new QuestionData("Have you played this game before?", new YesNoData()),
                new QuestionData("How familiar are you with similar games?", new LikertData(5,"Not at all","Very familiar")),
                new QuestionData("Were there other people nearby when you were playing?", new  YesNoData()),
                new QuestionData("Were you interrupted while playing?", new  YesNoData()),
                new QuestionData("Were you listening to music or other audio (besides in-game audio) while playing?", new  YesNoData()),
                new QuestionData("Do you have a microphone?", new  YesNoData()),

                new QuestionData("How noisy would you say that your environment was?", new LikertData(5,"No Noise","Lots of Noise")),
                new QuestionData("How worried were you about other people overhearing you when you were playing the game?", new LikertData(5,"Not at all","Very worried")),
                new QuestionData("How loud were you when playing the game?", new LikertData(5,"Very quiet","Very loud")),
                new QuestionData("To what extent were you worried about what you said being recorded?", new LikertData(5,"Not at all","Very worried")),
                new QuestionData("To what extent were you speaking naturally?", new LikertData(5,"Not at all","Very much so")),

                new QuestionData("How noisy would you say that your environment was?", new LikertData(5,"No Noise","Lots of Noise")),
                new QuestionData("How worried were you about other people overhearing you when you were playing the game?", new LikertData(5,"Not at all","Very worried")),
                new QuestionData("How loud were you when playing the game?", new LikertData(5,"Very quiet","Very loud")),
                new QuestionData("To what extent were you worried about what you said being recorded?", new LikertData(5,"Not at all","Very worried")),
                new QuestionData("To what extent were you speaking naturally?", new LikertData(5,"Not at all","Very much so")),

                new QuestionData("How noisy would you say that your environment was?", new LikertData(5,"No Noise","Lots of Noise")),
                new QuestionData("How worried were you about other people overhearing you when you were playing the game?", new LikertData(5,"Not at all","Very worried")),
                new QuestionData("How loud were you when playing the game?", new LikertData(5,"Very quiet","Very loud")),
                new QuestionData("To what extent were you worried about what you said being recorded?", new LikertData(5,"Not at all","Very worried")),
                new QuestionData("To what extent were you speaking naturally?", new LikertData(5,"Not at all","Very much so"))
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
                            experimentController.Telemetry.AddOrUpdateUserDataKeyValue(box.Question, box.AnswerSpace.Answer());
                        }
                    }
                }
                experimentController.Telemetry.UploadOrSaveCurrentUserData();
            }
        }
    }
}