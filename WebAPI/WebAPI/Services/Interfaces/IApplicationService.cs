using System.Collections.Generic;
using WebAPI.Entities;

namespace WebAPI.Services.Interfaces
{
    public interface IApplicationService
    {
        Application AddApplication(string userId, Application application);
        IEnumerable<Application> GetApplications(string userId);
        Application GetById(string userId, string id);
        void Update(string userId, Application application);
        void Delete(string userId, string id);

        void Authenticate(string userId, string id);
    }
}
