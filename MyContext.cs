using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using KrzyzowkiTabele.Migrations;

namespace KrzyzowkiTabele
{

    class MyContext : DbContext
    {
        public MyContext() : base("KrzyzowkiTabele.MyContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<MyContext, Configuration>());
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public virtual DbSet<Hasla3> Hasla3s { get; set; }
        public virtual DbSet<Hasla4> Hasla4s { get; set; }
        public virtual DbSet<Hasla5> Hasla5s { get; set; }
        public virtual DbSet<Hasla6> Hasla6s { get; set; }
        public virtual DbSet<Hasla7> Hasla7s { get; set; }
        public virtual DbSet<Hasla8> Hasla8s { get; set; }
        public virtual DbSet<Hasla9> Hasla9s { get; set; }
        public virtual DbSet<Hasla10> Hasla10s { get; set; }
        public virtual DbSet<Hasla11> Hasla11s { get; set; }
        public virtual DbSet<Hasla12> Hasla12s { get; set; }
        public virtual DbSet<Hasla13> Hasla13s { get; set; }
        public virtual DbSet<Hasla14> Hasla14s { get; set; }
        public virtual DbSet<Hasla15> Hasla15s { get; set; }
        public virtual DbSet<Hasla3Distinct> Hasla3Distincts { get; set; }
        public virtual DbSet<Hasla4Distinct> Hasla4Distincts { get; set; }
        public virtual DbSet<Hasla5Distinct> Hasla5Distincts { get; set; }
        public virtual DbSet<Hasla6Distinct> Hasla6Distincts { get; set; }
        public virtual DbSet<Hasla7Distinct> Hasla7Distincts { get; set; }
        public virtual DbSet<Hasla8Distinct> Hasla8Distincts { get; set; }
        public virtual DbSet<Hasla9Distinct> Hasla9Distincts { get; set; }
        public virtual DbSet<Hasla10Distinct> Hasla10Distincts { get; set; }
        public virtual DbSet<Hasla11Distinct> Hasla11Distincts { get; set; }
        public virtual DbSet<Hasla12Distinct> Hasla12Distincts { get; set; }
        public virtual DbSet<Hasla13Distinct> Hasla13Distincts { get; set; }
        public virtual DbSet<Hasla14Distinct> Hasla14Distincts { get; set; }
        public virtual DbSet<Hasla15Distinct> Hasla15Distincts { get; set; }
    }
}
