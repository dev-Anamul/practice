namespace Common
{
    public class Pagination
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string Search { get; set; } = String.Empty;

        public string Sort { get; set; } = "Id";

        public string Order { get; set; } = "asc";

        public string Filter { get; set; } = String.Empty;

        public string Fields { get; set; } = String.Empty;


    }
}