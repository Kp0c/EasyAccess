using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Entities;
using WebAPI.Helpers;

namespace WebAPI.Services.Interfaces
{
    public class TokenService : ITokenService
    {
        DataContext _context;

        public TokenService(DataContext context)
        {
            _context = context;
        }

        public void UpsertNewToken(FirebaseToken firebaseToken)
        {
            var token = _context.FirebaseTokens.Find(firebaseToken.DeviceId);

            if(token != null)
            {
                _context.Remove(token);
            }

            _context.Add(firebaseToken);

            _context.SaveChanges();
        }
    }
}
