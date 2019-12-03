﻿using Microsoft.AspNetCore.Identity;
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
    public class ActivityLogRepository : IActivityLogRepository
    {
        private readonly RepositoryContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public ActivityLogRepository(RepositoryContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public void LogActivity(ApplicationUser user, string activity, string comment)
        {

            ActivityLog log = new ActivityLog();
            log.Id = user.Id;
            log.Activity = activity.ToString();
            log.Comment = comment;
            log.Timestamp = DateTime.Now.ToString("MM'-'dd'-'yyyy HH':'mm':'ss");

            context.ActivityLogs.Add(log);
            context.SaveChanges();
        }
    }
}

