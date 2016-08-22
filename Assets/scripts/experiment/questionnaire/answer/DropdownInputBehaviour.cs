using UnityEngine;
using UnityEngine.UI;
using questionnaire.data;
using System.Collections.Generic;

namespace questionnaire.answer
{
    public class DropdownInputBehaviour : AnswerSpaceBehaviour
    {
        public Text label;
        public Dropdown dropdown;

        public void Create(GenderInputData genderInput)
        {
            label.text = genderInput.label;
            dropdown.options = MakeOptions(genderInput.options);
            dropdown.value = genderInput.defaultOption;
        }

        public override string Answer()
        {
            return dropdown.options[dropdown.value].text;
        }

        private List<Dropdown.OptionData> MakeOptions(string[] options)
        {
            List<Dropdown.OptionData> optionDataList = new List<Dropdown.OptionData>();
            foreach (string option in options)
            {
                optionDataList.Add(new Dropdown.OptionData(option));
            }
            return optionDataList;
        }
    }
}