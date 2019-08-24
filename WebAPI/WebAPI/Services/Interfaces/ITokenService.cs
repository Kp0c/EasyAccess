using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Entities;

namespace WebAPI.Services.Interfaces
{
    public interface ITokenService
    {
        void UpsertNewToken(FirebaseToken firebaseToken);
    }
}
