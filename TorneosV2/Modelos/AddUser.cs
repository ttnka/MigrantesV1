using System;
namespace TorneosV2.Modelos
{
    public class AddUser
    {
        public string UserId { get; set; } = "";
        public string Mail { get; set; } = "";
        public string Pass { get; set; } = "";
        public string Confirm { get; set; } = "";

        public string Nombre { get; set; } = "";
        public string Paterno { get; set; } = "";
        public string Materno { get; set; } = "";
        public string OldMail { get; set; } = "";

        public string OrgId { get; set; } = "";
        public string OrgName { get; set; } = "";
        public int Nivel { get; set; } = 1;
        public string Url { get; set; } = "";

        //public string UsuarioId { get; private set; } = "";
        public bool Sistema { get; set; } = true;

        //public string Url { get; private set; } = "";

        public AddUser (string userId, string mail, string pass, 
            string nombre, string paterno, string materno, string oldMail,
            string orgId, string orgName, int nivel, string url, bool sistema)
        {
            UserId = userId; Mail = mail; Pass = pass; 
            Nombre = nombre; Paterno = paterno; Materno = materno; OldMail = oldMail;
            OrgId = orgId; OrgName = orgName; Nivel = nivel; Url = url; Sistema = sistema;
        }

    }
}

