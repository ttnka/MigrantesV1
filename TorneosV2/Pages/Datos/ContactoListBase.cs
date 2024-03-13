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
        

        // EVENT CALLBACK
        [Parameter]
        public EventCallback<Z300_Nombres> ReadCEContactos { get; set; }
        [Parameter]
        public EventCallback ReadCETipos { get; set; }
        [Parameter]
        public EventCallback ReadCEPaises { get; set; }

        // Servicios Insertados        
        [Inject]
        public Repo<Z302_Contactos, ApplicationDbContext> ContactoRepo { get; set; } = default!;

        // Listado de Valores
        [Parameter]
        public Z300_Nombres ElNombre { get; set; } = new();
        [Parameter]
        public List<Z302_Contactos> LosCVContactos { get; set; } = new List<Z302_Contactos>();
        [Parameter]
        public  List<ZConfig> LosCVTipos { get; set; } = new List<ZConfig>();
        [Parameter]
        public List<Z390_Pais> LosCVPaises { get; set; } = new List<Z390_Pais>();

        [Parameter]
        public int IndexTabC { get; set; }
        [Parameter]
        public bool ShowDataC { get; set; }

        public RadzenDataGrid<Z302_Contactos>? ContactosGrid { get; set; } =
                                        new RadzenDataGrid<Z302_Contactos>();

        protected bool Primera { get; set; } = true;
        protected bool Editando { get; set; } = false;
        public string Msn { get; set; } = "";

        protected override async Task OnInitializedAsync()
        {
            if (Primera)
            {
                Primera = false;
                FiltroTipos();
                Z190_Bitacora bitaT = new(ElUser.UserId,
                    $"{TBita}, se consulto el listado de contacto ", ElUser.OrgId);
                BitacoraMas(bitaT);

            }
        }

        protected async Task DetReadConstacos()
        {
            await ReadCEContactos.InvokeAsync(ElNombre);
            await ReadCETipos.InvokeAsync();
            await ReadCEPaises.InvokeAsync();
            FiltroTipos();
        }

        protected void FiltroTipos()
        {
            LosCVTipos = LosCVTipos.Where(x => x.Grupo == "Contactos" &&
                                x.Tipo == "Elemento" && x.Status == true).ToList();
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

        public string Recortar(string tipoId, string valor)
        {
            string resp = valor.Length > 75 ? valor.Substring(0, 71) + " ..." : valor;
            if (LosCVTipos.Any(x => x.Titulo.Contains("Tel")) && resp.Length == 10)
            {
                string num = "";
                for (var i = 0; i < resp.Length; i++)
                {
                    num += i == 0 ? "(" : "";
                    num += i == 3 ? ") " : "";
                    num += i == 6 ? "-" : "";
                    num += resp.Substring(i, 1);
                }
                resp = num;
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

