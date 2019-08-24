using System;
using System.Collections.Generic;
using System.Text;

namespace EAMobile.PlatrformDependentInterfaces
{
    public interface IFirebaseToken
    {
        string Token { get; }
        string DeviceId { get; }
    }
}
