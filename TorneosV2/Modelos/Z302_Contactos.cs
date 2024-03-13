using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TorneosV2.Modelos
{
	public class Z302_Contactos
	{
        [Key]
        [StringLength(50)]
        public string ContactoId { get; set; } = Guid.NewGuid().ToString();
   
        [StringLength(50)]
        [ForeignKey("NombreId")]
        public string NombreId { get; set; } = "";
        [StringLength(50)]
        public string Pais { get; set; } = "";
        [StringLength(50)]
        public string Tipo { get; set; } = "";
        [StringLength(75)]
        public string Valor { get; set; } = "";

        public string? Observacion { get; set; }
        public int Estado { get; set; } = 2;
        public bool Status { get; set; } = true;


        public virtual Z300_Nombres Nombre { get; set; } = default!;
        public void NombreAdd(Z300_Nombres nombre)
        {
            Nombre = nombre;
            nombre.ConcactoAdd(this);
        }

        
	}
}

