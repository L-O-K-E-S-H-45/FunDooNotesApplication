using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Context
{
    public class FunDooDBContext : DbContext
    {

        public FunDooDBContext(DbContextOptions dbContext) : base(dbContext) { }

        public DbSet<UserEntity> Users { get; set; }

        public DbSet<ReviewEntity> Reviews { get; set; }
        public DbSet<NotesEntity> Notes { get; set; }
        public DbSet<LabelEntity> Labels { get; set; }

        //public DbSet<LabelsLogEntity> LabelsLog { get; set; }



    }
}
