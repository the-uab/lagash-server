﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Wargos.Lagash.Entities;
using LagashServer.Migrations;

namespace LagashServer.helper
{
    public class LagashContext : DbContext
    {
        public LagashContext() : base("LagashContext")
        {
            // Database.SetInitializer(new LagashInitializer());
            // Database.SetInitializer(new MigrateDatabaseToLatestVersion<LagashContext, Configuration>());
        }

        public DbSet<User> users { get; set; }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
