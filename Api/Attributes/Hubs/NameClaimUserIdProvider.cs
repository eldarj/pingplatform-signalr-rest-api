using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Api.Helpers
{
    public class UserClaimIdentifierProvider : IUserIdProvider
    {
        public virtual string GetUserId(HubConnectionContext connection)
        {
            // Use the user's Name claim value as the UserId ie. UserClaimIdentifier
            return connection.User?.FindFirst(ClaimTypes.Name)?.Value;
        }
    }
}
