using UnityEngine;
using UnityEngine.UI;
using questionnaire.data;
using menu;

namespace questionnaire
{
    public class QuestionnairePane : UIPane
    {
        public GameObject questionSpace;
        public GameObject QuestionBoxPrefab;

        public void Create(QuestionData[] questions)
        {
            float inset = 0;
            float size = 250;
            float padding = 50;
            foreach (QuestionData question in questions)
            {
                GameObject box = Instantiate(QuestionBoxPrefab);
                QuestionBoxBehaviour questionBox = box.GetComponent<QuestionBoxBehaviour>();
                questionBox.Create(question);
                questionBox.transform.SetParent(questionSpace.transform, false);
                questionBox.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, inset, size);
                inset += size + padding;
            }
        }


    }
}