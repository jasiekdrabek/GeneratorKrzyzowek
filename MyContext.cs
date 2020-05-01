using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace KrzyzowkiTabele
{

    class MyContext : DbContext
    {
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


    }
}
