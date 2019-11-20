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
            var traineesAndEmployees = employeeslist.Concat(traineeslist)
                .ToList();

            return context.TimeSheets
                .Select(t => new TimeSheet { Id = t.Id, SheetID = t.SheetID, Project = t.Project, Month = t.Month, ProjectHours = t.ProjectHours, Overwork = t.Overwork, Sick = t.Sick, Absence = t.Absence, Approved = t.Approved, Other = t.Other, Submitted = t.Submitted, Training = t.Training, Data = t.Data })
                //.Where(t => t.Id.Equals(traineesAndEmployees[1].Id))
                //.Where(t => traineesAndEmployees.Contains(t.applicationUser))
                .Where(t => t.Id.Equals(traineesAndEmployees[1].Id))
                .ToList();
        }

        public TimeSheet GetOneTimeSheet(int SheetID, string UserId)
        {
            var entity = context.TimeSheets.Single(t => t.SheetID == SheetID && t.Id == UserId && t.Month == "januari");
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
                Data = entity.Data
            };
        }
    }
}
