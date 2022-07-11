using BookStore.Models.DataModels;
using System.Collections.Generic;

namespace BookStore.Models.Models
{
    public class ListResponse<T> where T : class
    {
        public IEnumerable<T> records { get; set; }

        public int totalRecords { get; set; }
    }
}
