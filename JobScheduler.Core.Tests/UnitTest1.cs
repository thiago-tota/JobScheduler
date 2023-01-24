using FluentAssertions;
using Microsoft.Extensions.Configuration;

namespace JobScheduler.Core.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.json");
            IConfiguration configuration = configurationBuilder.Build();

            var jobSchedules = configuration.GetSection(JobSchedulesDto.JobSchedules).Get<JobDto[]>();

            var job1 = jobSchedules?.First();
            var job2 = jobSchedules?.Last();

            job1.Should().BeEquivalentTo(GetExpectedJob1());
            job2.Should().BeEquivalentTo(GetExpectedJob2());
        }

        private static JobDto GetExpectedJob1()
        {
            return new JobDto
            {
                Name = "Job1",
                ExecutionTime = "20:00",
                DayOfWeek = "MON",
                DayOfMonth = "1",
                Month = "1",
                Year = "2023",
                IntervalInMinutes = 5
            };
        }

        private static JobDto GetExpectedJob2()
        {
            return new JobDto
            {
                Name = "Job2",
                ExecutionTime = "22:00",
                DayOfWeek = "MON-FRI",
                DayOfMonth = "15",
                Month = "12",
                Year = "2023-2030"
            };
        }
    }
}