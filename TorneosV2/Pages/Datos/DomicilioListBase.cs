using System;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using TorneosV2.Data;
using TorneosV2.Modelos;
using static TorneosV2.Modelos.MyFunc;

namespace TorneosV2.Pages.Datos
{
	public class DomicilioListBase : ComponentBase
	{
        public const string TBita = "Listado de Domicilios";

        [Parameter]
        public bool Administracion { get; set; } = false;

        [Inject]
        public Repo<ZConfig, ApplicationDbContext> ZConfigRepo { get; set; } = default!;

        [Inject]
        public Repo<Z304_Domicilio, ApplicationDbContext> DomicilioRepo { get; set; } = default!;
        [Inject]
        public Repo<Z390_Pais, ApplicationDbContext> PaisRepo { get; set; } = default!;

        //protected List<KeyValuePair<string, string>> Sexos { get; set; } =
        //    new List<KeyValuePair<string, string>>();

        [Parameter]
        public Z300_Nombres ElNombre { get; set; } = default!;
        protected List<Z304_Domicilio> LosDomicilios { get; set; } = new List<Z304_Domicilio>();
        protected List<Z390_Pais> LosPaises { get; set; } = new List<Z390_Pais>();
        protected List<ZConfig> LosTipos { get; set; } = new List<ZConfig>();

        public RadzenDataGrid<Z304_Domicilio>? DomiciliosGrid { get; set; } =
                                        new RadzenDataGrid<Z304_Domicilio>();

        protected bool Primera { get; set; } = true;
        protected bool Leyendo { get; set; } = false;
        protected bool Editando { get; set; } = false;

        public string Msn { get; set; } = "";

        protected override async Task OnInitializedAsync()
        {
            if (Primera)
            {
                await Leer();
                await LeerDomicilios();
                Primera = false;
            }
        }

        protected async Task Leer()
        {
            var pTmp = await PaisRepo.Get();
            LosPaises = pTmp.Any() ? pTmp.ToList() : LosPaises;
        }

        protected async Task LeerDomicilios()
        {
            Leyendo = true;
            var domTmp = await DomicilioRepo.Get(x => x.NombreId == ElNombre.NombreId);
            LosDomicilios = domTmp.Any() ? domTmp.ToList() : LosDomicilios;
            Leyendo = false;
        }

        protected async Task<ApiRespuesta<Z304_Domicilio>> Servicio(ServiciosTipos tipo, Z304_Domicilio dom)
        {
            ApiRespuesta<Z304_Domicilio> resp = new() { Exito = false };

            if (tipo == ServiciosTipos.Insert)
            {
                Z390_Pais paisTemp = LosPaises.Any(x => x.PaisId == dom.PaisId) ?
                    LosPaises.FirstOrDefault(x => x.PaisId == dom.PaisId)! :
                    LosPaises.FirstOrDefault(x => x.Status == true)!;

                dom.PaisAdd(paisTemp);
                dom.NombreAdd(ElNombre);
                Z304_Domicilio domNew = await DomicilioRepo.Insert(dom);
                if (domNew != null)
                {
                    resp.Exito = true;
                    resp.Data = domNew;
                }
            }
            if (tipo == ServiciosTipos.Update)
            {
                Z304_Domicilio domUpdated = await DomicilioRepo.Update(dom);
                if (domUpdated != null)
                {
                    resp.Exito = true;
                    resp.Data = domUpdated;
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

	