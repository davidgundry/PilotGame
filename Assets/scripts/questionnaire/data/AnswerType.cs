namespace questionnaire.data
{
    public class AnswerData
    {

        protected AnswerData()
        {

        }

    }

    public class YesNo : AnswerData
    {

        public YesNo()
            : base()
        {

        }

    }

    public class Likert : AnswerData
    {
        public readonly int scaleLength;
        public readonly string labelLeft;
        public readonly string labelRight;

        public Likert(int scaleLength, string labelLeft, string labelRight)
            : base()
        {
            this.scaleLength = scaleLength;
            this.labelLeft = labelLeft;
            this.labelRight = labelRight;
        }
    }
}