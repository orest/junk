using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using MyProjects.Models;

namespace MyProjects.Data
{
    public class TimeTrakerContext : DbContext
    {
        public TimeTrakerContext()
            : base("name=DefaultConnection")
        {

        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<WorkLog> WorkLogs { get; set; }
        public DbSet<Client> Clients { get; set; }
    }
}