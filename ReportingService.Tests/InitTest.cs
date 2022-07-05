using Microsoft.EntityFrameworkCore;
using ReportingService.DAL.EF;
using ReportingService.DAL.Models;

namespace ReportingService.Tests
{
    [TestClass]
    public abstract class InitTest
    {
        protected DbContextOptions<ApplicationContext> ContextOptions { get; }
        protected InitTest(DbContextOptions<ApplicationContext> contextOptions)
        {
            ContextOptions = contextOptions;
            Seed();
        }

        private void Seed()
        {
            using var context = new ApplicationContext(ContextOptions);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var user = new User()
            {
                Id = "12345678",
                Email = "test@test.com",
                First_Name = "Salavat",
                Last_Name = "Miftakhov",
                UserName = "Salet"
            };

            var report = new Report()
            {
                Id = 1,
                Date = DateTime.UtcNow,
                Name = "Report",
                UserId = user.Id,
                User = user
            };

            var provider1 = new Provider()
            {
                Id = 1,
                Name = "DomRu",
                Address = "Kazan, Kremlin street, 1"
            };

            var provider2 = new Provider()
            {
                Id = 2,
                Name = "Rostelecom",
                Address = "Kazan, Amirkhan street, 1"
            };

            var service1 = new Service()
            {
                Id = 1,
                Type = "Building",
                Provider = provider1,
                ProviderId = 1,
                Report = report,
                ReportId = 1,
                Sum = 500
            };

            var service2 = new Service()
            {
                Id = 2,
                Type = "Working",
                Provider = provider1,
                ProviderId = 1,
                Report = report,
                ReportId = 1,
                Sum = 300
            };

            var service3 = new Service()
            {
                Id = 3,
                Type = "Security",
                Provider = provider1,
                ProviderId = 1,
                Report = report,
                ReportId = 1,
                Sum = 5000
            };

            var service4 = new Service()
            {
                Id = 4,
                Type = "Working",
                Provider = provider2,
                ProviderId = 2,
                Report = report,
                ReportId = 1,
                Sum = 100
            };

            context.AddRange(user, report, provider1, provider2, service1, service2, service3, service4);
            context.SaveChanges();
        }
    }
}