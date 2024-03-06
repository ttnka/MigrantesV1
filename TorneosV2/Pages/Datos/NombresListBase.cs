using System;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using TorneosV2.Data;
using TorneosV2.Modelos;
using static TorneosV2.Modelos.MyFunc;

namespace TorneosV2.Pages.Datos
{
	public class NombresListBase : ComponentBase
	{
        public const string TBita = "Listado de Nombres";

        [Parameter]
        public bool Administracion { get; set; } = false;
        [Inject]
        public Repo<ZConfig, ApplicationDbContext> ZConfigRepo { get; set; } = default!;
        [Inject]
        public Repo<Z100_Org, ApplicationDbContext> OrgRepo { get; set; } = default!;
        [Inject]
        public Repo<Z300_Nombres, ApplicationDbContext> NombreRepo { get; set; } = default!;
        [Inject]
        public Repo<Z390_Pais, ApplicationDbContext> PaisRepo { get; set; } = default!;

        protected List<KeyValuePair<string, string>> Sexos { get; set; } =
            new List<KeyValuePair<string, string>>();

        protected List<Z390_Pais> Nacionalidades { get; set; } = new List<Z390_Pais>();
        protected List<Z300_Nombres> LosNombres { get; set; } = new List<Z300_Nombres  >();


        public RadzenDataGrid<Z300_Nombres>? NombresGrid { get; set; } =
                                        new RadzenDataGrid<Z300_Nombres>();

        
        protected bool Primera { get; set; } = true;
        protected bool Leyendo { get; set; } = false;
        protected bool Editando { get; set; } = false;

        //protected bool AddFormShow { get; set; } = false;
        //protected bool BotonNuevo { get; set; } = false;
        //protected string BtnNewText { get; set; } = "Nuevo servicio";
        public string Msn { get; set; } = "";


        protected override async Task OnInitializedAsync()
        {
            if (Primera)
            {
                await Leer();
                await LeerNombres();
                Primera = false;
            }
        }

        protected async Task Leer()
        {
            string[] sexoArg = Constantes.Sexo.Split(",");
            foreach (var l in sexoArg)
            {
                Sexos.Add(new KeyValuePair<string, string>(l.Substring(0, 1), l));
            }

            var nacionTmp = await PaisRepo.Get(x => x.Status == true);
            nacionTmp = nacionTmp.OrderBy(x => x.Favorito).ThenBy(y => y.Corto);
            Nacionalidades = nacionTmp.Any() ? nacionTmp.ToList() : Nacionalidades;
        }

        protected async Task LeerNombres()
        {
            Leyendo = true;
            var nomTmp = await NombreRepo.Get(x => x.Status == (ElUser.Nivel > 5 ? x.Status : true));
            LosNombres = nomTmp.Any() ? nomTmp.ToList() : LosNombres;
            Leyendo = false;
        }

        protected async Task<ApiRespuesta<Z300_Nombres>> Servicio(ServiciosTipos tipo, Z300_Nombres nomb)
        {
            ApiRespuesta<Z300_Nombres> resp = new() { Exito = false };

            if (tipo == ServiciosTipos.Insert)
            {
                Z300_Nombres nomNew = await NombreRepo.Insert(nomb);
                if (nomNew != null)
                {
                    resp.Exito = true;
                    resp.Data = nomNew;
                }
            }
            if (tipo == ServiciosTipos.Update)
            {
                Z300_Nombres nomUpdated = await NombreRepo.Update(nomb);
                if (nomUpdated != null)
                {
                    resp.Exito = true;
                    resp.Data = nomUpdated;
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

