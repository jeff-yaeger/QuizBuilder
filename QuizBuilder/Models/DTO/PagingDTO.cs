namespace QuizBuilder.Models.DTO;

public class PagingDTO<T>
{
    public T Data { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalRecords { get; set; }
    public int TotalPages { get; set; }

    public void SetLimits()
    {
        if (PageSize > 50)
        {
            PageSize = 50;
        }

        if (PageSize < 10)
        {
            PageSize = 10;
        }

        if (PageNumber < 1)
        {
            PageNumber = 1;
        }

        if (TotalRecords > 0)
        {
            TotalPages = (int)Math.Ceiling((decimal)TotalRecords / PageSize);
            if (PageNumber > TotalPages)
            {
                PageNumber = TotalPages;
            }
        }
    }
}