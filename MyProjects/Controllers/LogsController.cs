using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyProjects.Data;
using MyProjects.Helpers;
using MyProjects.Models;
using Timetracker.Entities.Models;

namespace MyProjects.Controllers
{
    public class LogsController : Controller
    {
        private TimeTrakerContext db = new TimeTrakerContext();

        // GET: Logs
        public ActionResult Index(int projectId = 0, string timeSpan = "")
        {
            var workLogs = db.WorkLogs.Include(w => w.Project);
            if (projectId > 0)
                workLogs = workLogs.Where(p => p.ProjectId == projectId);
            if (!string.IsNullOrEmpty(timeSpan))
            {
                int thisWeek = Helper.GetWeek(DateTime.Now);
                workLogs = workLogs.Where(p => p.WeekId == thisWeek);
            }

            var result = workLogs.OrderByDescending(p => p.StartDate).ToList();
            return View(result);
        }

        // GET: Logs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkLog workLog = db.WorkLogs.Find(id);
            if (workLog == null)
            {
                return HttpNotFound();
            }
            return View(workLog);
        }

        // GET: Logs/Create
        public ActionResult Create()
        {
            ViewBag.ProjectId = new SelectList(db.Projects, "ProjectId", "Title");
            return View();
        }

        // POST: Logs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WorkLogId,ProjectId,StartDate,EndDate,ElapsedMinutes,WeekId,Notes")] WorkLog workLog)
        {
            if (ModelState.IsValid)
            {
                db.WorkLogs.Add(workLog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProjectId = new SelectList(db.Projects, "ProjectId", "Title", workLog.ProjectId);
            return View(workLog);
        }

        // GET: Logs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkLog workLog = db.WorkLogs.Find(id);
            if (workLog == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectId = new SelectList(db.Projects, "ProjectId", "Title", workLog.ProjectId);
            return View(workLog);
        }

        // POST: Logs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "WorkLogId,ProjectId,StartDate,EndDate,ElapsedMinutes,WeekId,Notes")] WorkLog workLog)
        {
            if (ModelState.IsValid)
            {
                db.Entry(workLog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectId = new SelectList(db.Projects, "ProjectId", "Title", workLog.ProjectId);
            return View(workLog);
        }

        // GET: Logs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkLog workLog = db.WorkLogs.Find(id);
            if (workLog == null)
            {
                return HttpNotFound();
            }
            return View(workLog);
        }

        // POST: Logs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            WorkLog workLog = db.WorkLogs.Find(id);
            db.WorkLogs.Remove(workLog);
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
    }
}
