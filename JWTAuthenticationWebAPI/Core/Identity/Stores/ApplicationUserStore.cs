﻿using JWTAuthenticationWebAPI.Core.DbContext;
using JWTAuthenticationWebAPI.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace JWTAuthenticationWebAPI.Core.Identity.Stores
{
    public class ApplicationUserStore(ApplicationDbContext context, IdentityErrorDescriber? describer = null)
    : UserStore<
        ApplicationUser,
        ApplicationRole,
        ApplicationDbContext,
        Guid,
        IdentityUserClaim<Guid>,
        IdentityUserRole<Guid>,
        IdentityUserLogin<Guid>,
        IdentityUserToken<Guid>,
        IdentityRoleClaim<Guid>
        >(context, describer)
    {
    }
}
