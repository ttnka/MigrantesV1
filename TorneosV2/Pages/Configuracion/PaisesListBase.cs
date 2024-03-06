using System;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using static TorneosV2.Modelos.MyFunc;
using TorneosV2.Data;
using TorneosV2.Modelos;

namespace TorneosV2.Pages.Configuracion
{
	public class PaisesListBase : ComponentBase
    {
        public const string TBita = "Listado de Paises";

        [Inject]
        public Repo<ZConfig, ApplicationDbContext> ZConfigRepo { get; set; } = default!;
        [Inject]
        public Repo<Z390_Pais, ApplicationDbContext> PaisRepo { get; set; } = default!;

        public List<Z390_Pais> LosPaises { get; set; } = new List<Z390_Pais>();

        public RadzenDataGrid<Z390_Pais>? PaisGrid { get; set; } = new RadzenDataGrid<Z390_Pais>();

        protected bool Primera { get; set; } = true;
        protected bool Leyendo { get; set; } = false;
        protected bool Editando { get; set; } = false;

        protected bool AddFormShow { get; set; } = false;
        protected bool BotonNuevo { get; set; } = false;
        protected string BtnNewText { get; set; } = "Nuevo pais";
        public string Msn { get; set; } = "";


        protected override async Task OnInitializedAsync()
        {
            if (Primera)
            {
                await LeerPaises();
                Primera = false;
            }
        }

        protected void Leer() { }

        protected async Task LeerPaises()
        {
            Leyendo = true;
            var paisesTmp = await PaisRepo.GetAll();
            LosPaises = paisesTmp.Any() ? paisesTmp.ToList() : LosPaises;
            Leyendo = false;
        }

        protected async Task<ApiRespuesta<Z390_Pais>> Servicio(ServiciosTipos tipo, Z390_Pais pais)
        {
            ApiRespuesta<Z390_Pais> resp = new() { Exito = false };

            if (tipo == ServiciosTipos.Insert)
            {
                Z390_Pais paisNew = await PaisRepo.Insert(pais);
                if (paisNew != null)
                {
                    resp.Exito = true;
                    resp.Data = paisNew;
                }
            }
            if (tipo == ServiciosTipos.Update)
            {
                Z390_Pais paisUpdated = await PaisRepo.Update(pais);
                if (paisUpdated != null)
                {
                    resp.Exito = true;
                    resp.Data = paisUpdated;
                }
            }

            return resp;
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

