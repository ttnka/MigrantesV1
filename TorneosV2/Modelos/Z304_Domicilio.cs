using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TorneosV2.Modelos
{
	public class Z304_Domicilio
	{
        [Key]
        [StringLength(50)]
        public string ContactoId { get; set; } = Guid.NewGuid().ToString();

        [StringLength(50)]
        [ForeignKey("NombreId")]
        public string NombreId { get; set; } = "";
        [StringLength(50)]
        [ForeignKey("PaisId")]
        public string PaisId { get; set; } = "";
        [StringLength(50)]
        public string Edo { get; set; } = "";
        [StringLength(50)]
        public string Municipio { get; set; } = "";
        [StringLength(50)]
        public string Ciudad { get; set; } = "";
        [StringLength(50)]
        public string Colonia { get; set; } = "";
        [StringLength(50)]
        public string Calle { get; set; } = "";
        public string? Observaciones { get; set; } = "";
        public int Estado { get; set; } = 2;
        public bool Status { get; set; } = true;

        public virtual Z300_Nombres Nombre { get; set; } = default!;
        public void NombreAdd(Z300_Nombres nombre)
        {
            Nombre = nombre;
            nombre.DomiciliosAdd(this);
        }

        public virtual Z390_Pais Pais { get; set; } = default!;
        public void PaisAdd(Z390_Pais pais)
        {
            Pais = pais;
            pais.DomicilioAdd(this);
        }
        
	}
}

