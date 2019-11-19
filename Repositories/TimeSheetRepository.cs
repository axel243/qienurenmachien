using QienUrenMachien.Data;
using QienUrenMachien.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QienUrenMachien.Repositories
{
    public class TimeSheetRepository
    {
        private RepositoryContext context = new RepositoryContext();
        public List<TimeSheet> GetUserTimeSheets(string UserId)
        {
            return context.TimeSheets
                .Select(u => new TimeSheet { UserId = u.UserId, SheetID = u.SheetID, applicationUser = u.applicationUser })
                .Where(u => u.UserId == UserId)
                .ToList();
        }

        public TimeSheet GetOneTimeSheet(int SheetID, string UserId)
        {
            var entity = context.TimeSheets.Single(t => t.SheetID == SheetID && t.UserId == UserId);
            return new TimeSheet
            {
                SheetID = entity.SheetID,
                UserId = entity.UserId,
                Absence = entity.Absence
                //and so on
            };
        }
    }
}
