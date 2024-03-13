    using System;
using System.ComponentModel.DataAnnotations;

namespace TorneosV2.Modelos
{
	public class Z390_Pais
	{
        [Key]
        [StringLength(50)]
        public string PaisId { get; set; } = Guid.NewGuid().ToString();
        [StringLength(5)]
        public string Clave { get; set; } = "";
        [StringLength(75)]
        public string Oficial { get; set; } = "";
        [StringLength(25)]
        public string Corto { get; set; } = "";
        [StringLength(5)]
        public string Telefono { get; set; } = "";
        public bool Favorito { get; set; } = false;
        public int Estado { get; set; } = 2;
        public bool Status { get; set; } = true;

        public string ClaveCorto => Clave + " " + Corto;
        public string ClaveTel => Clave + " " + Telefono;

        public virtual ICollection<Z304_Domicilio> Domicilio { get; set; } = new List<Z304_Domicilio>();

        public void DomicilioAdd(Z304_Domicilio domicilio)
        {
            Domicilio.Add(domicilio);
            domicilio.Pais = this;
        }



        
	}
}

