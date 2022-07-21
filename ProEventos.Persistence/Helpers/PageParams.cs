
namespace ProEventos.Persistence.Helpers
{
    public class PageParams
    {
        public const int  MaxPageSize = 50;

        public int PageNumber { get; set; } = 1;

        public string Term { get; set; } = string.Empty;

        private int pageSize;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize =  (value > MaxPageSize)? MaxPageSize: value; }
        }
    }
}