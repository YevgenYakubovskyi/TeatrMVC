﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theatr.Web.Models;

namespace Theatr.Web.Models
{
    public class PTUSoloViewModel
    {

        public PerformanceViewModel Performance { get; set; }
        public UserViewModel User { get; set; }
        public TicketViewModel Ticket { get; set; }
    }
}