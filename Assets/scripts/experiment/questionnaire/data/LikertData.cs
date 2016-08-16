namespace questionnaire.data
{
    public class LikertData : AnswerData
    {
        public readonly int scaleLength;
        public readonly string labelLeft;
        public readonly string labelRight;

        public LikertData(int scaleLength, string labelLeft, string labelRight)
        {
            this.scaleLength = scaleLength;
            this.labelLeft = labelLeft;
            this.labelRight = labelRight;
        }
    }
}