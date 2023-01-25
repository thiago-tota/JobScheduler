using FluentAssertions;
using Microsoft.Extensions.Configuration;
using  Microsoft.AspNetCore.Builder;
namespace JobScheduler.Core.Tests
{
    public class JobsRegisterTests
    {
        [Fact]
        public void Test1()
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Host.UseWindowsService();

            // Add services to the container.

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            builder.Services.AddJobs(builder.Configuration);

            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.json");
            IConfiguration configuration = configurationBuilder.Build();

            const string JobSchedules = "JobSchedules";

            var jobSchedules = configuration.GetSection(JobSchedules).GetChildren();

            var myCronFirstJob = jobSchedules?.First();
            var myCronSecondJob = jobSchedules?.Last();

            myCronFirstJob?.Key.Should().Be(GetExpectedFormyCronFirstJob().Key);
            myCronFirstJob?.Value.Should().Be(GetExpectedFormyCronFirstJob().Value);

            myCronSecondJob?.Key.Should().Be(GetExpectedFormyCronSecondJob().Key);
            myCronSecondJob?.Value.Should().Be(GetExpectedFormyCronSecondJob().Value);
        }

        private static KeyValuePair<string, string> GetExpectedFormyCronFirstJob()
        {
            return new KeyValuePair<string, string>(nameof, "*/1 21 * * *");
        }

        private static KeyValuePair<string, string> GetExpectedFormyCronSecondJob()
        {
            return new KeyValuePair<string, string>("MyCronSecondJob", "* 15 21 * * MON-FRI");
        }
    }
}