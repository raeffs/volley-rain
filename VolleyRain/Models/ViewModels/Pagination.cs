using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VolleyRain.Models
{
    public class Pagination
    {
        private readonly bool allowZero;

        public Pagination(int pageSize, int totalItemCount, int? currentPage, bool allowZero = false)
        {
            PageSize = pageSize;
            TotalItemCount = totalItemCount;
            PageCount = (int)Math.Ceiling(TotalItemCount / (double)PageSize);
            CurrentPage = currentPage.HasValue && currentPage.Value >= 1 && currentPage.Value <= PageCount ? currentPage.Value : 1;
            this.allowZero = allowZero;
        }

        public int PageSize { get; private set; }

        public int TotalItemCount { get; private set; }

        public int PageCount { get; private set; }

        public int CurrentPage { get; private set; }

        public virtual bool HasPreviousPage { get { return CurrentPage > 1 || allowZero; } }

        public virtual int PreviousPage { get { return CurrentPage - 1; } }

        public virtual bool HasNextPage { get { return CurrentPage < PageCount; } }

        public virtual int NextPage { get { return CurrentPage + 1; } }

        public int ItemsToSkip { get { return (CurrentPage - 1) * PageSize; } }
    }

    public class ReversePagination : Pagination
    {
        public ReversePagination(int pageSize, int totalItemCount, int? currentPage, bool allowZero = false)
            : base(pageSize, totalItemCount, currentPage, allowZero)
        {
        }

        public override bool HasPreviousPage { get { return base.HasNextPage; } }

        public override int PreviousPage { get { return base.NextPage; } }

        public override bool HasNextPage { get { return base.HasPreviousPage; } }

        public override int NextPage { get { return base.PreviousPage; } }
    }
}