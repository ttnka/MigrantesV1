using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NPOI.SS.Formula.Functions;

namespace TorneosV2.Modelos
{
	public class Z362_SolDet
	{
        [Key]
        [StringLength(50)]
        public string SolDetId { get; set; } = Guid.NewGuid().ToString();

        [StringLength(50)]
        [ForeignKey("SolicitudId")]
        public string SolicitudId { get; set; } = "";
        [StringLength(50)]
        [ForeignKey("NombreId")]
        public string NombreId { get; set; } = "";

        public int Estado { get; set; } = 2;
        public bool Status { get; set; } = true;

        public virtual Z300_Nombres Nombre { get; set; } = default!;
        public void NombreAdd(Z300_Nombres nombre)
        {
            Nombre = nombre;
            nombre.SolicitudesAdd(this);
        }

        public virtual Z360_Solicitud Solicitud { get; set; } = default!;
        public void SolicitudAdd(Z360_Solicitud solicitud)
        {
            Solicitud = solicitud;
            solicitud.DetalleAdd(this);
        }

        
	}
}

