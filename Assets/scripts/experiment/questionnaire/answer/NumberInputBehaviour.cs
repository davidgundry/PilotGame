using UnityEngine;
using UnityEngine.UI;
using questionnaire.data;

namespace questionnaire.answer
{
    public class NumberInputBehaviour : AnswerSpaceBehaviour
    {
        public Text label;
        public InputField inputField;

        public void Create(NumberInputData numberInput)
        {
            label.text = numberInput.label;
            inputField.placeholder.GetComponent<Text>().text = numberInput.placeholder;
        }

        public override string Answer()
        {
            return inputField.text;
        }
    }
}