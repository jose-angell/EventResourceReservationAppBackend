namespace EventResourceReservationApp.Application.DTOs.Categories
{
    public class ReadAllCategoryRequest
    {
        public string? NameFilter { get; set; }
        public Guid? CreatedByUserIdFilter { get; set; }
        public string? OrderBy { get; set; }
    }
}
