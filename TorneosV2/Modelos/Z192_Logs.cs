using System;
using System.ComponentModel.DataAnnotations;

namespace TorneosV2.Modelos
{
    public class Z192_Logs
    {
        [Key]
        [StringLength(50)]
        public string LogId { get; set; } = Guid.NewGuid().ToString();
        public DateTime Fecha { get; set; } = DateTime.Now;
        [StringLength(50)]
        public string UserId { get; set; } = "";
        public string Desc { get; set; } = "";
        public bool Sistema { get; set; } = false;

        public Z192_Logs (string userId, string desc, bool sistema)
        {
            UserId = userId; Desc = desc; Sistema = sistema;
        }
 
    }
}

