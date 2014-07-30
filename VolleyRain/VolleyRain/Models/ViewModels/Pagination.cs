using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VolleyRain.Models
{
    public class Pagination
    {
        public Pagination(int pageSize, int totalItemCount, int? currentPage)
        {
            PageSize = pageSize;
            TotalItemCount = totalItemCount;
            PageCount = (int)Math.Ceiling(TotalItemCount / (double)PageSize);
            CurrentPage = currentPage.HasValue && currentPage.Value >= 1 && currentPage.Value <= PageCount ? currentPage.Value : 1;
        }

        public int PageSize { get; private set; }

        public int TotalItemCount { get; private set; }

        public int PageCount { get; private set; }

        public int CurrentPage { get; private set; }

        public bool HasPreviousPage { get { return CurrentPage > 1; } }

        public int PreviousPage { get { return CurrentPage - 1; } }

        public bool HasNextPage { get { return CurrentPage < PageCount; } }

        public int NextPage { get { return CurrentPage + 1; } }

        public int ItemsToSkip { get { return (CurrentPage - 1) * PageSize; } }

        public void JumpToItem(int index)
        {
            CurrentPage = (int)Math.Ceiling(index / (double)PageSize);
        }
    }
}