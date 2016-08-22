using UnityEngine;
using System.Collections;

namespace questionnaire.data
{
    public class ButtonData : AnswerData
    {
        public readonly string[] options;
        public readonly string labelLeft;
        public readonly string labelRight;

        public ButtonData(string[] options, string labelLeft, string labelRight)
        {
            this.options = options;
            this.labelLeft = labelLeft;
            this.labelRight = labelRight;
        }
    }
}