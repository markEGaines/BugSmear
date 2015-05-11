using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugSmear.Models
{
    public class ProjectInfo
    {
        public string ProjectName { get; set; }
        public int NumTickets { get; set; }
    }

    public class DevInfo
    {
        public string DevName { get; set; }
        public int NumTickets { get; set; }
    }

    public class DashboardViewModel
    {
        public DashboardViewModel()
        {
            projInfo = new List<ProjectInfo>();
            devInfo = new List<DevInfo>();
        }
        public List<ProjectInfo> projInfo { get; set; }
        public List<DevInfo> devInfo { get; set; }
        public int TicketsAssigned { get; set; }
        public int TicketsNotAssigned { get; set; }
        public int TicketsResolved { get; set; }
        public int TicketsOpen { get; set; }
        public int TicketsDue3 { get; set; }
        public int TicketsOverDue { get; set; }


    }
}