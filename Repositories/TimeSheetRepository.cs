using Microsoft.AspNetCore.Identity;
using QienUrenMachien.Data;
using QienUrenMachien.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QienUrenMachien.Repositories
{
    public class TimeSheetRepository : ITimeSheetRepository
    {
        private readonly RepositoryContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public TimeSheetRepository(RepositoryContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public List<TimeSheet> GetAllTimeSheets()
        {
            var traineeslist = userManager.GetUsersInRoleAsync("Trainee").Result;
            var employeeslist = userManager.GetUsersInRoleAsync("Werknemer").Result;
            var traineesAndEmployeesIds = employeeslist.Concat(traineeslist)
                .Select(e => e.Id);

            return context.TimeSheets
                .Select(t => new TimeSheet { Id = t.Id, SheetID = t.SheetID, Project = t.Project, Month = t.Month, ProjectHours = t.ProjectHours, Overwork = t.Overwork, Sick = t.Sick, Absence = t.Absence, Approved = t.Approved, Other = t.Other, Submitted = t.Submitted, Training = t.Training, Data = t.Data, applicationUser = t.applicationUser })
                .Where(t => traineesAndEmployeesIds.Contains(t.Id))
                .ToList();
        }

        public TimeSheet GetOneTimeSheet(string Id, string Month)
        {
            var entity = context.TimeSheets.Single(t => t.Id == Id && t.Month == Month);  // && t.Month == [getcurrentmonth]
            return new TimeSheet
            {
                SheetID = entity.SheetID,
                Id = entity.Id,
                Project = entity.Project,
                Month = entity.Month,
                ProjectHours = entity.ProjectHours,
                Overwork = entity.Overwork,
                Sick = entity.Sick,
                Absence = entity.Absence,
                Training = entity.Training,
                Other = entity.Other,
                Submitted = entity.Submitted,
                Approved = entity.Approved,
                Data = entity.Data,
                applicationUser = entity.applicationUser
            };
        }
    }
}
