using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Timetracker.Data;
using Timetracker.Entities.Models;
using Timetracker.Helpers;
using Timetracker.Models;

namespace Timetracker.Controllers.api
{
    public class ProjectCommandController : ApiController
    {
        private TimeTrakerContext db = new TimeTrakerContext();

        public IHttpActionResult Post(ProjectCommand command)
        {
            switch (command.Action)
            {
                case "start":
                    Start(command);
                    break;
                case "stop":
                    ProcessStopForProject(command.ProjectId, command.ActionDetails);
                    break;
                case "time":
                    var time = GetTime(command);
                    return Ok(new
                    {
                        Minutes = time,
                        Hours=(time/60).ToString("N2")
                    });
                    break;
            }

            return Ok("");
        }

        private void Start(ProjectCommand command)
        {
            //make sure everything is stopped first
            var openLogs = db.WorkLogs.Where(w => w.EndDate == null).ToList();
            if (openLogs.Any())
            {
                foreach (var item in openLogs)
                {
                    ProcessStopForProject(item.ProjectId, "");
                }
            }

            var today = DateTime.Now;
            var workLog = new WorkLog()
            {
                ProjectId = command.ProjectId,
                StartDate = today,
                WeekId = Helper.GetWeek(today)
            };
            db.WorkLogs.Add(workLog);
            db.SaveChanges();
        }

        private void ProcessStopForProject(int projectId, string note, int time = 0)
        {
            var workItems = db.WorkLogs.Where(w => w.ProjectId == projectId && w.EndDate == null).ToList();
            foreach (var item in workItems)
            {
                item.EndDate = DateTime.Now;
                if (time > 0)
                    item.ElapsedMinutes = time;
                else
                    item.ElapsedMinutes = (int)(item.EndDate.Value - item.StartDate).TotalMinutes;
                item.Notes = note;
            }
            db.SaveChanges();
        }

        private double GetTime(ProjectCommand command)
        {
            var time = 0;
            var workItems = db.WorkLogs.FirstOrDefault(w => w.ProjectId == command.ProjectId && w.EndDate == null);
            if (workItems != null)
            {
                time = (int)(DateTime.Now - workItems.StartDate).TotalMinutes;
            }
            return time;
        }
    }


}
