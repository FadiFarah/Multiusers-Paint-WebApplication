using GroupPaintOnlineWebApp.Server.Models;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GroupPaintOnlineWebApp.Shared;

namespace GroupPaintOnlineWebApp.Server.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }
        public DbSet<GroupPaintOnlineWebApp.Shared.Room> Room { get; set; }
        public DbSet<GroupPaintOnlineWebApp.Shared.Painting> Painting { get; set; }
    }
}
