using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using questionnaire.data;
using questionnaire.answer;

namespace questionnaire
{
    public class QuestionBoxBehaviour : MonoBehaviour
    {
        public Text questionText;
        public GameObject likertPrefab;
        public GameObject yesNoPrefab;

        public string Question { get; private set; }
        public AnswerSpaceBehaviour AnswerSpace { get; private set; }

        public void Create(QuestionData questionData)
        {
            questionText.text = questionData.question;
            Question = questionData.question;

            if (questionData.answer.GetType() == typeof(Likert))
            {
                GameObject answer = Instantiate(likertPrefab);
                answer.transform.SetParent(this.transform,false);
                answer.GetComponent<LikertScaleBehaviour>().Create((Likert) questionData.answer);
                answer.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, 100);
                AnswerSpace = answer.GetComponent<AnswerSpaceBehaviour>();
            }
            else if (questionData.answer.GetType() == typeof(YesNo))
            {
                GameObject answer = Instantiate(yesNoPrefab);
                answer.transform.SetParent(this.transform, false);
                answer.GetComponent<YesNoBehaviour>().Create((YesNo) questionData.answer);
                answer.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, 100);
                AnswerSpace = answer.GetComponent<AnswerSpaceBehaviour>();
            }

        }



    }
}