using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Timetracker.Entities.Models;
using Timetracker.Helpers;

namespace Timetracker.Models
{
    public class ProjectVm
    {

        public ProjectVm()
        {
        }
        public Project Project { get; set; }
        public bool HasActiveLog
        {
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

        public bool IsPaused
        {
            get
            {
                if (Project.TimeSheet.Any())
                {
                    var activeLog = Project.TimeSheet.FirstOrDefault(t => t.EndDate == null);
                    if (activeLog != null)
                    {
                        var activeFragment = activeLog.Fragments.Where(f => f.EndDate == null);
                        return !activeFragment.Any();
                    }
                }

                return false;
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
                    total = Project.TimeSheet.Where(w => w.WeekId == thisWeek).Sum(t => t.ElapsedMinutes);
                }
                return total;
            }
        }

        public string TotalHours
        {
            get
            {
                var total = Convert.ToDecimal(TotalTime) / 60;
                return total.ToString("N1");
            }
        }

        public IEnumerable<SelectListItem> ProjectStatuses { get; set; }


    }
}