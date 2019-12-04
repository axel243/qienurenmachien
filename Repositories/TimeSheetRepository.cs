using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
            _timeSheet.Month = DateTime.Now.ToString("MMMM");
            _timeSheet.ProjectHours = 0;
            _timeSheet.Overwork = 0;
            _timeSheet.Sick = 0;
            _timeSheet.Training = 0;
            _timeSheet.Other = 0;
            _timeSheet.Project = "Macaw";
            _timeSheet.Submitted = false;
            _timeSheet.Approved = "Not submitted";
            _timeSheet.Data = data;
            _timeSheet.Url = Guid.NewGuid().ToString();
            _timeSheet.Comment = "";
            _timeSheet.theDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);


            var result = context.Add(_timeSheet);
            context.SaveChanges();


            return _timeSheet;
        }

        public async Task<string> TimeSheetData(){
            var result = await context.TimeSheets.OrderBy(c => c.theDate).ToListAsync();
            var dictionary = new Dictionary<String, double>();

            foreach (TimeSheet _timeSheet in result)
            {
                if (dictionary.ContainsKey(_timeSheet.theDate.ToString("MMMM"))) {
                    dictionary[_timeSheet.theDate.ToString("MMMM")] += _timeSheet.ProjectHours;
                }
                else {
                    dictionary[_timeSheet.theDate.ToString("MMMM")] = _timeSheet.ProjectHours;
                }
                
            }
            //Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(dictionary));

            List<string> x = new List<string>();
            List<Double> y = new List<Double>();
            List<Object> z = new List<Object>();


            // Display the keys.
            foreach (string date in dictionary.Keys) {
                x.Add(date);
                y.Add(dictionary[date]);
            }

            var DataSet = new {
                label = "Project uren",
                data = y
            };

            z.Add(DataSet);


            var myAnonymousType = new {
                labels = x, 
                data = z
            };

            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(myAnonymousType));
           // Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(y));

            // Convert the Dictionary's ValueCollection
            // into an array and display the values.
            // string[] values = Numbers.Values.ToArray();
            // for (int i = 0; i < values.Length; i++)
            //     lstValues.Items.Add(values[i]);

            return Newtonsoft.Json.JsonConvert.SerializeObject(myAnonymousType);
        }

        public TimeSheet AddTimeSheetTemp()
        {
            

            for (int i = 7; i < 12; i++)
            {
                DateTime dt = new DateTime(DateTime.Now.Year, i, 1);

                TimeSheet _timeSheet = new TimeSheet();
                _timeSheet.Id = "de42b22a-18b0-4c95-b344-0b92dc83ca7b";
                _timeSheet.Month = dt.ToString("MMMM");
                _timeSheet.ProjectHours = 176;
                _timeSheet.Overwork = 0;
                _timeSheet.Sick = 0;
                _timeSheet.Training = 0;
                _timeSheet.Other = 0;
                _timeSheet.Project = "Macaw";
                _timeSheet.Submitted = true;
                _timeSheet.Approved = "Approved";
                _timeSheet.Data = "{\"1\":{\"projecthours\":0,\"overwork\":0,\"sick\":0,\"absence\":0,\"training\":0,\"other\":0},\"2\":{\"projecthours\":\"8\",\"overwork\":0,\"sick\":0,\"absence\":0,\"training\":0,\"other\":0},\"3\":{\"projecthours\":\"8\",\"overwork\":0,\"sick\":0,\"absence\":0,\"training\":0,\"other\":0},\"4\":{\"projecthours\":\"8\",\"overwork\":0,\"sick\":0,\"absence\":0,\"training\":0,\"other\":0},\"5\":{\"projecthours\":\"8\",\"overwork\":0,\"sick\":0,\"absence\":0,\"training\":0,\"other\":0},\"6\":{\"projecthours\":\"8\",\"overwork\":0,\"sick\":0,\"absence\":0,\"training\":0,\"other\":0},\"7\":{\"projecthours\":0,\"overwork\":0,\"sick\":0,\"absence\":0,\"training\":0,\"other\":0},\"8\":{\"projecthours\":0,\"overwork\":0,\"sick\":0,\"absence\":0,\"training\":0,\"other\":0},\"9\":{\"projecthours\":\"8\",\"overwork\":0,\"sick\":0,\"absence\":0,\"training\":0,\"other\":0},\"10\":{\"projecthours\":\"8\",\"overwork\":0,\"sick\":0,\"absence\":0,\"training\":0,\"other\":0},\"11\":{\"projecthours\":\"8\",\"overwork\":0,\"sick\":0,\"absence\":0,\"training\":0,\"other\":0},\"12\":{\"projecthours\":\"8\",\"overwork\":0,\"sick\":0,\"absence\":0,\"training\":0,\"other\":0},\"13\":{\"projecthours\":\"8\",\"overwork\":0,\"sick\":0,\"absence\":0,\"training\":0,\"other\":0},\"14\":{\"projecthours\":0,\"overwork\":0,\"sick\":0,\"absence\":0,\"training\":0,\"other\":0},\"15\":{\"projecthours\":0,\"overwork\":0,\"sick\":0,\"absence\":0,\"training\":0,\"other\":0},\"16\":{\"projecthours\":\"8\",\"overwork\":0,\"sick\":0,\"absence\":0,\"training\":0,\"other\":0},\"17\":{\"projecthours\":\"8\",\"overwork\":0,\"sick\":0,\"absence\":0,\"training\":0,\"other\":0},\"18\":{\"projecthours\":\"8\",\"overwork\":0,\"sick\":0,\"absence\":0,\"training\":0,\"other\":0},\"19\":{\"projecthours\":\"8\",\"overwork\":0,\"sick\":0,\"absence\":0,\"training\":0,\"other\":0},\"20\":{\"projecthours\":\"8\",\"overwork\":0,\"sick\":0,\"absence\":0,\"training\":0,\"other\":0},\"21\":{\"projecthours\":0,\"overwork\":0,\"sick\":0,\"absence\":0,\"training\":0,\"other\":0},\"22\":{\"projecthours\":0,\"overwork\":0,\"sick\":0,\"absence\":0,\"training\":0,\"other\":0},\"23\":{\"projecthours\":\"8\",\"overwork\":0,\"sick\":0,\"absence\":0,\"training\":0,\"other\":0},\"24\":{\"projecthours\":\"8\",\"overwork\":0,\"sick\":0,\"absence\":0,\"training\":0,\"other\":0},\"25\":{\"projecthours\":\"8\",\"overwork\":0,\"sick\":0,\"absence\":0,\"training\":0,\"other\":0},\"26\":{\"projecthours\":\"8\",\"overwork\":0,\"sick\":0,\"absence\":0,\"training\":0,\"other\":0},\"27\":{\"projecthours\":\"8\",\"overwork\":0,\"sick\":0,\"absence\":0,\"training\":0,\"other\":0},\"28\":{\"projecthours\":0,\"overwork\":0,\"sick\":0,\"absence\":0,\"training\":0,\"other\":0},\"29\":{\"projecthours\":0,\"overwork\":0,\"sick\":0,\"absence\":0,\"training\":0,\"other\":0},\"30\":{\"projecthours\":\"8\",\"overwork\":0,\"sick\":0,\"absence\":0,\"training\":0,\"other\":0},\"31\":{\"projecthours\":\"8\",\"overwork\":0,\"sick\":0,\"absence\":0,\"training\":0,\"other\":0}}";
                _timeSheet.Url = Guid.NewGuid().ToString();
                _timeSheet.Comment = "";
                _timeSheet.theDate = dt;


                var result = context.Add(_timeSheet);
            }

        


            context.SaveChanges();


            return  new TimeSheet();
        }

        public async Task<TimeSheet> GetTimeSheet(int id)
        {
            return await context.TimeSheets.FindAsync(id);
        }

        public async Task<TimeSheet> GetTimeSheetUrl(string url)
        {
            return await context.TimeSheets.Where(t => t.Url == url).FirstOrDefaultAsync<TimeSheet>();
        }

        public async Task<TimeSheet> GetTimeSheet(string id)
        {
            return await context.TimeSheets.Where(c => c.Id == id && c.Month == "January").SingleOrDefaultAsync();
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
                list.Add(new SelectListItem { Value = month, Text = month });
            }
            return list;
        }

        public List<SelectListItem> GetYears()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString(), Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy"), Selected = true });
            DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            for (int i = 2018; i < DateTime.Now.Year; i++)
            {
                dt = dt.AddYears(-1);
                var year = dt.ToString();
                list.Add(new SelectListItem { Value = year, Text = dt.ToString("yyyy") });
            }
            return list;
        }

        public async Task<List<TimeSheet>> GetAllTraineeTimeSheets(TimeSheetsViewModel model)
        {
            var traineeslist = await userManager.GetUsersInRoleAsync("Trainee");
            var traineesIds = traineeslist
                .Select(e => e.Id);

            return await context.TimeSheets
                .Select(t => new TimeSheet { Id = t.Id, SheetID = t.SheetID, Project = t.Project, Month = t.Month, theDate = t.theDate, ProjectHours = t.ProjectHours, Overwork = t.Overwork, Sick = t.Sick, Absence = t.Absence, Approved = t.Approved, Other = t.Other, Submitted = t.Submitted, Training = t.Training, Data = t.Data, applicationUser = t.applicationUser })
                .Where(t => traineesIds.Contains(t.Id) && t.Month.Equals(model.Month) && t.theDate.Year == model.theDate.Year)
                .ToListAsync();
        }

        public async Task<List<TimeSheet>> GetAllEmployeeTimeSheets(TimeSheetsViewModel model)
        {
            var employeeslist = await userManager.GetUsersInRoleAsync("Werknemer");
            var employeesIds = employeeslist
                .Select(e => e.Id);

            return await context.TimeSheets
                .Select(t => new TimeSheet { Id = t.Id, SheetID = t.SheetID, Project = t.Project, Month = t.Month, theDate = t.theDate, ProjectHours = t.ProjectHours, Overwork = t.Overwork, Sick = t.Sick, Absence = t.Absence, Approved = t.Approved, Other = t.Other, Submitted = t.Submitted, Training = t.Training, Data = t.Data, applicationUser = t.applicationUser })
                .Where(t => employeesIds.Contains(t.Id) && t.Month.Equals(model.Month) && t.theDate.Year == model.theDate.Year)
                .ToListAsync();
        }

        //public TimeSheetViewModel GetOneTimeSheet(string Id, string Month)
        //{

        //    var entity = context.TimeSheets.Single(t => t.Id == Id && t.Month == Month);
       

        //    return new TimeSheetViewModel
        //    {
        //        SheetID = entity.SheetID,
        //        Id = entity.Id,
        //        Project = entity.Project,
        //        Month = entity.Month,
        //        ProjectHours = entity.ProjectHours,
        //        Overwork = entity.Overwork,
        //        Sick = entity.Sick,
        //        Absence = entity.Absence,
        //        Training = entity.Training,
        //        Other = entity.Other,
        //        Submitted = entity.Submitted,
        //        Approved = entity.Approved,
        //        Data = entity.Data,
        //        Url = entity.Url
        //    };

        //}

        public async Task<TimeSheetViewModel> GetOneTimeSheetAsync(string Id, int SheetID)
        {

            var entity = await context.TimeSheets.Where(t => t.Id == Id && t.SheetID == SheetID).FirstOrDefaultAsync<TimeSheet>();


            return new TimeSheetViewModel
            {
                SheetID = entity.SheetID,
                Id = entity.Id,
                Project = entity.Project,
                Month = entity.Month,
                theDate = entity.theDate,
                ProjectHours = entity.ProjectHours,
                Overwork = entity.Overwork,
                Sick = entity.Sick,
                Absence = entity.Absence,
                Training = entity.Training,
                Other = entity.Other,
                Submitted = entity.Submitted,
                Data = entity.Data,
                Url = entity.Url,
            };

        }

        public async Task<TimeSheetViewModel> GetOneTimeSheetByUrl(string url)
        {

            var entity = await context.TimeSheets.Where(t => t.Url == url).FirstOrDefaultAsync<TimeSheet>();

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
                Data = entity.Data,
                Url = entity.Url,
                Comment = entity.Comment
            };

        }



        public TimeSheet GetOneTimeSheet(string url)
        {
            Console.WriteLine("######################");
            Console.WriteLine(url);
            return context.TimeSheets.Where(c => c.Url == url).SingleOrDefault();
        }


        public List<TimeSheet> GetTimeSheets()
        {
            var sheetList = context.TimeSheets.Select(n => new TimeSheet
            {
                SheetID = n.SheetID,
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

        public void AddNewSheet(TimeSheet timeSheetModel)
        {
            context.TimeSheets.Add(new TimeSheet
            {
                Month = timeSheetModel.Month,
                ProjectHours = timeSheetModel.ProjectHours,
                Overwork = timeSheetModel.Overwork,
                Sick = timeSheetModel.Sick,
                Absence = timeSheetModel.Absence,
                Training = timeSheetModel.Training,
                Other = timeSheetModel.Other,
                Data = timeSheetModel.Data,
                Comment = timeSheetModel.Comment

            });
            context.SaveChanges();
        }

        public async Task<List<TimeSheet>> GetUserOverview(string id)
        {
            var result = await context.TimeSheets.Where(c => c.Id == id).OrderByDescending(c => c.theDate).ToListAsync();
            bool current_month = false;

            foreach (TimeSheet _timeSheet in result)
            {
                Console.WriteLine(_timeSheet.theDate);
                DateTime dt = DateTime.Now;

                if (_timeSheet.theDate.Year == dt.Year && _timeSheet.theDate.Month == dt.Month)
                {
                    current_month = true;
                }
            }

            if (result.Count == 0 || current_month == false)
            {
                DateTime dt = DateTime.Now;

                int nDays = DateTime.DaysInMonth(2019, dt.Month);
                string data = "{";

                for (int i = 1; i <= nDays; i++)
                {
                    DayJulian _day = new DayJulian();
                    data += $"\"{i}\": " + System.Text.Json.JsonSerializer.Serialize<DayJulian>(_day);
                    if (i != nDays)
                    {
                        data += ", ";
                    }
                }
                data += "}";

                TimeSheet entity2 = AddTimeSheet(id, data);
                result = await context.TimeSheets.Where(c => c.Id == id).OrderByDescending(theDate => theDate).ToListAsync();
            }

            return result;
        }
    }
}
