using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnePixelBE.Models;

namespace OnePixelBE.EF
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Code> Codes { get; set; }
        public virtual DbSet<Logg> Loggs { get; set; }
        public virtual DbSet<MenuAccessData> MenuAccessDatas { get; set;}
        public virtual DbSet<MenuPart> MenuParts { get; set; }
        public virtual DbSet<ShootingRange> ShootingRanges { get; set; }
        public virtual DbSet<OneRange> OneRanges { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<WebFile> WebFile { get; set; }
        public virtual DbSet<PointModel> Points { get; set; }
        public virtual DbSet<Target> Targets { get; set; }
        public virtual DbSet<CrewStand> CrewStands { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<AvaliblePermissions> AvaliblePermissions { get; set; }
        public virtual DbSet<AvaliblePermissionDetail> AvaliblePermissionDetail { get; set; }
        public virtual DbSet<UserPermission> UserPermissions { get; set; }
        public virtual DbSet<UserPermissionDetail> UserPermissionDetails { get; set; }
        public virtual DbSet<FieldModel> FieldModels { get; set; }
    }
}
