using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class PageableObject<T>
    {
        public List<T> Elements { get; set; }
        public int TotalPage { get; set; }
        public int TotalRecord { get; set; }

        public PageableObject()
        {
            Elements = new List<T>();
        }
    }
}
