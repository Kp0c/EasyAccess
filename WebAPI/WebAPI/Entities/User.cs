using System;
using System.Collections.Generic;

namespace WebAPI.Entities
{
    public class User
    {
        public string Id { get; set; }
        public string Username { get; set; }

        public virtual ICollection<Application> Applications { get; set; }

        public User() { }

        public User(UserToRegister userToRegister)
        {
            Id = userToRegister.Id;
            Username = userToRegister.Username;

            Applications = new List<Application>();

            Applications.Add(new Application
            {
                Id = Guid.NewGuid().ToString(),
                ApplicationType = ApplicationType.Email,
                Name = "Email",
                Action = userToRegister.Email
            });
        }

    }
}
