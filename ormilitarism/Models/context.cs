using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace ormilitarism.Models
{
    public class context: DbContext
    {
        public DbSet<postimgs> postimgs { get; set; }
        public DbSet<status> statuses { get; set; }
        public DbSet<title> titles { get; set; }
        public DbSet<customer> customers { get; set; }
        public DbSet<admin> admins { get; set; }
        public DbSet<post> posts { get; set; }
        public DbSet<like> likes { get; set; }
        public DbSet<favorit> favorits { get; set; }
        public DbSet<sorgu> sorgus { get; set; }
        public DbSet<report> reports { get; set; }
    }
}