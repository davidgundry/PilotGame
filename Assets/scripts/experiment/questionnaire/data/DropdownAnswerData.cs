using UnityEngine;
using System.Collections;

namespace questionnaire.data
{
    public class DropdownAnswerData : AnswerData
    {
        public readonly string label;
        public readonly int defaultOption;
        public readonly string[] options;

        public DropdownAnswerData(string label, string[] options, int defaultOption)
        {
            this.label = label;
            this.options = options;
            this.defaultOption = defaultOption;
        }
    }
}