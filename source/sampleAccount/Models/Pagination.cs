namespace sampleAccount.Models
{
    public class Pagination
    {
        public Pagination(int pageIndex, int pageSize)
        {
            this.pageIndex = pageIndex;
            this.pageSize = pageSize;
        }

        public int pageIndex { get; set; }
        public int pageSize { get; set; }
    }
}