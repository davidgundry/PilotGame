using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using questionnaire.data;

namespace questionnaire.answer
{
    public class LikertScaleBehaviour : AnswerSpaceBehaviour
    {
        public Text leftLabelText;
        public Text rightLabelText;
        public ButtonSpaceBehaviour buttonSpace;

        public void Create(LikertData liketScale)
        {
            buttonSpace.Create(liketScale.scaleLength);
            leftLabelText.text = liketScale.labelLeft;
            rightLabelText.text = liketScale.labelRight;
        }

        public override string Answer()
        {
            return buttonSpace.SelectedLabel;
        }
    }
}