﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TutorApplication.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CHATTERJEECDEntities1 : DbContext
    {
        public CHATTERJEECDEntities1()
            : base("name=CHATTERJEECDEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<city> cities { get; set; }
        public virtual DbSet<Contact_tbl> Contact_tbl { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<StudentSingup> StudentSingups { get; set; }
        public virtual DbSet<Tutorregi> Tutorregis { get; set; }
    }
}