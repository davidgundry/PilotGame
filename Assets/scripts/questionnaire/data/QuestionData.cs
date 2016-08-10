namespace questionnaire.data
{
    public class QuestionData
    {
        public readonly string question;
        public readonly AnswerData answer;

        public QuestionData(string question, AnswerData answer)
        {
            this.question = question;
            this.answer = answer;
        }
    }
}