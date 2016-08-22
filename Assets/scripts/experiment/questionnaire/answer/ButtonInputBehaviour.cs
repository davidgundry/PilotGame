using UnityEngine;
using UnityEngine.UI;
using questionnaire.data;

namespace questionnaire.answer
{
    public class ButtonInputBehaviour : AnswerSpaceBehaviour
    {
        public Text leftLabelText;
        public Text rightLabelText;
        public ButtonSpaceBehaviour buttonSpace;

        public void Create(ButtonData data)
        {
            buttonSpace.Create(data.options);
            if (leftLabelText != null)
                leftLabelText.text = data.labelLeft;
            if (rightLabelText != null)
                rightLabelText.text = data.labelRight;
        }

        public override string Answer()
        {
            return buttonSpace.SelectedLabel;
        }
    }
}