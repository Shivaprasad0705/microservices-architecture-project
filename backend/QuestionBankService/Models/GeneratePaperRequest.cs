namespace QuestionBankService.Models
{
    public class GeneratePaperRequest
    {
        public string Subject { get; set; }=string.Empty;
        public int EasyCount { get; set; }
        public int MediumCount { get; set; }
        public int HardCount { get; set; }
    }
}
