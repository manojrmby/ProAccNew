﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProACC_DB
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class ProAccEntities : DbContext
    {
        public ProAccEntities()
            : base("name=ProAccEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Instance> Instances { get; set; }
        public virtual DbSet<PhaseMaster> PhaseMasters { get; set; }
        public virtual DbSet<ProjectMonitor> ProjectMonitors { get; set; }
        public virtual DbSet<StatusMaster> StatusMasters { get; set; }
        public virtual DbSet<IndustrySector> IndustrySectors { get; set; }
        public virtual DbSet<RoleMaster> RoleMasters { get; set; }
        public virtual DbSet<User_Type> User_Type { get; set; }
        public virtual DbSet<UserMaster> UserMasters { get; set; }
        public virtual DbSet<FileUploadMaster> FileUploadMasters { get; set; }
        public virtual DbSet<ApplicationAreaMaster> ApplicationAreaMasters { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<ActivityMaster> ActivityMasters { get; set; }
        public virtual DbSet<Buldingblock> Buldingblocks { get; set; }
        public virtual DbSet<MailMaster> MailMasters { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ScenarioMaster> ScenarioMasters { get; set; }
        public virtual DbSet<PMTaskCategory> PMTaskCategories { get; set; }
        public virtual DbSet<PMTaskMaster> PMTaskMasters { get; set; }
        public virtual DbSet<PMTaskMonitor_> PMTaskMonitor_ { get; set; }
    
        public virtual ObjectResult<SP_ReadinessReport_Result> SP_ReadinessReport(string type, string instanceId)
        {
            var typeParameter = type != null ?
                new ObjectParameter("Type", type) :
                new ObjectParameter("Type", typeof(string));
    
            var instanceIdParameter = instanceId != null ?
                new ObjectParameter("InstanceId", instanceId) :
                new ObjectParameter("InstanceId", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_ReadinessReport_Result>("SP_ReadinessReport", typeParameter, instanceIdParameter);
        }
    
        public virtual ObjectResult<string> SP_SimplificationReport(string type, string input, string instanceId)
        {
            var typeParameter = type != null ?
                new ObjectParameter("Type", type) :
                new ObjectParameter("Type", typeof(string));
    
            var inputParameter = input != null ?
                new ObjectParameter("Input", input) :
                new ObjectParameter("Input", typeof(string));
    
            var instanceIdParameter = instanceId != null ?
                new ObjectParameter("InstanceId", instanceId) :
                new ObjectParameter("InstanceId", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("SP_SimplificationReport", typeParameter, inputParameter, instanceIdParameter);
        }
    
        public virtual ObjectResult<SP_CustomCode_Result> SP_CustomCode(string type, string instanceId)
        {
            var typeParameter = type != null ?
                new ObjectParameter("Type", type) :
                new ObjectParameter("Type", typeof(string));
    
            var instanceIdParameter = instanceId != null ?
                new ObjectParameter("InstanceId", instanceId) :
                new ObjectParameter("InstanceId", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_CustomCode_Result>("SP_CustomCode", typeParameter, instanceIdParameter);
        }
    
        public virtual ObjectResult<SP_CreateAnalysis_Result> SP_CreateAnalysis(string type)
        {
            var typeParameter = type != null ?
                new ObjectParameter("Type", type) :
                new ObjectParameter("Type", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_CreateAnalysis_Result>("SP_CreateAnalysis", typeParameter);
        }
    }
}
