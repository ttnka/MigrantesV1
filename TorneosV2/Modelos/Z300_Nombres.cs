        using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TorneosV2.Modelos
{
	public class Z300_Nombres
	{
        [Key]
        [StringLength(50)]
        public string NombreId { get; set; } = Guid.NewGuid().ToString();
        [StringLength(20)]
        public string? Apodo { get; set; } = "";
        [StringLength(25)]
        public string Nombre { get; set; } = "";
        [StringLength(25)]
        public string Paterno { get; set; } = "";
        [StringLength(25)]
        public string? Materno { get; set; } = "";
        [StringLength(1)]
        public string Sexo { get; set; } = "";
        public DateTime Nacimiento { get; set; } = DateTime.Today;
        [StringLength(50)]
        public string Nacionalidad { get; set; } = "";

        public int Estado { get; set; } = 2;
        public bool Status { get; set; } = true;
        
        public string Completo => Nombre + " " + Paterno + " " + Materno;
        public string PMN => Paterno + " " + Materno + " " + Nombre + " " + Apodo;

        public virtual ICollection<Z302_Contactos> Contactos { get; set; } = new List<Z302_Contactos>();

        public void ConcactoAdd(Z302_Contactos contacto)
        {
            Contactos.Add(contacto);
            contacto.Nombre = this;
        }

        public virtual ICollection<Z304_Domicilio> Domicilios { get; set; } = new List<Z304_Domicilio>();
        public void DomiciliosAdd(Z304_Domicilio domicilio)
        {
            Domicilios.Add(domicilio);
            domicilio.Nombre = this;
        }

        public virtual ICollection<Z309_Pariente> Parientes { get; set; } = new List<Z309_Pariente>();
        public void ParientesAdd(Z309_Pariente pariente)
        {
            Parientes.Add(pariente);
            pariente.Nombre = this;
        }

        public virtual ICollection<Z362_SolDet> Solicitudes { get; set; } = new List<Z362_SolDet>();
        public void SolicitudesAdd(Z362_SolDet solicitud)
        {
            Solicitudes.Add(solicitud);
            solicitud.Nombre = this;
        }
        public virtual ICollection<Z368_Seguimiento> Seguimientos { get; set; } = new List<Z368_Seguimiento>();
        public void SeguimientosAdd(Z368_Seguimiento seguimiento)
        {
            Seguimientos.Add(seguimiento);
            seguimiento.Nombre = this;
        }
    }
}

