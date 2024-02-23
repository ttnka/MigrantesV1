using System;
using Microsoft.AspNetCore.Components;
using Radzen;
using TorneosV2.Data;
using TorneosV2.Modelos;

namespace TorneosV2.Pages.Sistema
{
	public class MisDatosBase : ComponentBase
	{
        public const string TBita = "Mis datos!";
        /*
        [Inject]
        public IEnviarMail ReenviarMail { get; set; } = default!;
        [Inject]
        public Repo<ZConfig, ApplicationDbContext> ZConfigRepo { get; set; } = default!;
        [Inject]
        public Repo<Z100_Org, ApplicationDbContext> OrgRepo { get; set; } = default!;
        [Inject]
        public Repo<Z110_User, ApplicationDbContext> UserRepo { get; set; } = default!;

        
        protected bool Leyendo { get; set; } = false;
        protected bool Editando { get; set; } = false;

        protected bool AddFormShow { get; set; } = false;
        protected bool BotonNuevo { get; set; } = false;
        protected string BtnNewText { get; set; } = "Nueva Organizacion";
        public string Msn { get; set; } = "";
        */

        protected bool Primera { get; set; } = true;

        protected override async Task OnInitializedAsync()
        {
            if (Primera)
            {
                //Leer();
                Primera = false;
            }

            //await LeerOrgs();
        }

        


        

        #region Usuario y Bitacora

        [CascadingParameter(Name = "ElUserAll")]
        public Z110_User ElUser { get; set; } = default!;


        [Inject]
        public Repo<Z190_Bitacora, ApplicationDbContext> BitaRepo { get; set; } = default!;
        [Inject]
        public Repo<Z192_Logs, ApplicationDbContext> LogRepo { get; set; } = default!;

        public MyFunc MyFunc { get; set; } = new MyFunc();
        public NotificationMessage ElMsn(string tipo, string titulo, string mensaje, int duracion)
        {
            NotificationMessage respuesta = new();
            switch (tipo)
            {
                case "Info":
                    respuesta.Severity = NotificationSeverity.Info;
                    break;
                case "Error":
                    respuesta.Severity = NotificationSeverity.Error;
                    break;
                case "Warning":
                    respuesta.Severity = NotificationSeverity.Warning;
                    break;
                default:
                    respuesta.Severity = NotificationSeverity.Success;
                    break;
            }
            respuesta.Summary = titulo;
            respuesta.Detail = mensaje;
            respuesta.Duration = 4000 + duracion;
            return respuesta;
        }
        [Inject]
        public NavigationManager NM { get; set; } = default!;
        public Z190_Bitacora LastBita { get; set; } = new(userId: "", desc: "", orgId: "");
        public Z192_Logs LastLog { get; set; } = new(userId: "Sistema", desc: "", sistema: false);
        public async Task BitacoraAll(Z190_Bitacora bita)
        {
            if (bita.BitacoraId != LastBita.BitacoraId)
            {
                LastBita = bita;
                await BitaRepo.Insert(bita);
            }
        }

        public async Task LogAll(Z192_Logs log)
        {
            if (log.LogId != LastLog.LogId)
            {
                LastLog = log;
                await LogRepo.Insert(log);
            }

        }
        #endregion

    }
}

