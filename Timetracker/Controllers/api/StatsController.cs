using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using Timetracker.Data;
using Timetracker.Models;

namespace Timetracker.Controllers.api
{

    public class StatsController : ApiController
    {
        private TimeTrakerContext db = new TimeTrakerContext();

        public IHttpActionResult Get(string action)
        {
            var currWeek = Helpers.Helper.GetWeek(DateTime.Now);
            action = action.ToLower();
            switch (action)
            {
                case "weeklyreport":
                    var data = db.WorkLogs.Where(w => w.WeekId == currWeek)
                            .GroupBy(w => w.ProjectId)
                            .Select(p => new
                            {
                                ProjectId = p.Key,
                                ElapsedMinutes = p.Sum(c => c.ElapsedMinutes)
                            });

                    var projects = db.Projects.ToList();
                    var projectData = projects.Join(data,
                        pr => pr.ProjectId,
                        dt => dt.ProjectId,
                        (p, d) => new
                        {
                            p.ProjectId,
                            p.Title,
                            Elapsed = TotalHours(d.ElapsedMinutes)
                        }
                        );
                    return Ok(projectData);
                    break;
            }

            return Ok("");
        }

        public string TotalHours(int minutes)
        {

            var total = Convert.ToDecimal(minutes) / 60;
            return total.ToString("N1");

        }

    }


}
