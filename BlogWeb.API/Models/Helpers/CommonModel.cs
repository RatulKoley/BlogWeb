namespace BlogWeb.API.Models.Helpers
{
    public class CommonModel
    {
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
        public string Term { get; set; }
        public string NameSortOrder { get; set; }
        public string OrderBy { get; set; }
        public bool? FailureStatus { get; set; }
        public string? Massage { get; set; }
        public int? StatusCode { get; set; }
        public string? Token { get; set; }
        public DateTime? ActionTime { get; set; }
    }
}
