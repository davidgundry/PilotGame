namespace questionnaire.data
{
    public class LikertData : ButtonData
    {
        public LikertData(int scaleLength, string labelLeft, string labelRight)
            : base(ScaleOptions(scaleLength), labelLeft, labelRight)
        {

        }

        private static string[] ScaleOptions(int scaleLength)
        {
            string[] options = new string[scaleLength];
            for (int i=1;i<=scaleLength;i++)
                options[i-1] = i.ToString();
            return options;
        }
    }
}