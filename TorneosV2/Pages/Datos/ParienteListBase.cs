using System;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using TorneosV2.Data;
using TorneosV2.Modelos;
using static TorneosV2.Modelos.MyFunc;

namespace TorneosV2.Pages.Datos
{
	public class ParienteListBase : ComponentBase
	{
        public const string TBita = "Listado de parientes";
        


        [Inject]
        public Repo<Z300_Nombres, ApplicationDbContext> NombreRepo { get; set; } = default!;

        [Inject]
        public Repo<Z309_Pariente, ApplicationDbContext> ParienteRepo { get; set; } = default!;

        [Parameter]
        public Z300_Nombres ElNombre { get; set; } = default!;

        protected List<KeyValuePair<string, string>> Parentescos { get; set; } =
            new List<KeyValuePair<string, string>>();
        protected List<Z309_Pariente> LosParientes { get; set; } = new List<Z309_Pariente>();
        protected List<Z300_Nombres> LosNombresAll { get; set; } = new List<Z300_Nombres>();
        protected List<Z300_Nombres> LosActivos { get; set; } = new List<Z300_Nombres>();

        public RadzenDataGrid<Z309_Pariente>? ParientesGrid { get; set; } =
                                        new RadzenDataGrid<Z309_Pariente>();

        protected List<Z190_Bitacora> LasBitacoras { get; set; } = new List<Z190_Bitacora>();
        protected bool Primera { get; set; } = true;
        protected bool Editando { get; set; } = false;
        public string Msn { get; set; } = "";

        protected override async Task OnInitializedAsync()
        {
            if (Primera)
            {
                Primera = false;
                Z190_Bitacora bitaT = new(ElUser.UserId, $"{TBita} se consulto el listado de parientes", ElUser.OrgId);
                BitacoraMas(bitaT);
            }
        }

        protected async Task Leer()
        {
            string[] pArre = Constantes.Parentescos.Split(",");
            bool par = true;
            for (var i = 0; i < pArre.Length; i++)
            {
                string txt = par ? pArre[i + 1] : pArre[i - 1];
                Parentescos.Add(new KeyValuePair<string, string>
                    (pArre[i], pArre[i] + " - " + txt ));
                par = !par;
            }
        }

        protected async Task LeerParientes()
        {
            var pareTmp = await ParienteRepo.Get(x => x.NombreId == ElNombre.NombreId ||
                            x.ParienteId == ElNombre.NombreId);
            LosParientes = pareTmp.Any(x => x.NombreId == ElNombre.NombreId) ?
                            pareTmp.Where(x => x.NombreId == ElNombre.NombreId).ToList() : LosParientes;
            var nombresAll = await NombreRepo.GetAll();
            LosNombresAll = nombresAll.Any() ? nombresAll.ToList() : LosNombresAll;

            LosActivos = nombresAll.Any(x => x.Status == true && x.NombreId != ElNombre.NombreId &&
                            !LosParientes.Any(z=>z.NombreId == x.NombreId)) ?
                        nombresAll.Where(x => x.Status == true && x.NombreId != ElNombre.NombreId &&
                            !LosParientes.Any(z=>z.NombreId == x.NombreId)).ToList() : LosActivos;
        }

        protected async Task<ApiRespuesta<Z309_Pariente>> Servicio(ServiciosTipos tipo, Z309_Pariente pari)
        {
            ApiRespuesta<Z309_Pariente> resp = new() { Exito = false };

            if (tipo == ServiciosTipos.Insert)
            {
                pari.NombreAdd(ElNombre);
                Z309_Pariente pariNew = await ParienteRepo.Insert(pari);
                if (pariNew != null)
                {
                    resp.Exito = true;
                    resp.Data = pariNew;
                }
            }
            if (tipo == ServiciosTipos.Update)
            {
                Z309_Pariente pariUpdated = await ParienteRepo.Update(pari);
                if (pariUpdated != null)
                {
                    resp.Exito = true;
                    resp.Data = pariUpdated;
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
        public void BitacoraMas(Z190_Bitacora bita)
        {
            if (!LasBitacoras.Any(b => b.BitacoraId == bita.BitacoraId))
            {
                bita.OrgAdd(ElUser.Org);
                LasBitacoras.Add(bita);
            }
        }
        public async Task BitacoraWrite()
        {
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

