using System;
using System.ComponentModel.DataAnnotations;

namespace TorneosV2.Modelos
{
    public class Z100_Org
    {
        [Key]
        [StringLength(50)]
        public string OrgId { get; set; } = Guid.NewGuid().ToString();
        [StringLength(15)]
        public string Rfc { get; set; } = "";
        [StringLength(25)]
        public string Comercial { get; set; } = "";
        [StringLength(75)]
        public string? RazonSocial { get; set; } = "";
        public bool Moral { get; set; } = true;
        [StringLength(10)]
        public string? NumCliente { get; set; } = "";
        [StringLength(15)]
        public string Tipo { get; set; } = "Cliente";
        public int Estado { get; set; } = 3;
        public bool Status { get; set; } = true;

        public string ComercialRfc => Comercial + " " + Rfc;

        public Z100_Org(string rfc, string comercial, string razonSocial, string? numCliente, string tipo,
            int estado, bool status)
        {
            Rfc = rfc; Comercial = comercial; RazonSocial = razonSocial; NumCliente = numCliente; Tipo = tipo;
            Estado = estado; Status = status;
        }

        public virtual ICollection<Z110_User> Users { get; set; } = new List<Z110_User>();
        public void UserAdd(Z110_User user)
        {
            Users.Add(user);
            user.Org = this;
        }

        public virtual ICollection<Z190_Bitacora> Bitacoras { get; set; } = new List<Z190_Bitacora>();
        public void BitacoraAdd(Z190_Bitacora bita)
        {
            Bitacoras.Add(bita);
            bita.Org = this;
        }
        public virtual ICollection<Z380_Servicios> Servicios { get; set; } = new List<Z380_Servicios>();
        public void ServicioAdd(Z380_Servicios servicio)
        {
            Servicios.Add(servicio);
            servicio.Org = this;
        }

    }
}

