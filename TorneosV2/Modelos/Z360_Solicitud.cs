using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NPOI.SS.Formula.Functions;

namespace TorneosV2.Modelos
{
	public class Z360_Solicitud
	{
        [Key]
        [StringLength(50)]
        public string ContactoId { get; set; } = Guid.NewGuid().ToString();

        public DateTime Fecha { get; set; } = DateTime.Today;
        [StringLength(50)]
        [ForeignKey("ServicioId")]
        public string ServicioId { get; set; } = "";
        public string? Observaciones { get; set; }
        public int Estado { get; set; } = 2;
        public bool Status { get; set; } = true;

        public virtual Z380_Servicios Servicio { get; set; } = default!;
        public void ServicioAdd(Z380_Servicios servicio)
        {
            Servicio = servicio;
            servicio.SolicitudAdd(this);
        }

        

        public virtual ICollection<Z362_SolDet> Detalles { get; set; } = new List<Z362_SolDet>();
        public void DetalleAdd(Z362_SolDet detalle)
        {
            Detalles.Add(detalle);
            detalle.Solicitud = this;
        }
        public virtual ICollection<Z368_Seguimiento> Seguimientos { get; set; } = new List<Z368_Seguimiento>();
        public void SeguimientoAdd(Z368_Seguimiento seguimiento)
        {
            Seguimientos.Add(seguimiento);
            seguimiento.Solicitud = this;
        }
    }
}

