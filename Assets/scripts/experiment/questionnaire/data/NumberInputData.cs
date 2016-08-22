using UnityEngine;
using System.Collections;

namespace questionnaire.data
{
    public class NumberInputData : AnswerData
    {
        public readonly string label;
        public readonly string placeholder;

        public NumberInputData(string label, string placeholder)
        {
            this.label = label;
            this.placeholder = placeholder;
        }
    }
}
