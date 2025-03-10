using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SkincareBookingSystem.DataAccess.Seed;
using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.DBContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Appointments> Appointments { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogCategory> BlogCategory { get; set; }
        public DbSet<ComboItem> ComboItem { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerSkinTest> CustomerSkinTest { get; set; }
        public DbSet<DurationItem> DurationItem { get; set; }
        public DbSet<Feedbacks> Feedbacks { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<ServiceDuration> ServiceDurations { get; set; }
        public DbSet<Services> Services { get; set; }
        public DbSet<ServiceCombo> ServiceCombo { get; set; }
        public DbSet<ServiceType> ServiceType { get; set; }
        public DbSet<SkinProfile> SkinProfile { get; set; }
        public DbSet<SkinServiceType> SkinServiceType { get; set; }
        public DbSet<SkinTest> SkinTest { get; set; }
        public DbSet<SkinTherapist> SkinTherapist { get; set; }
        public DbSet<Slot> Slot { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<TestAnswer> TestAnswer { get; set; }
        public DbSet<TestQuestion> TestQuestion { get; set; }
        public DbSet<TherapistSchedule> TherapistSchedules { get; set; }
        public DbSet<TypeItem> TypeItem { get; set; }
        public DbSet<TherapistServiceType> TherapistServiceTypes { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            //Seed data
            ApplicationDbContextSeed.SeedAdminAndManagerAccount(modelBuilder);
            
            // Composite key for ComboItem
            modelBuilder.Entity<ComboItem>()
                .HasKey(ci => new { ci.ServiceId, ci.ServiceComboId });
            
            modelBuilder.Entity<ComboItem>()
                .HasOne(ci => ci.Services)
                .WithMany(s => s.ComboItems)
                .HasForeignKey(ci => ci.ServiceId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<ComboItem>()
                .HasOne(ci => ci.ServiceCombo)
                .WithMany(sc => sc.ComboItems)
                .HasForeignKey(ci => ci.ServiceComboId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Composite key for TypeItem
            modelBuilder.Entity<TypeItem>()
                .HasKey(ti => new { ti.ServiceId, ti.ServiceTypeId });
            
            modelBuilder.Entity<TypeItem>()
                .HasOne(ti => ti.Services)
                .WithMany(s=> s.TypeItems)
                .HasForeignKey(ti => ti.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<TypeItem>()
                .HasOne(ti => ti.ServiceType)
                .WithMany(st=> st.TypeItems)
                .HasForeignKey(ti => ti.ServiceTypeId)
                .OnDelete(DeleteBehavior.Cascade);
                
            // Composite key for SkinServiceType
            modelBuilder.Entity<SkinServiceType>()
                .HasKey(sst => new { sst.SkinProfileId, sst.ServiceTypeId });
            
            modelBuilder.Entity<SkinServiceType>()
                .HasOne(sst => sst.ServiceType)
                .WithMany(st=> st.SkinServiceTypes)
                .HasForeignKey(sst => sst.ServiceTypeId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<SkinServiceType>()
                .HasOne(sst => sst.SkinProfile)
                .WithMany(sp=> sp.SkinServiceTypes)
                .HasForeignKey(sst => sst.SkinProfileId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Composite key for DurationItem
            modelBuilder.Entity<DurationItem>()
                .HasKey(di => new { di.ServiceId, di.ServiceDurationId });
            
            modelBuilder.Entity<DurationItem>()
                .HasOne(di => di.Services)
                .WithMany(s => s.DurationItems)
                .HasForeignKey(di => di.ServiceId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<DurationItem>()
                .HasOne(di => di.ServiceDuration)
                .WithMany(d => d.DurationItems)
                .HasForeignKey(di => di.ServiceDurationId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Composite key for Appointment 
            modelBuilder.Entity<Appointments>()  
                .HasKey(a => a.AppointmentId);  

            modelBuilder.Entity<Appointments>()  
                .HasOne(a => a.Customer)  
                .WithMany()
                .HasForeignKey(a => a.CustomerId)  
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointments>()  
                .HasOne(a => a.Order)  
                .WithMany() 
                .HasForeignKey(a => a.OrderId)  
                .OnDelete(DeleteBehavior.Restrict); 

            // Composite key for Order
            modelBuilder.Entity<Order>()  
                .HasKey(o => o.OrderId);  

            modelBuilder.Entity<Order>()  
                .HasOne(o => o.Customer)  
                .WithMany()   
                .HasForeignKey(o => o.CustomerId)  
                .OnDelete(DeleteBehavior.Restrict);
            
            // Composite key for ComboItem
            modelBuilder.Entity<TherapistServiceType>()
                .HasKey(tst => new {tst.TherapistId , tst.ServiceTypeId });
            
            modelBuilder.Entity<TherapistServiceType>()
                .HasOne(tst => tst.SkinTherapist)
                .WithMany(st => st.TherapistServiceTypes)
                .HasForeignKey(tst => tst.TherapistId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<TherapistServiceType>()
                .HasOne(tst => tst.ServiceType)
                .WithMany(st => st.TherapistServiceTypes)
                .HasForeignKey(tst => tst.ServiceTypeId)
                .OnDelete(DeleteBehavior.Cascade);
            
            
            // Composite key for TherapistSchedule
            modelBuilder.Entity<TherapistSchedule>()
                .HasOne(ts => ts.Appointment)
                .WithMany(a => a.TherapistSchedules)
                .HasForeignKey(ts => ts.AppointmentId)
                .OnDelete(DeleteBehavior.Restrict); // Không xóa khi Appointment bị xóa

            // Thiết lập quan hệ giữa TherapistSchedule và Slot (1-1)
            modelBuilder.Entity<TherapistSchedule>()
                .HasOne(ts => ts.Slot)
                .WithMany(s => s.TherapistSchedules)
                .HasForeignKey(ts => ts.SlotId)
                .OnDelete(DeleteBehavior.Restrict);

            // Thiết lập quan hệ giữa TherapistSchedule và SkinTherapist (1-1)
            modelBuilder.Entity<TherapistSchedule>()
                .HasOne(ts => ts.SkinTherapist)
                .WithMany(st => st.TherapistSchedules)
                .HasForeignKey(ts => ts.TherapistId)
                .OnDelete(DeleteBehavior.Restrict);
            
            //OrderNumber is unique
            modelBuilder.Entity<Order>()
                .HasIndex(o => o.OrderNumber)
                .IsUnique();
        
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Orders)
                .WithMany()
                .HasForeignKey(p => p.OrderNumber)
                .HasPrincipalKey(o => o.OrderNumber)
                .OnDelete(DeleteBehavior.Restrict);
            
            /*modelBuilder.Entity<ComboItem>()
                .HasIndex(p => p.Priority)
                .IsUnique();*/
        }
            
    }
}
