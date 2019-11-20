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

        public List<TimeSheet> GetTimeSheets()
        {
            var sheetList = context.TimeSheets.Select(n => new TimeSheet
            {
                SheetID = n.SheetID,
                Project = n.Project,
                Month = n.Month,
                ProjectHours = n.ProjectHours,
                Overwork = n.Overwork,
                Sick = n.Sick,
                Absence = n.Absence,
                Training = n.Training,
                Other = n.Other,
                Data = n.Data
            }).ToList();
            return sheetList;
        }

        public List<Day> GetAllDaysInMonth(int year, int month)
        {
            var days = new List<Day>();
            days.Add(new Day(new DateTime(year, month, 1), "Nos", 4, 4, 2, 5, 7, 0, "None"));
            days.Add(new Day(new DateTime(year, month, 2), "Qien", 4, 4, 2, 5, 7, 0, "Eerder weggegaan"));
            return days;

        }

        public void AddNewSheet(TimeSheet timeSheetModel)
        {
            context.TimeSheets.Add(new TimeSheet
            {
                Project = timeSheetModel.Project,
                Month = timeSheetModel.Month,
                ProjectHours = timeSheetModel.ProjectHours,
                Overwork = timeSheetModel.Overwork,
                Sick = timeSheetModel.Sick,
                Absence = timeSheetModel.Absence,
                Training = timeSheetModel.Training,
                Other = timeSheetModel.Other,
                Data = timeSheetModel.Data

            });
            context.SaveChanges();
        }

        //public List<DateTime> GetAllDaysInMonth(int year, int month)
        //{
        //    var ret = new List<DateTime>();
        //    for (int i = 1; i <= DateTime.DaysInMonth(year, month); i++)
        //    {
        //        ret.Add(new DateTime(year, month, i));
        //    }
        //    return ret;
        //}
    }
}
