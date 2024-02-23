using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TorneosV2.Modelos;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace TorneosV2.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
    
    public DbSet<ZConfig> Configuraciones { get; set; }
    public DbSet<Z100_Org> Organizaciones { get; set; }
    public DbSet<Z102_Grupo> Grupos { get; set; }
    public DbSet<Z104_GpoDet> GruposDet { get; set; }
    public DbSet<Z110_User> Usuarios { get; set; }
    public DbSet<Z170_Files> Archivos { get; set; }
    public DbSet<Z172_Registros> Registros { get; set; }
    public DbSet<Z190_Bitacora> Bitacora { get; set; }
    public DbSet<Z192_Logs> LogsBitacora { get; set; }
    //public DbSet<Z200_Folio> Folios { get; set; }
    //public DbSet<Z201_FolioPrint> FolioPrints { get; set; }

    
}

