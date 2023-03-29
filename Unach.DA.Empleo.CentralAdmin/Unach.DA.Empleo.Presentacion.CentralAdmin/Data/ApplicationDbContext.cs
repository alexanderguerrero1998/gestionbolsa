using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Unach.DA.Empleo.Persistencia.Core.Models;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<SicoaContext> options)
            : base(options)
        {
        }
    }
}