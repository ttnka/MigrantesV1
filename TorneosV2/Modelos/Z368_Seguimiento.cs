using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TorneosV2.Modelos
{
	public class Z368_Seguimiento
	{
        [Key]
        [StringLength(50)]
        public string SeguimientoId { get; set; } = Guid.NewGuid().ToString();

        [StringLength(50)]
        [ForeignKey("DetalleId")]
        public string DetalleId { get; set; } = "";
        [StringLength(50)]
        [ForeignKey("NombreId")]
        public string NombreId { get; set; } = "";
        [StringLength(50)]
        public string Responsable { get; set; } = "";
        public string Observacion { get; set; } = "";
        public int Estado { get; set; } = 2;
        public bool Status { get; set; } = true;

        public virtual Z300_Nombres Nombre { get; set; } = default!;
        public void NombreAdd(Z300_Nombres nombre)
        {
            Nombre = nombre;
            nombre.SeguimientosAdd(this);
        }

        public virtual Z360_Solicitud Solicitud { get; set; } = default!;
        public void SolicitudAdd(Z360_Solicitud solicitud)
        {
            Solicitud = solicitud;
            solicitud.SeguimientoAdd(this);
        }

        
	}
}

