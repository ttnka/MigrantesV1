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
        

        // Event Call Back
        [Parameter]
        public EventCallback ReadPaises { get; set; }

        // Servicios Insertados
        [Inject]
        public Repo<Z390_Pais, ApplicationDbContext> PaisRepo { get; set; } = default!;

        // Listado de valores
        [Parameter]
        public List<Z390_Pais> LosPaises { get; set; } = new List<Z390_Pais>();

        public RadzenDataGrid<Z390_Pais>? PaisGrid { get; set; } = new RadzenDataGrid<Z390_Pais>();
        
        protected bool Primera { get; set; } = true;
        protected bool Editando { get; set; } = false;

        public string Msn { get; set; } = "";

        protected override async Task OnInitializedAsync()
        {
            if (Primera)
            {
                Primera = false;
                Z190_Bitacora bitaT = new(ElUser.UserId, $"{TBita}, Se consulto el listado de paises y claves telefonicas", ElUser.OrgId);
                BitacoraMas(bitaT);
            }
        }

        protected void Leer() { }

        protected async Task DetReadPaises()
        {
            await ReadPaises.InvokeAsync();
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
        [CascadingParameter(Name ="LasBitacorasAll")]
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
            foreach(var b in LasBitacoras)
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

