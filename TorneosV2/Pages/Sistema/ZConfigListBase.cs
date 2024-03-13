using System;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using TorneosV2.Data;
using TorneosV2.Modelos;
using static TorneosV2.Modelos.MyFunc;

namespace TorneosV2.Pages.Sistema
{
	public class ZConfigListBase : ComponentBase
	{
        public const string TBita = "Listado de variables configuracion";
        

        // Event Call Back
        
        [Parameter]
        public EventCallback ReadallConfig { get; set; }
        [Parameter]
        public EventCallback ReadallUser { get; set; }

        //Listado de Valores
        [Parameter]
        public List<ZConfig> LosConfigs { get; set; } = new List<ZConfig>();
        [Parameter]
        public List<ZConfig> LosGrupos { get; set; } = new List<ZConfig>();
        [Parameter]
        public List<Z110_User> LosUsers { get; set; } = new List<Z110_User>();

        // Servicios Insertados
        [Inject]
        public Repo<ZConfig, ApplicationDbContext> ZConfigRepo { get; set; } = default!;
        
        
        public RadzenDataGrid<ZConfig>? ConfigsGrid { get; set; } =
                                        new RadzenDataGrid<ZConfig>();

        protected bool Primera { get; set; } = true;
        protected bool Editando { get; set; } = false;
        public string Msn { get; set; } = "";

        protected override async Task OnInitializedAsync()
        {
            if (Primera)
            {
                LeerGrupos();
                Primera = false;
                Z190_Bitacora bitaT = new(ElUser.UserId, $"{TBita}, Se consulto el listado de varialbes", ElUser.OrgId);
                BitacoraMas(bitaT);
            }
        }

        protected void LeerGrupos()
        {
            LosGrupos = LosConfigs.Any(x => x.Grupo == "Grupos" && x.Tipo == "Encabezado" && x.Status == true) ?
                LosConfigs.Where(x => x.Grupo == "Grupos" && x.Tipo == "Encabezado" && x.Status == true).ToList() :
                new List<ZConfig>();
        }

        protected async Task DetReadConfigs()
        {
            await ReadallConfig.InvokeAsync();
            LeerGrupos();
            await ReadallUser.InvokeAsync();
        }

        protected async Task<ApiRespuesta<ZConfig>> Servicio(ServiciosTipos tipo, ZConfig zcon)
        {
            ApiRespuesta<ZConfig> resp = new() { Exito = false };

            if (tipo == ServiciosTipos.Insert)
            {
                ZConfig zconNew = await ZConfigRepo.Insert(zcon);
                if (zconNew != null)
                {
                    resp.Exito = true;
                    resp.Data = zconNew;
                }
            }
            if (tipo == ServiciosTipos.Update)
            {
                ZConfig zconUpdated = await ZConfigRepo.Update(zcon);
                if (zconUpdated != null)
                {
                    resp.Exito = true;
                    resp.Data = zconUpdated;
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

