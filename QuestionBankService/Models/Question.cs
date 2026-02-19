public class Question
{
    public int Id { get; set; }

    public required string QuestionText { get; set; }
    public required string OptionA { get; set; }
    public required string OptionB { get; set; }
    public required string OptionC { get; set; }
    public required string OptionD { get; set; }
    public required string CorrectOption { get; set; }
    public required string Subject { get; set; }
    public required string Difficulty { get; set; }

    public bool IsActive { get; set; }
}
