namespace questionnaire.data
{
    public class YesNoData : ButtonData
    {
        private static readonly string[] options = new string[2] { "Yes", "No" };
        public YesNoData() : base (options,"","")
        {

        }
    }
}