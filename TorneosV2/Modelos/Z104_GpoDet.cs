using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TorneosV2.Modelos
{
	public class Z104_GpoDet
	{
        [Key]
        [StringLength(50)]
        public string GpoDetId { get; set; } = Guid.NewGuid().ToString();
        [ForeignKey("GpoId")]
        [StringLength(50)]
        public string GpoId { get; set; } = "";
        [ForeignKey("UserId")]
        [StringLength(50)]
        public string UserId { get; set; } = "";
        public int Estado { get; set; } = 3;
        public bool Status { get; set; } = true;

        public virtual Z102_Grupo Grupo { get; set; } = default!;

        public Z104_GpoDet(string gpoId, string userId)
        {
            GpoId = gpoId; UserId = userId; 
        }

        public void GpoAdd(Z102_Grupo gpo)
        {
            Grupo = gpo;
            gpo.DetalleAdd(this);
        }

    }
}

