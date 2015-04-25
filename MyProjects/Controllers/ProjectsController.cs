using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyProjects.Data;
using MyProjects.Helpers;
using MyProjects.Models;

namespace MyProjects.Controllers
{
    public class ProjectsController : Controller
    {
        private TimeTrakerContext db = new TimeTrakerContext();

        // GET: Projects
        public ActionResult Index()
        {
            var projects = db.Projects.Include(p => p.Client).Include(p => p.TimeSheet).OrderByDescending(p=>p.Rate).ToList();
            
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
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "CompanyName");
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProjectId,ClientId,Title,Description,ProjectStatusId,StartDate,Duration,Rate,MaxHoursPerWeek,Notes")] Project project)
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
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProjectId,ClientId,Title,Description,ProjectStatusId,StartDate,Duration,Rate,MaxHoursPerWeek,Notes")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "CompanyName", project.ClientId);
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
            var workItems = db.WorkLogs.Where(w => w.ProjectId == id && w.EndDate ==null).ToList();
            foreach (var item in workItems)
            {
                item.EndDate = DateTime.Now;
                item.ElapsedMinutes = (item.EndDate.Value - item.StartDate).Minutes;
                item.Notes = note;
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}
