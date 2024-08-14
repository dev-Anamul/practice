﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dto
{
    public class PagedResultFacilityAdministratorDto<T>
    {
        public List<T> Data { get; set; }
        public int TotalItemsActive { get; set; }
        public int TotalItemsRecover{ get; set; }
        public int TotalItemsPending{ get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
