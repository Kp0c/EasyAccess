using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using WebAPI.Entities;
using WebAPI.Extensions;
using WebAPI.Helpers;
using WebAPI.Resources;
using WebAPI.Services.Interfaces;

namespace WebAPI.Services
{
    public class UserService : IUserService
    {
        DataContext _context;
        IHttpContextAccessor _httpContextAccessor;

        public UserService(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public bool AuthenticateByEmail(string username, string authName)
        {
            var user = _context.Users.SingleOrDefault(x => x.Username == username);

            if(user == null)
            {
                return false;
            }

            var emailApp = user.Applications.FirstOrDefault(app => app.ApplicationType == ApplicationType.Email);

            if(emailApp == null)
            {
                return false;
            }

            return emailApp.Authenticate(_context, _httpContextAccessor, authName);
        }

        public UserToRegister Register(UserToRegister user)
        {
            if(String.IsNullOrWhiteSpace(user.Username))
            {
                throw new AppException(MessagesManager.CannotBeEmpty(nameof(user.Username)));
            }

            if (String.IsNullOrWhiteSpace(user.Email))
            {
                throw new AppException(MessagesManager.CannotBeEmpty(nameof(user.Email)));
            }

            if (!user.Email.IsValidEmail())
            {
                throw new AppException(MessagesManager.InvalidEmail);
            }

            if (_context.Users.Any(x => x.Username == user.Username))
            {
                throw new AppException(MessagesManager.UsernameOrEmailIsAlreadyTaken(user.Username));
            }

            if (_context.Users.Any(x => x.Applications.Where(app => app.ApplicationType == ApplicationType.Email)
                                                      .Any(app => app.Action == user.Email)))
            {
                throw new AppException(MessagesManager.UsernameOrEmailIsAlreadyTaken(user.Email));
            }

            user.Id = Guid.NewGuid().ToString();

            _context.UsersToCompleteRegistration.Add(user);
            _context.SaveChanges();

            EmailHelper.SendRegistrationEmail(user, _httpContextAccessor.HttpContext?.Request?.GetDisplayUrl());

            return user;
        }

        public void Delete(string id)
        {
            var user = _context.Users.Find(id);
            if(user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        public User GetById(string id)
        {
            return _context.Users.Find(id);
        }

        public User GetByUsername(string username)
        {
            return _context.Users.FirstOrDefault(user => user.Username == username);
        }

        /*public IEnumerable<User> GetUsers()
        {
            return _context.Users;
        }*/

        public void Update(UserToRegister userParam, string password = null)
        {
            var user = _context.Users.Find(userParam.Id);

            if(user == null)
            {
                throw new AppException(MessagesManager.UserNotFound);
            }

            if(userParam.Username != user.Username)
            {
                if(_context.Users.Any(x => x.Username == userParam.Username))
                {
                    throw new AppException(MessagesManager.UsernameOrEmailIsAlreadyTaken(userParam.Username));
                }
            }

            user.Username = userParam.Username;

            if(!String.IsNullOrWhiteSpace(password))
            {
                // CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

                // user.PasswordHash = passwordHash;
                // user.PasswordSalt = passwordSalt;
            }

            _context.Users.Update(user);
            _context.SaveChanges();
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (String.IsNullOrEmpty(password)) throw new ArgumentException(MessagesManager.CannotBeEmptyOrWhitespaceOnly, nameof(password));
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", nameof(password));
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", nameof(password));

            using (var hmac = new HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if(computedHash[i] != storedHash[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public User CompleteRegistration(string id)
        {
            var userToRegister = _context.UsersToCompleteRegistration.Find(id);

            if(userToRegister == null)
            {
                throw new AppException("Invalid id");
            }

            _context.UsersToCompleteRegistration.Remove(userToRegister);

            User user = new User(userToRegister);

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }
    }
}
