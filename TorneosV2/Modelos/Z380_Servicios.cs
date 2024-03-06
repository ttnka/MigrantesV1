using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NPOI.SS.Formula.Functions;

namespace TorneosV2.Modelos
{
	public class Z380_Servicios
	{
        [Key]
        [StringLength(50)]
        public string ServicioId { get; set; } = Guid.NewGuid().ToString();
        [StringLength(50)]
        [ForeignKey("OrgId")]
        public string OrgId { get; set; } = "";
        [StringLength(20)]
        public string Clave { get; set; } = "";
        [StringLength(50)]
        public string Titulo { get; set; } = "";
        [StringLength(25)]
        public string Tipo { get; set; } = "";
        public string? Observaciones { get; set; }
        public int Estado { get; set; } = 2;
        public bool Status { get; set; } = true;

        public virtual ICollection<Z360_Solicitud> Solicitud { get; set; } = new List<Z360_Solicitud>();
        public void SolicitudAdd(Z360_Solicitud solicitud)
        {
            Solicitud.Add(solicitud);
            solicitud.ServicioAdd(this);
        }

        public virtual Z100_Org Org { get; set; } = default!;
        public void OrgAdd(Z100_Org org)
        {
            Org = org;
            org.ServicioAdd(this);
        }

    }
}

