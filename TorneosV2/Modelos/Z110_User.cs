using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TorneosV2.Modelos
{
    public class Z110_User
    {
        [Key]
        [StringLength(50)]
        public string UserId { get; set; } = "";
        [StringLength(25)]
        public string Nombre { get; set; } = "";
        [StringLength(25)]
        public string Paterno { get; set; } = "";
        [StringLength(25)]
        public string? Materno { get;set; }
        public int Nivel { get; set; } = 1;
        [StringLength(50)]
        [ForeignKey("OrgId")]
        public string OrgId { get; set; } = "";
        [StringLength(75)]
        public string OldEmail { get; set; } = "";
        public int Estado { get; set; } = 3;
        public bool Status { get; set; } = true;

        public string Completo => Nombre + " " + Paterno + " " + Materno;
        public virtual Z100_Org Org { get; set; } = default!;

        public Z110_User(string userId, string nombre, string paterno, string? materno,
            int nivel, string orgId, string oldEmail, int estado, bool status)
        {
            UserId = userId; Nombre = nombre; Paterno = paterno; Materno = materno; Nivel = nivel;
            OrgId = orgId; OldEmail = oldEmail; Estado = estado; Status = status;
        }
        
        public void OrgAdd(Z100_Org org)
        {
            Org = org;
            org.UserAdd(this);
        }

    }
}

