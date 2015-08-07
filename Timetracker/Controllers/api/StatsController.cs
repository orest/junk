using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using Timetracker.Data;
using Timetracker.Models;
using System.Data.Entity;

namespace Timetracker.Controllers.api {

    public class StatsController : ApiController {
        private TimeTrakerContext db = new TimeTrakerContext();

        public IHttpActionResult Get(string action, string filter = "") {

            var currWeek = Helpers.Helper.GetWeek(DateTime.Now);
            action = action.ToLower();
            if (!string.IsNullOrEmpty(filter))
                filter = filter.ToLower();
            var today = DateTime.Now;

            switch (action) {
                case "weeklyreport":
                    var query = db.WorkLogs.Include(p => p.Fragments);
                    if (filter == "last") {
                        query = query.Where(w => w.WeekId == currWeek - 1);
                    } else if (filter == "today") {
                        today = new DateTime(today.Year, today.Month, today.Day);
                        query = query.Where(w => w.StartDate > today);
                    } else if (filter == "month") {
                        var firstOfThisMonth = new DateTime(today.Year, today.Month, 1);
                        query = query.Where(w => w.StartDate > firstOfThisMonth);
                    } else {
                        query = query.Where(w => w.WeekId == currWeek);
                    }


                    var data = query.GroupBy(w => w.ProjectId)
                            .Select(p => new {
                                ProjectId = p.Key,
                                ElapsedMinutes = p.Sum(c => c.ElapsedMinutes)
                            });

                    //var data = db.WorkLogs.Where(w => w.WeekId == currWeek)
                    //        .GroupBy(w => w.ProjectId)
                    //        .Select(p => new
                    //        {
                    //            ProjectId = p.Key,
                    //            ElapsedMinutes = p.Sum(c => c.ElapsedMinutes)
                    //        });

                    var projects = db.Projects.ToList();
                    var projectData = projects.Join(data,
                        pr => pr.ProjectId,
                        dt => dt.ProjectId,
                        (p, d) => new {
                            p.ProjectId,
                            p.Title,
                            Elapsed = TotalHours(d.ElapsedMinutes),
                            p.Rate,
                            Total = (Convert.ToDecimal(d.ElapsedMinutes) / 60) * p.Rate
                        }
                        );
                    return Ok(projectData);
                case "week-series":
                    var report = db.WorkLogs
                            .GroupBy(w => w.WeekId).ToList()
                            .Select(p => new {
                                WeekId = p.Key,
                                ElapsedHours = TotalHours(p.Sum(c => c.ElapsedMinutes))
                            }).OrderBy(w=>w.WeekId);
                    return Ok(report);
                    break;
            }

            return Ok("");
        }

        public string TotalHours(int minutes) {

            var total = Convert.ToDecimal(minutes) / 60;
            return total.ToString("N1");

        }

    }


}
