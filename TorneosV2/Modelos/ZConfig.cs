using System;
using System.ComponentModel.DataAnnotations;

namespace TorneosV2.Modelos
{
    public class ZConfig
    {
        [Key]
        [StringLength(50)]
        public string ConfigId { get; private set; } = Guid.NewGuid().ToString();
        
        [StringLength(25)]
        public string Grupo { get; private set; } = "";
        [StringLength(25)]
        public string Tipo { get; private set; } = "";
        [StringLength(50)]
        public string Titulo { get; private set; } = "";

        [StringLength(50)]
        public string? Usuario { get; private set; } = "";
        public string? Txt { get; private set; } = "";
        public int Entero { get; private set; } = 0;
        public decimal Decim { get; private set; } = 0;
        public DateTime Fecha1 { get; private set; }
        public DateTime Fecha2 { get; private set; }
        public bool SiNo { get; private set; } = true;

        public int Estado { get; private set; } = 3;
        public bool Status { get; private set; } = true;

        public ZConfig (string grupo, string tipo, string titulo, string usuario,
            string txt, int entero, decimal decim, DateTime fecha1, DateTime fecha2)
        {
            Grupo = grupo; Tipo = tipo; Titulo = titulo; Usuario = usuario; Txt = txt;
            Entero = entero; Decim = decim; Fecha1 = fecha1; Fecha2 = fecha2;
        }

    }
}

