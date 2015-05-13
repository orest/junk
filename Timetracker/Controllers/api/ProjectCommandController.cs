using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Newtonsoft.Json;
using Timetracker.Data;
using Timetracker.Entities.Models;
using Timetracker.Helpers;
using Timetracker.Models;
using System.Data.Entity;

namespace Timetracker.Controllers.api
{
    public class ProjectCommandController : ApiController
    {
        private TimeTrakerContext db = new TimeTrakerContext();


        public IHttpActionResult Post(ProjectCommand command)
        {
            switch (command.Action)
            {
                //case "start":
                case ProjectCommandActions.Start:
                    Start(command);
                    break;
                //case "restart":
                case ProjectCommandActions.Restart:
                    ReStart(command);
                    break;
                //case "pause":
                case ProjectCommandActions.Pause:
                    PauseProject(command.ProjectId);
                    break;
                //case "stop":
                case ProjectCommandActions.Stop:
                    string message = command.ActionDetails;
                    int newTime = 0;
                    if (!string.IsNullOrEmpty(message))
                    {
                        try
                        {
                            Dictionary<string, string> details = JsonConvert.DeserializeObject<Dictionary<string, string>>(message);
                            if (details.ContainsKey("message"))
                                message = details["message"];
                            if (details.ContainsKey("message"))
                            {
                                Int32.TryParse(details["time"], out newTime);
                            }
                        }
                        catch { }
                    }
                    ProcessStopForProject(command.ProjectId, message, newTime);
                    break;
                //case "time":
                case ProjectCommandActions.Time:
                    var time = GetTime(command);
                    return Ok(new
                    {
                        Minutes = time,
                        Hours = (time / 60).ToString("N2")
                    });
                    break;
            }

            return Ok("");
        }

        private void PauseProject(int projectId)
        {
            var log = db.WorkLogs.Include(w => w.Fragments).FirstOrDefault(w => w.ProjectId == projectId && w.EndDate == null);
            if (log != null)
            {
                FinalizeFragment(log);
                db.SaveChanges();
            }

        }

        private void ReStart(ProjectCommand command)
        {
            var workLog = db.WorkLogs.FirstOrDefault(w => w.ProjectId == command.ProjectId && w.EndDate == null);
            if (workLog != null)
            {
                var fragment = new WorkLogFragment() { StartDate = DateTime.Now };
                workLog.Fragments.Add(fragment);
                db.SaveChanges();
            }
        }

        private void Start(ProjectCommand command)
        {
            //make sure everything is stopped first
            var openLogs = db.WorkLogs.Where(w => w.EndDate == null).ToList();
            if (openLogs.Any())
            {
                foreach (var item in openLogs)
                    ProcessStopForProject(item.ProjectId, "");

            }

            var today = DateTime.Now;
            var workLog = new WorkLog()
            {
                ProjectId = command.ProjectId,
                StartDate = today,
                WeekId = Helper.GetWeek(today)
            };
            var fragment = new WorkLogFragment() { StartDate = today };
            workLog.Fragments.Add(fragment);
            db.WorkLogs.Add(workLog);
            db.SaveChanges();
        }

        private void ProcessStopForProject(int projectId, string note, int time = 0)
        {
            var log = db.WorkLogs.Include(w => w.Fragments).FirstOrDefault(w => w.ProjectId == projectId && w.EndDate == null);
            if (log != null)
            {
                FinalizeFragment(log);
                log.EndDate = DateTime.Now;
                log.ElapsedMinutes = time > 0 ? time : log.Fragments.Sum(f => f.ElapsedMinutes);
                log.Notes = note;
                db.SaveChanges();
            }


        }


        private void FinalizeFragment(WorkLog log)
        {
            if (log != null)
            {
                var activeFragment = log.Fragments.FirstOrDefault(f => f.EndDate == null);
                if (activeFragment != null)
                {
                    activeFragment.ElapsedMinutes = (int)(DateTime.Now - activeFragment.StartDate).TotalMinutes;
                    activeFragment.EndDate = DateTime.Now;
                }

            }
        }

        private double GetTime(ProjectCommand command)
        {
            var time = 0;
            var log = db.WorkLogs.Include(w => w.Fragments).FirstOrDefault(w => w.ProjectId == command.ProjectId && w.EndDate == null);
            if (log != null)
            {
                FinalizeFragment(log);
                time = log.Fragments.Sum(f => f.ElapsedMinutes);
            }
            return time;
        }
    }


}
