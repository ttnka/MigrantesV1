using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TorneosV2.Modelos
{
	public class Z170_Files
	{
        [Key]
        [StringLength(50)]
        public string FileId { get; set; } = Guid.NewGuid().ToString();
        public DateTime Fecha { get; set; } = DateTime.Now;

        [StringLength(50)]
        [ForeignKey("RegistroId")]
        public string RegistroId { get; set; } = "";
        public virtual Z172_Registros Registro { get; set; } = default!;

        [StringLength(25)]
        public string Tipo { get; set; } = "";
        public string Folder { get; set; } = "";
        public string Archivo { get; set; } = "";

        [StringLength(75)]
        public string Titulo { get; set; } = "";

        public int Estado { get; set; } = 2;
        public bool Status { get; set; } = true;

        public Z170_Files(string tipo, string folder, string archivo, string titulo, int estado, bool status)
        {
            Tipo = tipo; Folder = folder; Archivo = archivo; 
            Titulo = titulo; Estado = estado; Status = status;
        }

        public void RegistroAdd(Z172_Registros registro)
        {
            Registro = registro;
            registro.ArchivosAdd(this);
        }

    }
}

