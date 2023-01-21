using JobScheduler.Core;
using JobScheduler.Infraestructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobScheduler.Server.Controllers
{
    [ApiController]
    public class SchedulerController : ControllerBase, IApi
    {
        private readonly JobSchedulerDbContext _db;
        public SchedulerController(JobSchedulerDbContext db)
        {
            _db = db;
        }

        [HttpGet(ApiPaths.SchedulerHistory)]
        public async Task<List<JobDto>> GetSchedulerHistory()
        {
            await _db.Database.EnsureCreatedAsync();
            return await _db.RunHistory.Select(x => new JobDto
            {
                Name = x.JobName,
                Status = x.Status,
                TimeStart = x.StartTime,
                TimeEnd = x.EndTime
            }).ToListAsync();
            //return new List<JobDto>
            //{
            //    new JobDto
            //    {
            //        Name = "Job 1 test",
            //        Status = Shared.Enums.JobStatus.Success,
            //        TimeStart = DateTime.Now.AddHours(-2),
            //        TimeEnd = DateTime.Now.AddHours(-1).AddMinutes(-30),
            //    },
            //    new JobDto
            //    {
            //        Name = "Job 2",
            //        Status = Shared.Enums.JobStatus.Fail,
            //        TimeStart = DateTime.Now.AddHours(-1),
            //        TimeEnd = DateTime.Now.AddHours(-1).AddMinutes(23),
            //    },
            //    new JobDto
            //    {
            //        Name = "Jobbo testero",
            //        Status = Shared.Enums.JobStatus.Success,
            //        TimeStart = DateTime.Now.AddMinutes(-5),
            //        TimeEnd = DateTime.Now.AddMinutes(-4),
            //    },
            //};
        }
    }
}