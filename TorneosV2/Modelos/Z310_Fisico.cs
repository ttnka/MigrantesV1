using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TorneosV2.Modelos
{
	public class Z310_Fisico
	{
        [Key]
        [StringLength(50)]
        public string ContactoId { get; set; } = Guid.NewGuid().ToString();
        [StringLength(50)]
        [ForeignKey("NombreId")]
        public string NombreId { get; set; } = "";
        public int EstaturaCm { get; set; } = 0;
        public int PesoKg { get; set; } = 0;
        public string Piel { get; set; } = "";
        
	}
}

