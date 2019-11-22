using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QienUrenMachien.Data;
using QienUrenMachien.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
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


            //using (var message = new MailMessage())
            //{
            //    message.To.Add(new MailAddress("j.m.r.kramer@gmail.com", "To Name"));
            //    message.From = new MailAddress("info@qienurenmachien.nl", "Qien Uren Machien");
            //    message.CC.Add(new MailAddress("cc@email.com", "CC Name"));
            //    message.Bcc.Add(new MailAddress("bcc@email.com", "BCC Name"));
            //    message.Subject = "Lever je timesheet in lul";
            //    message.Body = "https://localhost:44398/sheet/confirmsheet/" + _timeSheet.Url;
            //    message.IsBodyHtml = true;

            //    using (var client = new SmtpClient("smtp.gmail.com"))
            //    {
            //        client.Port = 587;
            //        client.Credentials = new NetworkCredential("qienurenmachien@gmail.com", "Test1234!");
            //        client.EnableSsl = true;
            //        client.Send(message);
            //    }



            //}
            
            return _timeSheet;
        }

        public async Task<TimeSheet> GetTimeSheet(int id)
        {
            return await context.TimeSheets.FindAsync(id);
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

        public TimeSheet GetOneTimeSheet(string url)
        {
            Console.WriteLine("######################");
            Console.WriteLine(url);
            return context.TimeSheets.Where(c => c.Url == url).SingleOrDefault();
        }
    }
}
