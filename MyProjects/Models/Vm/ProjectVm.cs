using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyProjects.Helpers;

namespace MyProjects.Models
{
    public class ProjectVm
    {
        public  ProjectVm()
        {
            ProjectStatuses = new List<SelectListItem>()
            {
                new SelectListItem() {Value = "ACTV", Text = "Active"},
                new SelectListItem() {Value = "CMLT", Text = "Completed"},
            };

        }
        public Project Project { get; set; }
        public bool HasActiveLog {
            get
            {
                var hasActive = false;
                if (Project.TimeSheet.Any())
                {
                    hasActive = Project.TimeSheet.Any(t => t.EndDate == null);
                }
                return hasActive;
            }
        }
        public int TotalTime
        {
            get
            {
                var thisWeek = Helper.GetWeek(DateTime.Today);
                var total = 0;
                if (Project.TimeSheet.Any())
                {
                    total = Project.TimeSheet.Where(w=>w.WeekId==thisWeek).Sum(t => t.ElapsedMinutes);
                }
                return total;
            }
        }

        public decimal TotalHours
        {
            get { return Convert.ToDecimal(TotalTime)/60; }
        }
        public IEnumerable<SelectListItem> ProjectStatuses { get; set; }

    }
}