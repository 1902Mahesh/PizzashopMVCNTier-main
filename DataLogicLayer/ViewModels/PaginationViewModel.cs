using System;

namespace DataLogicLayer.ViewModels;

public class PaginationViewModel
{
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int TotalRecords { get; set; }

    public int CurrentPage { get; set; }

    public int FromRec { get; set; }
    public int ToRec { get; set; }
}

