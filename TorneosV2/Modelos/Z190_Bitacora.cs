using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TorneosV2.Modelos
{
    public class Z190_Bitacora
    {
        [Key]
        [StringLength(50)]
        public string BitacoraId { get; set; } = Guid.NewGuid().ToString();
        public DateTime Fecha { get; set; } = DateTime.Now;
        [StringLength(50)]
        [ForeignKey("UserId")]
        public string UserId { get; set; } = "";
        public string Desc { get; set; } = "";
        [StringLength(50)]
        [ForeignKey("OrgId")]
        public string OrgId { get; set; } = "";

        public Z190_Bitacora(string userId, string desc, string orgId)
        {
            UserId = userId; Desc = desc; OrgId = orgId;
        }

        public virtual Z100_Org Org { get; set; } = default!;

        public void OrgAdd(Z100_Org org)
        {
            Org = org;
            org.BitacoraAdd(this);
        }

    }
}

