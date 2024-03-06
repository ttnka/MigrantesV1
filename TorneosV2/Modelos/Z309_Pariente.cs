﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TorneosV2.Modelos
{
	public class Z309_Pariente
	{
        [Key]
        [StringLength(50)]
        public string ContactoId { get; set; } = Guid.NewGuid().ToString();

        [StringLength(50)]
        [ForeignKey("NombreId")]
        public string NombreId { get; set; } = "";

        [StringLength(50)]
        [ForeignKey("NombreId")]
        public string ParienteId { get; set; } = "";

        [StringLength(50)]
        public string Parentesco { get; set; } = "";
        public int Estado { get; set; } = 2;
        public bool Status { get; set; } = true;


        public virtual Z300_Nombres Nombre { get; set; } = default!;
        public void NombreAdd(Z300_Nombres nombre)
        {
            Nombre = nombre;
            nombre.ParientesAdd(this);
        }

        
	}
}
