using FluentAssertions;
using JobScheduler.Server.JobBase;
using JobScheduler.Server.Jobs;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace JobScheduler.Core.Tests
{
    public class JobsRegisterTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private IServiceProvider _serviceProvider;

        public JobsRegisterTests(WebApplicationFactory<Program> factory)
        {
            _serviceProvider = factory.Services;
        }

        [Fact]
        public void GivenCronJobConfiguration_JobShouldBeInitialized_ThenCronExpressionShouldMatch()
        {
            var scope = _serviceProvider.CreateScope();
            var myFirstCronJob = scope.ServiceProvider.GetRequiredService<IScheduleConfig<MyFirstCronJob>>();
            var mySecondCronJob = scope.ServiceProvider.GetRequiredService<IScheduleConfig<MySecondCronJob>>();

            myFirstCronJob.CronExpression.Should().Be("0/2 * * * *");
            mySecondCronJob.CronExpression.Should().Be("1/2 * * * *");
        }
    }
}