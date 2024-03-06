using System;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using TorneosV2.Data;
using TorneosV2.Modelos;
using static TorneosV2.Modelos.MyFunc;

namespace TorneosV2.Pages.Datos
{
	public class ContactoListBase : ComponentBase
	{
        public const string TBita = "Listado de datos de contacto";

        [Parameter]
        public bool Administracion { get; set; } = false;

        [Inject]
        public Repo<ZConfig, ApplicationDbContext> ZConfigRepo { get; set; } = default!;
        
        [Inject]
        public Repo<Z302_Contactos, ApplicationDbContext> ContactoRepo { get; set; } = default!;

        [Parameter]
        public Z300_Nombres ElNombre { get; set; } = default!;

        protected List<Z302_Contactos> LosContactos { get; set; } = new List<Z302_Contactos>();
        protected List<ZConfig> LosTipos { get; set; } = new List<ZConfig>();

        public RadzenDataGrid<Z302_Contactos>? ContactosGrid { get; set; } =
                                        new RadzenDataGrid<Z302_Contactos>();


        protected bool Primera { get; set; } = true;
        protected bool Leyendo { get; set; } = false;
        protected bool Editando { get; set; } = false;

        
        public string Msn { get; set; } = "";


        protected override async Task OnInitializedAsync()
        {
            if (Primera)
            {
                await Leer();
                await LeerContactos();
                Primera = false;
            }
        }

        protected async Task Leer()
        {
            /*
            string[] sexoArg = Constantes.Sexo.Split(",");
            foreach (var l in sexoArg)
            {
                Sexos.Add(new KeyValuePair<string, string>(l.Substring(0, 1), l));
            }

            var nacionTmp = await PaisRepo.Get(x => x.Status == true);
            nacionTmp = nacionTmp.OrderBy(x => x.Favorito).ThenBy(y => y.Corto);
            Nacionalidades = nacionTmp.Any() ? nacionTmp.ToList() : Nacionalidades;
            */
            var tipoTmp = await ZConfigRepo.Get(x => x.Grupo == "Contactos" && x.Tipo == "Tipo" && x.Status == true);
            LosTipos = tipoTmp.Any() ? tipoTmp.ToList() : LosTipos;
        }

        protected async Task LeerContactos()
        {
            Leyendo = true;
            var contTmp = await ContactoRepo.Get(x => x.NombreId == ElNombre.NombreId);
            LosContactos = contTmp.Any() ? contTmp.ToList() : LosContactos;
            Leyendo = false;
        }

        protected async Task<ApiRespuesta<Z302_Contactos>> Servicio(ServiciosTipos tipo, Z302_Contactos cont)
        {
            ApiRespuesta<Z302_Contactos> resp = new() { Exito = false };

            if (tipo == ServiciosTipos.Insert)
            {   
                cont.NombreAdd(ElNombre);
                Z302_Contactos contNew = await ContactoRepo.Insert(cont);
                if (contNew != null)
                {
                    resp.Exito = true;
                    resp.Data = contNew;
                }
            }
            if (tipo == ServiciosTipos.Update)
            {
                Z302_Contactos nomUpdated = await ContactoRepo.Update(cont);
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

