using System;
using System.ComponentModel.DataAnnotations;

namespace TorneosV2.Modelos
{
    /*
      Instrucciones generales de llenado:
        Un grupo de datos son:
        --- ENCABEZADO --                 --- Encabezado ---
        Grupo: Contactos                  Grupo: XXXX
        Tipo: Encabezado                  Tipo: Encabezado
        Titulo: Datos de Contacto         Titulo: 
        ---Elementos ---                  --- Elementos ---

        Grupo: Contactos
        Tipo: Elementos
        Titulo: Telefono
        Entero: 10 digitos

        Grupo: Contactos
        Tipo: Elementos
        Titulo: Email
        Entero: 75 digitos
     */
    public class ZConfig
    {
        [Key]
        [StringLength(50)]
        public string ConfigId { get; set; } = Guid.NewGuid().ToString();
        
        [StringLength(25)]
        public string Grupo { get; set; } = "";
        [StringLength(25)]
        public string Tipo { get; set; } = "";
        [StringLength(50)]
        public string Titulo { get; set; } = "";

        [StringLength(50)]
        public string? Usuario { get; set; } = "";
        public string? Txt { get; set; } = "";
        public int Entero { get; set; } = 0;
        public decimal Decim { get; set; } = 0;
        public DateTime Fecha1 { get; set; }
        public DateTime Fecha2 { get; set; }
        public bool SiNo { get; set; } = true;

        public int Estado { get; set; } = 3;
        public bool Status { get; set; } = true;


    }
}

