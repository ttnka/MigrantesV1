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
        

        // Event CallBack
        [Parameter]
        public EventCallback<Z300_Nombres> ReadDEDomicilios { get; set; }
        [Parameter]
        public EventCallback ReadDETipos { get; set; }
        [Parameter]
        public EventCallback ReadDEPaises { get; set; }

        // Servicios Insertados
        [Inject]
        public Repo<Z304_Domicilio, ApplicationDbContext> DomicilioRepo { get; set; } = default!;

        // Listado de valores
        [Parameter]
        public Z300_Nombres ElNombre { get; set; } = new();
        [Parameter]
        public List<Z304_Domicilio> LosDomicilios { get; set; } = new List<Z304_Domicilio>();
        [Parameter]
        public List<Z390_Pais> LosPaises { get; set; } = new List<Z390_Pais>();
        [Parameter]
        public List<ZConfig> LosTipos { get; set; } = new List<ZConfig>();
        [Parameter]
        public int IndexTabD { get; set; }
        [Parameter]
        public bool ShowDataD { get; set; }

        public RadzenDataGrid<Z304_Domicilio>? DomiciliosGrid { get; set; } =
                                        new RadzenDataGrid<Z304_Domicilio>();

        protected bool Primera { get; set; } = true;
        protected bool Editando { get; set; } = false;
        public string Msn { get; set; } = "";

        protected override async Task OnInitializedAsync()
        {
            if (Primera)
            {
                Primera = false;
                Z190_Bitacora bitaT = new(ElUser.UserId,
                    $"{TBita}, se consulto el listado de domicilios {ElNombre.Completo}", ElUser.OrgId);
                BitacoraMas(bitaT);
            }
        }

        
        protected async Task LeerDomicilios()
        {
            await ReadDEDomicilios.InvokeAsync(ElNombre);
            await ReadDEPaises.InvokeAsync();
            await ReadDETipos.InvokeAsync();
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
                dom.Estado = 1;
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

	