using System;
using System.ComponentModel.DataAnnotations;

namespace TorneosV2.Modelos
{
	public class Z102_Grupo
	{
        [Key]
        [StringLength(50)]
        public string GrupoId { get; set; } = Guid.NewGuid().ToString();
        [StringLength(50)]
        public string Titulo { get; set; } = "";
        public string Desc { get; set; } = "";
        [StringLength(50)]
        public string Administrador { get; set; } = "";
        public int Estado { get; set; } = 3;
        public bool Status { get; set; } = true;

        
        public Z102_Grupo (string titulo, string desc, int estado, bool status)
        {
            Titulo = titulo; Desc = desc; Estado = estado; Status = status;
        }

        public virtual ICollection<Z104_GpoDet> GpoDetalles { get; set; } = new List<Z104_GpoDet>();

        public void DetalleAdd(Z104_GpoDet det)
        {
            GpoDetalles.Add(det);
            det.Grupo = this;
        }

    }
}

