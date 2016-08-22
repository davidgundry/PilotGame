using UnityEngine;
using UnityEngine.UI;
using questionnaire.data;
using questionnaire.answer;

namespace questionnaire
{
    public class QuestionBoxBehaviour : MonoBehaviour
    {
        public Text questionText;
        public GameObject likertPrefab;
        public GameObject yesNoPrefab;
        public GameObject numberInputPrefab;
        public GameObject genderInputPrefab;

        public string QuestionLabel { get; private set; }
        public AnswerSpaceBehaviour AnswerSpace { get; private set; }

        public void Create(QuestionData questionData)
        {
            questionText.text = questionData.question;
            QuestionLabel = questionData.question;
            AnswerSpace = CreateAnswerSpaceForQuestion(questionData);
        }

        private AnswerSpaceBehaviour CreateAnswerSpaceForQuestion(QuestionData questionData)
        {
            GameObject answer = null;

            if (questionData.answer.GetType() == typeof(LikertData))
            {
                answer = Instantiate(likertPrefab);
                answer.GetComponent<ButtonInputBehaviour>().Create((LikertData)questionData.answer);
            }
            else if (questionData.answer.GetType() == typeof(YesNoData))
            {
                answer = Instantiate(yesNoPrefab);
                answer.GetComponent<ButtonInputBehaviour>().Create((YesNoData)questionData.answer);
            }
            else if (questionData.answer.GetType() == typeof(GenderInputData))
            {
                answer = Instantiate(genderInputPrefab);
                answer.GetComponent<DropdownInputBehaviour>().Create((GenderInputData)questionData.answer);
            }
            else if (questionData.answer.GetType() == typeof(NumberInputData))
            {
                answer = Instantiate(numberInputPrefab);
                answer.GetComponent<NumberInputBehaviour>().Create((NumberInputData)questionData.answer);
            }

            if (answer != null)
            {
                answer.transform.SetParent(this.transform, false);
                answer.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, 100);
                return answer.GetComponent<AnswerSpaceBehaviour>();
            }
            throw new System.ArgumentException("QuestionData's AnswerData is not of a recognised type.");
        }
    }
}