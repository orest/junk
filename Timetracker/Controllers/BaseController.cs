using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Timetracker.Data;
using Timetracker.Entities.Models.Lookup;
using MyProjects.Helpers;

namespace MyProjects.Controllers
{
    public class BaseController : Controller
    {

        private readonly InMemoryCache _cacheService = new InMemoryCache();
        private List<ProjectStatus> _projectStatus;
        private List<Frequency> _frequency;
        private List<TimesheetCode> _timesheetCode;


        public BaseController()
        {

        }


        public List<ProjectStatus> LookProjectStatus
        {
            get
            {
                if (_projectStatus.IsNullOrEmpty())
                {
                    var cacheKey = typeof(ProjectStatus).ToString();
                    _projectStatus = _cacheService.Get<List<ProjectStatus>>(cacheKey);
                    if (_projectStatus == null)
                    {
                        using (TimeTrakerContext db = new TimeTrakerContext())
                        {
                            _projectStatus = db.ProjectStatuses.ToList();
                            _cacheService.Add(cacheKey, _projectStatus);
                        }

                    }

                }
                return _projectStatus;
            }
        }

        public List<Frequency> LookFrequency
        {
            get
            {
                if (_frequency.IsNullOrEmpty())
                {
                    var cacheKey = typeof(Frequency).ToString();
                    _frequency = _cacheService.Get<List<Frequency>>(cacheKey);
                    if (_frequency == null)
                    {
                        using (TimeTrakerContext db = new TimeTrakerContext())
                        {
                            _frequency = db.Frequency.ToList();
                            _cacheService.Add(cacheKey, _frequency);
                        }

                    }
                }
                return _frequency;
            }
        }

        public List<TimesheetCode> LookTimesheetCodes
        {
            get
            {
                if (_timesheetCode.IsNullOrEmpty())
                {
                    var cacheKey = typeof(TimesheetCode).ToString();
                    _timesheetCode = _cacheService.Get<List<TimesheetCode>>(cacheKey);
                    if (_timesheetCode == null)
                    {
                        using (TimeTrakerContext db = new TimeTrakerContext())
                        {
                            _timesheetCode = db.TimesheetCodes.ToList();
                            _cacheService.Add(cacheKey, _timesheetCode);
                        }
                    }
                }
                return _timesheetCode;
            }
        }
        protected override void Dispose(bool disposing)
        {
 
            base.Dispose(disposing);
        }


    }


}