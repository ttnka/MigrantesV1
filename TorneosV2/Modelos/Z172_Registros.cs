using System;
using System.ComponentModel.DataAnnotations;

namespace TorneosV2.Modelos
{
	public class Z172_Registros
	{
        [Key]
        [StringLength(50)]
        public string RegistroId { get; set; } = Guid.NewGuid().ToString();
        public DateTime Fecha { get; set; } = DateTime.Now;

        [StringLength(50)]
        public string Registro { get; set; } = "";

        public virtual ICollection<Z170_Files> Archivos { get; set; } = new List<Z170_Files>();

        public int Estado { get; set; } = 2;
        public bool Status { get; set; } = false;

        public Z172_Registros(string registro, int estado, bool status)
        {
            Registro = registro; Estado = estado; Status = status;
        }

        public void ArchivosAdd(Z170_Files file)
        {
            Archivos.Add(file);
            file.Registro = this;
        }

    }
}

