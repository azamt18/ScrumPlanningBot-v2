namespace ScrumPlanningBot.Application
{
    public class ChatMessageEventArgs
    {
        public string? Command { get; set; }
        public string? Text { get; set; }
        public long UserId { get; set; }
        public long ChatId { get; set; }
        public int MessageId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
    }
}