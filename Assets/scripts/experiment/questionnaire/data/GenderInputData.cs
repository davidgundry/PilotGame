namespace questionnaire.data
{
    public class GenderInputData : DropdownAnswerData
    {
        private const string defaultLabel = "Gender";
        private static readonly string[] genderOptions = new string[4] { "Male", "Female", "Other", "Prefer not to say" };
        private const int defaultOption = 3;

        public GenderInputData(string label)
            : base(defaultLabel, genderOptions, defaultOption)
        {

        }

    }
}