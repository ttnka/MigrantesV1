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

        //Event Call Back
        
        [Parameter]
        public EventCallback ReadOrganizaciones { get; set; }
        [Parameter]
        public EventCallback ReadServicios { get; set; }

        // Servicios Injectados
        [Inject]
        public Repo<ZConfig, ApplicationDbContext> ZConfigRepo { get; set; } = default!;
        [Inject]
        public Repo<Z380_Servicios, ApplicationDbContext> ServiciosRepo { get; set; } = default!;

        // Parametros con valores
        [Parameter]
        public List<Z100_Org> LasOrgs { get; set; } = new List<Z100_Org>();
        [Parameter]
        public List<Z380_Servicios> LosServicios { get; set; } = new List<Z380_Servicios>();
        
        public RadzenDataGrid<Z380_Servicios>? ServiciosGrid { get; set; } =
                                        new RadzenDataGrid<Z380_Servicios>();

        protected bool Primera { get; set; } = true;
        protected bool Editando { get; set; } = false;
        public string Msn { get; set; } = "";

        protected override async Task OnInitializedAsync()
        {
            if (Primera)
            {
                Primera = false;
                Z190_Bitacora bitaT = new(ElUser.UserId, $"{TBita}, Se consulto el listado de servicios que se prestan", ElUser.OrgId);
                BitacoraMas(bitaT);
            }
        }

        
        protected async Task DetReadOrganizaciones() => await ReadOrganizaciones.InvokeAsync();

        protected async Task DetReadServicios() => await ReadServicios.InvokeAsync();
           
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
        [CascadingParameter(Name = "LasBitacorasAll")]
        public List<Z190_Bitacora> LasBitacoras { get; set; } = new List<Z190_Bitacora>();

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
        public void BitacoraMas(Z190_Bitacora bita)
        {
            if (!LasBitacoras.Any(b => b.BitacoraId == bita.BitacoraId))
            {

                LasBitacoras.Add(bita);
            }
        }
        public async Task BitacoraWrite()
        {
            foreach (var b in LasBitacoras)
            {
                b.OrgAdd(ElUser.Org);
            }
            await BitaRepo.InsertPlus(LasBitacoras);
            LasBitacoras.Clear();
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

