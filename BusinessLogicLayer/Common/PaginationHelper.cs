using DataLogicLayer.ViewModels;

namespace BusinessLogicLayer.Common;

public static class PaginationHelper
{
    public static void  SetPagination(
        this PaginationViewModel page,
        int totalRecords,
        int pageSize,
        int pageNumber
    )
    {
        if (totalRecords <= 0)
            return;

        page.TotalRecords = totalRecords;
        page.FromRec = (pageNumber - 1) * pageSize;
        page.ToRec = page.FromRec + pageSize;

        if (page.ToRec > page.TotalRecords)
        {
            page.ToRec = page.TotalRecords;
        }

        page.FromRec += 1;
        page.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
        page.CurrentPage = pageNumber;
        page.PageSize = pageSize;
    }
}
