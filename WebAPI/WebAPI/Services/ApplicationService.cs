using System;
using System.Collections.Generic;
using System.Linq;
using WebAPI.Entities;
using WebAPI.Extensions;
using WebAPI.Helpers;
using WebAPI.Resources;
using WebAPI.Services.Interfaces;

namespace WebAPI.Services
{
    public class ApplicationService : IApplicationService
    {
        readonly IUserService _userService;
        DataContext _context;

        public ApplicationService(IUserService userService, DataContext context)
        {
            _userService = userService;
            _context = context;
        }

        public Application AddApplication(string userId, Application application)
        {
            var user = GetUser(userId);

            user.Applications.Add(application);

            _userService.Update(user);
     
            return application;
        }

        public void Authenticate(string userId, string id)
        {
            return;
        }

        public void Delete(string userId, string id)
        {
            var user = GetUser(userId);

            user.Applications.RemoveWhere(app => app.Id == id);

            _userService.Update(user);
        }

        public IEnumerable<Application> GetApplications(string userId)
        {
            return GetUser(userId).Applications;
        }

        public Application GetById(string userId, string id)
        {
            return GetUser(userId).Applications.FirstOrDefault(app => app.Id == id);
        }

        public void Update(string userId, Application applicationParam)
        {
            var user = GetUser(userId);

            var application = user.Applications.FirstOrDefault(app => app.Id == applicationParam.Id);

            if(application == null)
            {
                throw new AppException(MessagesManager.ApplicationNotFound);
            }

            application.Name = applicationParam.Name;
            application.ApplicationType = applicationParam.ApplicationType;

            _userService.Update(user);
        }

        private User GetUser(string userId)
        {
            var user = _userService.GetById(userId);

            if (user == null)
            {
                throw new AppException(MessagesManager.UserNotFound);
            }

            return user;
        }
    }
}
