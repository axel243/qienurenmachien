using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using QienUrenMachien.Data;
using QienUrenMachien.Entities;
using QienUrenMachien.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.Json;
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
        public async Task<TimeSheet> UpdateTimeSheet(TimeSheet _timeSheet)   
        {
            context.TimeSheets.Update(_timeSheet);
            await context.SaveChangesAsync();
            
            return _timeSheet;
        }

        public TimeSheet AddTimeSheet(string userId, string data)
        {
            Console.WriteLine(userId);
            TimeSheet _timeSheet = new TimeSheet();
            _timeSheet.Id = userId;
            _timeSheet.Month = "January";
            _timeSheet.ProjectHours = 0;
            _timeSheet.Overwork = 0;
            _timeSheet.Sick = 0;
            _timeSheet.Training = 0;
            _timeSheet.Other = 0;
            _timeSheet.Project = "Macaw";
            _timeSheet.Submitted = false;
            _timeSheet.Approved = "Not submitted";
            _timeSheet.Data = data;

            var result = context.Add(_timeSheet);
            context.SaveChanges();


            return _timeSheet;
        }

        public async Task<TimeSheet> GetTimeSheet(int id)
        {
            return await context.TimeSheets.FindAsync(id);
        }

        public async Task<TimeSheet> GetTimeSheet(string id)
        {
            return context.TimeSheets.Where(c => c.Id == id && c.Month == "January").SingleOrDefault();
        }

        public List<SelectListItem> GetMonths()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Value = DateTime.Now.ToString("MMMM"), Text = DateTime.Now.ToString("MMMM"), Selected = true });
            DateTime dt = DateTime.Now;
            for (int i = 1; i < DateTime.Now.Month; i++)
            {
                dt = dt.AddMonths(-1);
                var month = dt.ToString("MMMM");
                //var year = dt.Year;
                list.Add(new SelectListItem { Value = month, Text = month });
            }
            return list;
        }

        public async Task<List<TimeSheet>> GetAllTraineeTimeSheets(TimeSheetsViewModel model)
        {
            var traineeslist = await userManager.GetUsersInRoleAsync("Trainee");
            var traineesIds = traineeslist
                .Select(e => e.Id);

            return await context.TimeSheets
                .Select(t => new TimeSheet { Id = t.Id, SheetID = t.SheetID, Project = t.Project, Month = t.Month, ProjectHours = t.ProjectHours, Overwork = t.Overwork, Sick = t.Sick, Absence = t.Absence, Approved = t.Approved, Other = t.Other, Submitted = t.Submitted, Training = t.Training, Data = t.Data, applicationUser = t.applicationUser })
                .Where(t => traineesIds.Contains(t.Id) && t.Month.Equals(model.Month))
                .ToListAsync();
        }

        public async Task<List<TimeSheet>> GetAllEmployeeTimeSheets(TimeSheetsViewModel model)
        {
            var employeeslist = await userManager.GetUsersInRoleAsync("Werknemer");
            var employeesIds = employeeslist
                .Select(e => e.Id);

            return await context.TimeSheets
                .Select(t => new TimeSheet { Id = t.Id, SheetID = t.SheetID, Project = t.Project, Month = t.Month, ProjectHours = t.ProjectHours, Overwork = t.Overwork, Sick = t.Sick, Absence = t.Absence, Approved = t.Approved, Other = t.Other, Submitted = t.Submitted, Training = t.Training, Data = t.Data, applicationUser = t.applicationUser })
                .Where(t => employeesIds.Contains(t.Id) && t.Month.Equals(model.Month))
                .ToListAsync();
        }

        public TimeSheetViewModel GetOneTimeSheet(int SheetID)
        {
            var entity = context.TimeSheets.Single(t => SheetID == t.SheetID);

            return new TimeSheetViewModel
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

        public TimeSheet GetOneTimeSheet(string url)
        {
            Console.WriteLine("######################");
            Console.WriteLine(url);
            return context.TimeSheets.Where(c => c.Url == url).SingleOrDefault();
        }
    }
}
