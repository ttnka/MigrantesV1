using System;
namespace TorneosV2.Modelos
{
    public class Constantes
    {
        public const string ElDominio = "https://torneos.mx";
        //public const string ElDominio = "localhost:7234";
        // Mail 01
        // Este usuario envia los mails
        public const string DeNombreMail01Soporte = "Soporte WebMaster";
        public const string DeMail01Soporte = "soporte@zuverworks.com";
        public const string ServerMail01 = "mail.omnis.com";
        public const int PortMail01 = 587;
        public const int Nivel01 = 8;
        public const string UserNameMail01 = "soporte@zuverworks.com";
        public const string PasswordMail01 = "wB.2468022";
        

        // Mail 02
        // Ese es el usuario del sistema pero no envia mails
        public const string DeNombreMail02 = "WebMaster";
        public const string DeMail02 = "webmaster@zuverworks.com";
        public const string ServerMail02 = "mail.omnis.com";
        public const int PortMail02 = 587;
        public const string UserNameMail02 = "webmaster@zuverworks.com";
        public const string PasswordMail02 = "2468022Ih.";

        // Registro Inicial Publico en GENERAL Organizacion

        public const string PgRfc = "PGE010101AAA";
        public const string PgRazonSocial = "Publico en General";
        public const int PgEstado = 3;  // En caso de no quere que se utilice poner 2
        public const bool PgStatus = true;
        public const string PgMail = "peg@peg.com";

        // Registro Usuario Publico en GENERAL

        public const string DeNombreMailPublico = "Publico";
        public const string DeMailPublico = "publico@torneos.mx";
        public const int EstadoPublico = 3;
        public const int NivelPublico = 1;
        public const string UserNameMailPublico = "publico@torneos.mx";
        public const string PasswordMailPublico = "PublicoLibre1.";

        // Registro de Sistema
        public const string SyRfc = "ZME130621FFA";
        public const string SyRazonSocial = "Zuverworks de Mexico";
        public const int SyEstado = 1;
        public const bool SyStatus = true;
        public const string SyMail = "webmaster@zuverworks.com";
        public const string SysPassword = "24680212Ih.";

        // Configuracion
        public const string Arranque = "2.468022";

        public const string OrgTipo = "Administracion,Proveedor,Cliente";
        public const string Grupo = "General";
        public const string Niveles = "Registrado,Proveedor,Jugador,Cliente,Cliente_Admin,Zuver,Zuver_Admin,ABD";

        public const bool EsNecesarioConfirmarMail = false;
        public const string ConfirmarMailTxt = "https://torneos.mx/Account/ConfirmEmail/Id=";

        public const string FolderImagenes = "Imagenes";

        

    }
}

