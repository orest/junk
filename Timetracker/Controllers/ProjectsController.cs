using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Http.Results;
using System.Web.Mvc;
using MyProjects.Helpers;
using MyProjects.Models;
using Timetracker.Data;
using Timetracker.Entities.Models;

namespace MyProjects.Controllers
{
    public class ProjectsController : BaseController
    {
        private TimeTrakerContext db = new TimeTrakerContext();

        // GET: Projects
        public ActionResult Index(bool activeOnly = true)
        {
            var query = db.Projects.Include(p => p.Client).Include(p => p.TimeSheet);
            if (activeOnly)
                query = query.Where(p => p.ProjectStatusId == 1);

            query = query.OrderByDescending(p => p.Rate);
            var projects = query.ToList();
            var pr = projects.Select(p => new ProjectVm()
            {
                Project = p
            });

            return View(pr);
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Projects/Create
        public ActionResult Create()
        {
            ViewBag.ProjectStatusId = LookProjectStatus.ToSelectList();
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "CompanyName");
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Project project)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "CompanyName", project.ClientId);
            return View(project);
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "CompanyName", project.ClientId);
            ViewBag.ProjectStatus = LookProjectStatus.ToSelectList();
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProjectId,ClientId,Title,Description,ProjectStatusId,StartDate,Duration,Rate,MaxHoursPerWeek,MinHoursPerWeek,Notes")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "CompanyName", project.ClientId);
            ViewBag.ProjectStatus = LookProjectStatus.ToSelectList();
            return View(project);
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Start(int id)
        {
            //make sure everything is stopped first
            var openLogs = db.WorkLogs.Where(w => w.ProjectId == id && w.EndDate == null).ToList();
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
                ProjectId = id,
                StartDate = today,
                WeekId = Helper.GetWeek(today)
            };
            db.WorkLogs.Add(workLog);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Stop(int id, string note)
        {
            ProcessStopForProject(id, note);
            return RedirectToAction("Index");
        }

        private void ProcessStopForProject(int projectId, string note)
        {
            var workItems = db.WorkLogs.Where(w => w.ProjectId == projectId && w.EndDate == null).ToList();
            foreach (var item in workItems)
            {
                item.EndDate = DateTime.Now;
                item.ElapsedMinutes = (int)(item.EndDate.Value - item.StartDate).TotalMinutes;
                item.Notes = note;
            }
            db.SaveChanges();
        }

        public ActionResult GetTime(int id)
        {
            var time = 0;
            var workItems = db.WorkLogs.FirstOrDefault(w => w.ProjectId == id && w.EndDate == null);
            if (workItems != null)
            {
                time = (int)(DateTime.Now - workItems.StartDate).TotalMinutes;
            }

            return Json(new
            {
                Elapsed = time
            }, JsonRequestBehavior.AllowGet);
        }
    }


}



