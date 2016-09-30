using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Timetracker.Data;
using Timetracker.Entities.Models;
using Timetracker.Models;

namespace Timetracker.Controllers.api
{
    public class ProjectController : ApiController
    {
        private TimeTrakerContext db = new TimeTrakerContext();

        // GET: api/Project
        public IHttpActionResult GetProjects()
        {
            var query = db.Projects.Include(p => p.Client)
                .Include(p => p.TimeSheet)                
                .Include(p=>p.TimeSheet.Select(t=>t.Fragments))
                .Include(p => p.Tasks);
            query = query.Where(p => p.ProjectStatusId == 1);            
            query = query.OrderByDescending(p => p.Rate);
            var projects = query.ToList();
            var pr = projects.Select(p => new ProjectVm()
            {
                Project = p
            });

            pr = pr.OrderByDescending(p => p.HasActiveLog).ThenByDescending( p => p.Project.Rate).ThenBy(p=>p.Project.ClientId);
            return Ok(pr);
        }

        // GET: api/Project/5
        [ResponseType(typeof(Project))]
        public IHttpActionResult GetProject(int id)
        {
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return NotFound();
            }

            return Ok(project);
        }

        // PUT: api/Project/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProject(int id, Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != project.ProjectId)
            {
                return BadRequest();
            }

            db.Entry(project).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Project
        [ResponseType(typeof(Project))]
        public IHttpActionResult PostProject(Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Projects.Add(project);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = project.ProjectId }, project);
        }

        // DELETE: api/Project/5
        [ResponseType(typeof(Project))]
        public IHttpActionResult DeleteProject(int id)
        {
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return NotFound();
            }

            db.Projects.Remove(project);
            db.SaveChanges();

            return Ok(project);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProjectExists(int id)
        {
            return db.Projects.Count(e => e.ProjectId == id) > 0;
        }
    }
}