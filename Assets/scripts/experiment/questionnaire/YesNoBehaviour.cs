using UnityEngine;
using System.Collections;
using questionnaire.data;

namespace questionnaire.answer
{
    public class YesNoBehaviour : AnswerSpaceBehaviour
    {
        public ButtonSpaceBehaviour buttonSpace;

        public void Create(YesNo yesNo)
        {
            buttonSpace.Create(new string[2] { "Yes", "No" });
        }

        public override string Answer()
        {
            return buttonSpace.SelectedLabel;
        }
    }
}