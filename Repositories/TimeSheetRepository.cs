using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QienUrenMachien.Data;
using QienUrenMachien.Entities;
using QienUrenMachien.Models;
using QienUrenMachien.Translation;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            var dictionary = new Dictionary<String, DataModel>();

            foreach (TimeSheet _timeSheet in result)
            {
                if (dictionary.ContainsKey(_timeSheet.theDate.ToString("MMMM"))) {
                    dictionary[_timeSheet.theDate.ToString("MMMM")].ProjectHours += _timeSheet.ProjectHours;
                    dictionary[_timeSheet.theDate.ToString("MMMM")].Overwork += _timeSheet.Overwork;
                    dictionary[_timeSheet.theDate.ToString("MMMM")].Sick += _timeSheet.Sick;
                    dictionary[_timeSheet.theDate.ToString("MMMM")].Absence += _timeSheet.Absence;
                    dictionary[_timeSheet.theDate.ToString("MMMM")].Training += _timeSheet.Training;
                    dictionary[_timeSheet.theDate.ToString("MMMM")].Other += _timeSheet.Other;
                }
                else {
                    dictionary[_timeSheet.theDate.ToString("MMMM")] = new DataModel {
                        ProjectHours = _timeSheet.ProjectHours,
                        Overwork = _timeSheet.Overwork,
                        Sick = _timeSheet.Sick,
                        Absence = _timeSheet.Absence,
                        Training = _timeSheet.Training,
                        Other = _timeSheet.Other

                    } ;
                }
                
            }
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(dictionary));

            List<string> x = new List<string>();
            List<Double> ProjectHours = new List<Double>();
            List<Double> Overwork = new List<Double>();
            List<Double> Sick = new List<Double>();
            List<Double> Absence = new List<Double>();
            List<Double> Training = new List<Double>();
            List<Double> Other = new List<Double>();

            List<Object> z = new List<Object>();


            // Display the keys.
            foreach (string date in dictionary.Keys) {
                x.Add(date);
                ProjectHours.Add(dictionary[date].ProjectHours);
                Overwork.Add(dictionary[date].Overwork);
                Sick.Add(dictionary[date].Sick);
                Absence.Add(dictionary[date].Absence);
                Training.Add(dictionary[date].Training);
                Other.Add(dictionary[date].Other);
            }

            var DataSet = new {
                label = "Project",
                data = ProjectHours,
                borderColor = "#3e95cd",
                fill = false,
                backgroundColor = "#3e95cd"
            };

            var DataSet2 = new {
                label = "Overwerk",
                data = Overwork,
                borderColor = "#8e5ea2",
                fill = false,
                backgroundColor = "#8e5ea2"
            };

            var DataSet3 = new {
                label = "Ziek",
                data = Sick,
                borderColor = "#3cba9f",
                fill = false,
                backgroundColor = "#3cba9f"
            };

            var DataSet4 = new {
                label = "Afwezig",
                data = Absence, 
                borderColor = "#e8c3b9",
                fill = false,
                backgroundColor = "#e8c3b9"
            };

            var DataSet5 = new {
                label = "Training",
                data = Training,
                borderColor = "#c45850",
                fill = false,
                backgroundColor = "#c45850"
            };

            var DataSet6 = new {
                label = "Overig",
                data = Other,
                borderColor = "lightgray",
                fill = false,
                backgroundColor = "lightgray"
            };

            z.Add(DataSet);
            z.Add(DataSet2);
            z.Add(DataSet3);
            z.Add(DataSet4);
            z.Add(DataSet5);
            z.Add(DataSet6);


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

        public async Task<string> TimeSheetDataCSV(){
            var result = await context.TimeSheets.OrderBy(c => c.theDate).ToListAsync();
            var dictionary = new Dictionary<String, DataModel>();

            foreach (TimeSheet _timeSheet in result)
            {
                if (dictionary.ContainsKey(_timeSheet.theDate.ToString("MMMM"))) {
                    dictionary[_timeSheet.theDate.ToString("MMMM")].ProjectHours += _timeSheet.ProjectHours;
                    dictionary[_timeSheet.theDate.ToString("MMMM")].Overwork += _timeSheet.Overwork;
                    dictionary[_timeSheet.theDate.ToString("MMMM")].Sick += _timeSheet.Sick;
                    dictionary[_timeSheet.theDate.ToString("MMMM")].Absence += _timeSheet.Absence;
                    dictionary[_timeSheet.theDate.ToString("MMMM")].Training += _timeSheet.Training;
                    dictionary[_timeSheet.theDate.ToString("MMMM")].Other += _timeSheet.Other;
                }
                else {
                    dictionary[_timeSheet.theDate.ToString("MMMM")] = new DataModel {
                        ProjectHours = _timeSheet.ProjectHours,
                        Overwork = _timeSheet.Overwork,
                        Sick = _timeSheet.Sick,
                        Absence = _timeSheet.Absence,
                        Training = _timeSheet.Training,
                        Other = _timeSheet.Other

                    } ;
                }
                
            }
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(dictionary));

            string csv = "Month, Project, Overwerk, Ziek, Afwezig, Training, Overig\n";

            foreach(KeyValuePair<string, DataModel> entry in dictionary)
            {
                csv += $"{entry.Key}, {entry.Value.ProjectHours}, {entry.Value.Overwork}, {entry.Value.Sick}, {entry.Value.Absence}, {entry.Value.Training}, {entry.Value.Other}\n";
            }

            return csv;
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
            list.Add(new SelectListItem { Value = DateTime.Now.ToString("MMMM"), Text = Translator.TranslateMonth(DateTime.Now.ToString("MMMM")), Selected = true });
            DateTime dt = DateTime.Now;
            for (int i = 1; i < DateTime.Now.Month; i++)
            {
                dt = dt.AddMonths(-1);
                var month = dt.ToString("MMMM");
                list.Add(new SelectListItem { Value = month, Text = Translator.TranslateMonth(month) });
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
                .Select(t => new TimeSheet { Id = t.Id, SheetID = t.SheetID, Project = t.Project, Month = t.Month, theDate = t.theDate, ProjectHours = t.ProjectHours, Overwork = t.Overwork, Sick = t.Sick, Absence = t.Absence, Approved = t.Approved, Other = t.Other, Submitted = t.Submitted, Comment = t.Comment, Training = t.Training, Url = t.Url, Data = t.Data, applicationUser = t.applicationUser })
                .Where(t => traineesIds.Contains(t.Id) && t.Month.Equals(model.Month) && t.theDate.Year == model.theDate.Year)
                .ToListAsync();
        }

        public async Task<List<TimeSheet>> GetAllEmployeeTimeSheets(TimeSheetsViewModel model)
        {
            var employeeslist = await userManager.GetUsersInRoleAsync("Werknemer");
            var employeesIds = employeeslist
                .Select(e => e.Id);

            return await context.TimeSheets
                .Select(t => new TimeSheet { Id = t.Id, SheetID = t.SheetID, Project = t.Project, Month = t.Month, theDate = t.theDate, ProjectHours = t.ProjectHours, Overwork = t.Overwork, Sick = t.Sick, Absence = t.Absence, Approved = t.Approved, Other = t.Other, Submitted = t.Submitted, Comment = t.Comment, Training = t.Training, Url = t.Url, Data = t.Data, applicationUser = t.applicationUser })
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
                Approved = entity.Approved,
                Comment = entity.Comment
                
            };

        }

        public async Task<List<TimeSheetWithUser>> GetTimeSheetAndUser()
        {
            DateTime _dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var timesheets = await context.TimeSheets.Include(ts => ts.applicationUser).Where(t =>  t.theDate < _dt && t.Approved == "Not submitted" )
                .Select(ts => new TimeSheetWithUser { FirstName = ts.applicationUser.Firstname, LastName = ts.applicationUser.Lastname, Status = ts.Approved, WerkgeverId = ts.applicationUser.WerkgeverID, url = ts.Url })
                .ToListAsync();

            var timesheetsRejected = await context.TimeSheets.Include(ts => ts.applicationUser).Where(t => t.theDate < _dt && t.Approved == "Rejected")
               .Select(ts => new TimeSheetWithUser { FirstName = ts.applicationUser.Firstname, LastName = ts.applicationUser.Lastname, Status = ts.Approved, WerkgeverId = ts.applicationUser.WerkgeverID, url = ts.Url })
               .ToListAsync();

            timesheets.AddRange(timesheetsRejected);

            return timesheets;

        }

        public async Task<List<TimeSheetWithUser>> GetLastMonthProfileEdits()
        {
            var timesheets = await context.Users.Where(s => s.NewProfile != null).Select(t => new TimeSheetWithUser { Status = t.NewProfile, FirstName = t.Firstname, LastName = t.Lastname, userId = t.Id }).ToListAsync();

            return timesheets;

        }

        public async Task<TimeSheetViewModel> GetOneTimeSheetByUrl(string url)
        {

            var entity = await context.TimeSheets.Where(t => t.Url == url).FirstOrDefaultAsync<TimeSheet>();
            ApplicationUser user = await userManager.FindByIdAsync(entity.Id);

            return new TimeSheetViewModel
            {
                UserName = user.Firstname + " " + user.Lastname,
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
                Comment = entity.Comment,
                theDate = entity.theDate
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

        public async Task<List<TimeSheet>> GetUserOverview(string id, DateTime ActiveFrom, DateTime ActiveUntil)
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

            if ((result.Count == 0 || current_month == false) && DateTime.Now >= ActiveFrom && DateTime.Now <= ActiveUntil)
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
