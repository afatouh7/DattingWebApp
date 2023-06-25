namespace APi.Helpers
{
    public class PaginationHeader
    {
        public PaginationHeader(int currentPage, int itemsPage, int tOtalItems, int totalPages)
        {
            CurrentPage = currentPage;
            ItemsPage = itemsPage;
            TotalItems = tOtalItems;
            TotalPages = totalPages;
        }

        public int CurrentPage { get; set; }
        public int ItemsPage { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
    }
}
