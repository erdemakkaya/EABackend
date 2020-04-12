using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EA.Application.Common.Enttiy;
using EA.Application.Data.Entitites;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EA.Application.Data.Context
{
    /// <summary>
    /// Bu sınıf bizim DbContext sınıfımız yani database ile ilgili tanımlamaların ve bağlantıların bulunduğu sınıf 
    /// Normal şartlarda DbContext base sınıfından kalıtım almamız gerekiyor ancak projede Identity alt yapısını kullanacağımız için
    /// IdentityDbContext kullanıyoruz, User ve Role sınıflarımızı bu generic base class a tip olarak gönderiyoruz.
    /// Aslında IdentityDbContext i inceleyecek olursanız onunda DbContext base classından kalıtım aldığını görebilirsiniz.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid, IdentityUserClaim<Guid>, ApplicationUserRole, ApplicationUserLogin, IdentityRoleClaim<Guid>, ApplicationUserToken>
    {
        #region Constructor
        /// <summary>
        /// Constructor yani yapıcı method bizim için bu sınıf türetildiğinde devreye ilk girecek olan kısımdır.
        /// Constructor DbContextOptions isminde bir sınıf türetilmesini sağlıyor ve bu sınıfı kalıtım aldığı IdentityDbContext sınıfına parametre olarak gönderiyor.
        /// </summary>
        /// <param name="options"></param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            this.EnsureAutoHistory();
        }
        #endregion

        #region ModelCreating
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.EnableAutoHistory(int.MaxValue);
            modelBuilder.Entity<ApplicationUserRole>().HasKey(p => new { p.UserId, p.RoleId });
        }
        #endregion

        #region OnConfiguring
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
        }
        #endregion
        public override Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            foreach (var auditableEntity in ChangeTracker.Entries<IFullAudited>())
            {
                if (auditableEntity.State == EntityState.Added ||
                    auditableEntity.State == EntityState.Modified)
                {
                    auditableEntity.Entity.LastModificationTime = DateTime.Now;
                    auditableEntity.Entity.LastModifierUserId=Guid.Empty;
                    if (auditableEntity.State == EntityState.Added)
                    {
                        auditableEntity.Entity.CreateDate = DateTime.Now;
                        auditableEntity.Entity.Creator=Guid.Empty;//Current User gelecek
                    }
                }
            }
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        #region DbSets
        /// <summary>
        /// Projemiz içerisinde kullanacağımız tüm entity sınıflarımızın bu kısımda DbSet ile tanımlıyoruz
        /// Bu sayede migration yaparken bu sınıflar veritabanında birer tablo olarak oluşturulacak. ve yapacağımız CRUD işlemler veritabanına yansıyacak.
        /// </summary>
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<AppResource> AppRecource { get; set; }
        #endregion
    }
}

