namespace Dtos
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Post { get; set; } = string.Empty;
        public string User { get; set; } = string.Empty;
    }
}