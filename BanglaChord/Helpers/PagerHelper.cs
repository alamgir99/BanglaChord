using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BanglaChord.Helpers
{
    public class PagerHelper
    {
        public int totPage { get; set; } // total number of pages to deal with
        public int curPage { get; set; } // current page that is being viewed
        public int pageCount { get; set; } // how many pages to show in the pager
        public bool useDot { get; set; }

        int pageStart, pageEnd; //starting and enging number of the pager

        public PagerHelper(int tP, int pC)
        {
            totPage = tP < 1 ? 1 : tP;

            pageCount = pC < 5 ? 5: pC; // not less than 5
            pageCount = pageCount > 100 ? 15 : pageCount; // not more than 15

            pageCount = pageCount > tP ? tP : pageCount; // not more than total page
            pageStart = 1;
            pageEnd = pageCount;
            curPage = 1;
            useDot = true;
        }

        public void SetCurrentPage(int current_page)
        {
            if (pageCount == 0)
                return;

             if (current_page == pageEnd) // current page is at the end of the pager
            {
                 if(totPage <= pageCount)
                    pageStart = current_page - pageCount > 0 ? current_page - pageCount : 1;
                 else
                     pageStart = current_page - 1 > 0 ? current_page - 1 : 1;

                pageEnd = pageStart + pageCount < totPage ? pageStart + pageCount : totPage;

                if (pageEnd - pageStart <= pageCount)
                    useDot = false;
                else
                    useDot = true;
            }
             if (current_page > pageEnd) // tricky situation
             {
                 pageStart = ((int)current_page / pageCount)*pageCount;
                 pageEnd = pageStart + pageCount < totPage ? pageStart + pageCount : totPage;
                 if (pageEnd - pageStart <= pageCount)
                     useDot = false;
                 else
                     useDot = true;
             }

            //too few pages
            if (totPage < pageCount)
                useDot = false;
            //else
             //   useDot = true;

            //current page is the last page of total page
            if (pageEnd == totPage)
            {
                useDot = false;
            }
        }

        public IEnumerable<int> GetPages()
        {
            if (pageCount == 0)
                yield return 0;

            for (int i = pageStart; i <= pageEnd; i++)
            {
                yield return i;
            }
        }
    }
}