using System;
using MathNet.Numerics.Distributions;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using TorneosV2.Data;
using TorneosV2.Modelos;
using static TorneosV2.Modelos.MyFunc;

namespace TorneosV2.Pages.Sistema
{
	public class FilesListBase : ComponentBase
	{
        public const string TBita = "Listado de archivos Imagenes y otros";

        [Inject]
        public Repo<Z170_Files, ApplicationDbContext> FileRepo { get; set; } = default!;

        // Listas y clases
        [Parameter]
        public string ElRegistro { get; set; } = "";
        public List<Z170_Files> LosDatos { get; set; } = new List<Z170_Files>();


        public RadzenDataGrid<Z170_Files>? FilesGrid { get; set; } = new RadzenDataGrid<Z170_Files>();

        protected bool Primera { get; set; } = true;
        protected bool Leyendo { get; set; } = false;
        protected bool Editando { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            if (Primera)
            {
                Primera = false;
                
            }

            await Leer();
        }

        protected async Task Leer()
        {
            if (ElRegistro == "") return;
            IEnumerable<Z170_Files> resp = await FileRepo.Get(x => x.RegistroId == ElRegistro);
            LosDatos = (resp != null && resp.Any()) ? resp.ToList() : new List<Z170_Files>();
            Z190_Bitacora bitaTemp = new(ElUser.UserId, $"Consulto la seccion de {TBita}",  ElUser.OrgId);
            await BitacoraAll(bitaTemp);   
        }

        protected async Task<ApiRespuesta<Z170_Files>> Servicio(ServiciosTipos tipo, Z170_Files archivo)
        {
            ApiRespuesta<Z170_Files> resp = new() { Exito = false };
            
            if (tipo == ServiciosTipos.Insert)
            {
                Z170_Files fileUped = await FileRepo.Insert(archivo);
                if (fileUped != null)
                {
                    resp.Exito = true;
                    resp.Data = fileUped;
                }
            }
            if (tipo == ServiciosTipos.Update)
            {
                Z170_Files fileUpDated = await FileRepo.Update(archivo);
                if (fileUpDated != null)
                {
                    resp.Exito = true;
                    resp.Data = fileUpDated;
                }
            }
            
            return resp;
        }

        #region Usuario y Bitacora

        [CascadingParameter(Name = "ElUserAll")]
        public Z110_User ElUser { get; set; } = default!;
        [CascadingParameter(Name = "LaOrgAll")]
        public Z100_Org LaOrg { get; set; } = default!;

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

