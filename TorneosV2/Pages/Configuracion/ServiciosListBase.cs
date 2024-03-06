using System;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using TorneosV2.Data;
using TorneosV2.Modelos;
using static TorneosV2.Modelos.MyFunc;

namespace TorneosV2.Pages.Configuracion
{
	public class ServiciosListBase : ComponentBase 
	{
        public const string TBita = "Listado de Servicios";

        [Inject]
        public Repo<ZConfig, ApplicationDbContext> ZConfigRepo { get; set; } = default!;
        [Inject]
        public Repo<Z100_Org, ApplicationDbContext> OrgRepo { get; set; } = default!;
        [Inject]
        public Repo<Z380_Servicios, ApplicationDbContext> ServiciosRepo { get; set; } = default!;

        public List<Z100_Org> LasOrgs { get; set; } = new List<Z100_Org>();
        public List<Z380_Servicios> LosServicios { get; set; } = new List<Z380_Servicios>();

        public RadzenDataGrid<Z380_Servicios>? ServiciosGrid { get; set; } =
                                        new RadzenDataGrid<Z380_Servicios>();

        protected bool Primera { get; set; } = true;
        protected bool Leyendo { get; set; } = false;
        protected bool Editando { get; set; } = false;

        protected bool AddFormShow { get; set; } = false;
        protected bool BotonNuevo { get; set; } = false;
        protected string BtnNewText { get; set; } = "Nuevo servicio";
        public string Msn { get; set; } = "";


        protected override async Task OnInitializedAsync()
        {
            if (Primera)
            {
                await Leer();
                await LeerServicios();
                Primera = false;
            }
        }

        protected async Task Leer()
        {
            var orgTmp = await OrgRepo.GetAll();
            LasOrgs = orgTmp.Any() ? orgTmp.ToList() : LasOrgs;
        }

        protected async Task LeerServicios()
        {
            Leyendo = true;
            var servTmp = await ServiciosRepo.Get(x => x.OrgId == (ElUser.Nivel < 6 ?
                    ElUser.OrgId : x.OrgId));
            LosServicios = servTmp.Any() ? servTmp.ToList() : LosServicios;
            Leyendo = false;
        }

        protected async Task<ApiRespuesta<Z380_Servicios>> Servicio(ServiciosTipos tipo, Z380_Servicios serv)
        {
            ApiRespuesta<Z380_Servicios> resp = new() { Exito = false };

            if (tipo == ServiciosTipos.Insert)
            {
                Z380_Servicios servNew = await ServiciosRepo.Insert(serv);
                if (servNew != null)
                {
                    resp.Exito = true;
                    resp.Data = servNew;
                }
            }
            if (tipo == ServiciosTipos.Update)
            {
                Z380_Servicios servUpdated = await ServiciosRepo.Update(serv);
                if (servUpdated != null)
                {
                    resp.Exito = true;
                    resp.Data = servUpdated;
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

