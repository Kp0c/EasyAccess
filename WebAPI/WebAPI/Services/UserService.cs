using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using WebAPI.Entities;
using WebAPI.Helpers;
using WebAPI.Resources;
using WebAPI.Services.Interfaces;

namespace WebAPI.Services
{
    public class UserService : IUserService
    {
        DataContext _context;

        public UserService(DataContext context)
        {
            _context = context;
        }

        public User Authenticate(string username, string password)
        {
            if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            var user = _context.Users.SingleOrDefault(x => x.Username == username);

            if (user != null &&
                !VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                user = null;
            }

            return user;
        }

        public User Create(User user, string password)
        {
            if(String.IsNullOrWhiteSpace(password))
            {
                throw new AppException(MessagesManager.CannotBeEmpty(nameof(password)));
            }

            if(String.IsNullOrWhiteSpace(user.Username))
            {
                throw new AppException(MessagesManager.CannotBeEmpty(nameof(user.Username)));
            }

            if(_context.Users.Any(x => x.Username == user.Username))
            {
                throw new AppException(MessagesManager.UsernameIsAlreadyTaken(user.Username));
            }

            user.Id = Guid.NewGuid().ToString();

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.Users.Add(user);
            _context.SaveChanges();

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

        public IEnumerable<User> GetUsers()
        {
            return _context.Users;
        }

        public void Update(User userParam, string password = null)
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
                    throw new AppException(MessagesManager.UsernameIsAlreadyTaken(userParam.Username));
                }
            }

            user.FirstName = userParam.FirstName;
            user.LastName = userParam.LastName;
            user.Username = userParam.Username;

            if(!String.IsNullOrWhiteSpace(password))
            {
                CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            _context.Users.Update(user);
            _context.SaveChanges();
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (String.IsNullOrWhiteSpace(password)) throw new ArgumentException(MessagesManager.CannotBeEmptyOrWhitespaceOnly, nameof(password));

            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
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
    }
}
