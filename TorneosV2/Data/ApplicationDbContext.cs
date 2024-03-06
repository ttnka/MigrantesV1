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

    
    public DbSet<Z300_Nombres> Nombres { get; set; }
    public DbSet<Z302_Contactos> Contactos { get; set; }
    public DbSet<Z304_Domicilio> Domicilios  { get; set; }
    public DbSet<Z309_Pariente> Parientes { get; set; }
    //public DbSet<Z310_Fisico> Complexion  { get; set; }
    public DbSet<Z360_Solicitud> Solicitudes { get; set; }
    public DbSet<Z362_SolDet> SolDetalles { get; set; }
    public DbSet<Z368_Seguimiento> Seguimientos  { get; set; }
    public DbSet<Z380_Servicios> Servicios { get; set; }
    public DbSet<Z390_Pais> Paises { get; set; }
    
    
}

