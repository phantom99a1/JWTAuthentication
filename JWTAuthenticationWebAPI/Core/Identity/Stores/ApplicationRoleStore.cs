using JWTAuthenticationWebAPI.Core.DbContext;
using JWTAuthenticationWebAPI.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace JWTAuthenticationWebAPI.Core.Identity.Stores
{
    public class ApplicationRoleStore(ApplicationDbContext context, IdentityErrorDescriber? describer = null)
    : RoleStore<ApplicationRole, ApplicationDbContext, Guid>(context, describer)
    {
    }
}
