using System;

namespace DataLogicLayer.ViewModels;

public class PaginationViewModel
{
     public int PageNo { get; set; }
    public int PageSize { get; set; }
    public int TotalRecords { get; set; }
    public string ActionName { get; set; }
    public string ControllerName { get; set; }
}

