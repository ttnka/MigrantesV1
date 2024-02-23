using System;
namespace TorneosV2.Modelos
{
    public class MailCampos
    {
        public List<string> ParaNombre { get; set; } = new List<string>();
        public List<string> ParaEmail { get; set; } = new List<string>();
        public string Titulo { get; set; } = "";
        public string Cuerpo { get; set; } = "";

        public string SenderName { get; set; } = "";
        public string SenderEmail { get; set; } = "";
        public string Replayto { get; set; } = "";

        public string Server { get; set; } = "";
        public int Port { get; set; }
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";

        public string UserId { get; set; } = "";
        public string OrgId { get; set; } = "";
        

        public MailCampos PoblarMail(List<string> paraNombre, List<string> paraEmail,
            string titulo, string cuerpo, string userId, string orgId, 
            string senderName, string senderEMail, string replayto, string server,
            int port, string userName, string password)
        {
            this.ParaEmail = paraEmail;
            this.ParaNombre = paraNombre;
            this.Replayto = replayto;
            this.Titulo = titulo;
            this.Cuerpo = cuerpo;
            this.UserId = userId;
            this.OrgId = orgId;
            
            this.SenderName = senderName;
            this.SenderEmail = senderEMail;
            this.Server = server;
            this.Port = port;
            this.UserName = userName;
            this.Password = password;
            return this;
        }
    }
}

