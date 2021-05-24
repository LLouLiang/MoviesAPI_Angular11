using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularMoviesAPI.DTOs
{
    public class PaginationDTO
    {
        public int page { get; set; } = 1;
        private int recordsPerPaged= 10;
        private readonly int maxPages = 50;

        public int RecordsPerPage
        {
            get
            {
                return recordsPerPaged;
            }
            set
            {
                recordsPerPaged = (value > maxPages) ? maxPages : value;
            }
        }
    }
}
