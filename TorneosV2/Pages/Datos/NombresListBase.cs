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
        

        // --- EVENTCALL BACK
        
        [Parameter]
        public EventCallback ReadNEConfig { get; set; }
        [Parameter]
        public EventCallback ReadNEPaises { get; set; }
        [Parameter]
        public EventCallback ReadNENombres { get; set; }
        
        
        

        // Servicios Insertados
        
        [Inject]
        public Repo<Z300_Nombres, ApplicationDbContext> NombreRepo { get; set; } = default!;
        [Inject]
        public Repo<Z302_Contactos, ApplicationDbContext> ContactoRepo { get; set; } = default!;
        [Inject]
        public Repo<Z304_Domicilio, ApplicationDbContext> DomicilioRepo { get; set; } = default!;
        [Inject]
        public Repo<Z309_Pariente, ApplicationDbContext> ParienteRepo { get; set; } = default!;

        // Listado de valores
        [Parameter]
        public List<Z390_Pais> LosPaises { get; set; } = new List<Z390_Pais>();
        [Parameter]
        public List<Z300_Nombres> LosNombres { get; set; } = new List<Z300_Nombres>();
        [Parameter]
        public List<Z302_Contactos> LosContactosAll { get; set; } = new List<Z302_Contactos>();
        [Parameter]
        public List<Z304_Domicilio> LosDomiciliosAll { get; set; } = new List<Z304_Domicilio>();
        [Parameter]
        public List<Z309_Pariente> LosParientesAll { get; set; } = new List<Z309_Pariente>();
        [Parameter]
        public List<ZConfig> LosConfigs { get; set; } = new List<ZConfig>();

        protected List<KeyValuePair<string, string>> Sexos { get; set; } =
            new List<KeyValuePair<string, string>>();

        
        public RadzenDataGrid<Z300_Nombres>? NombresGrid { get; set; } =
                                        new RadzenDataGrid<Z300_Nombres>();

        protected List<Z190_Bitacora> LasBitacoras { get; set; } = new List<Z190_Bitacora>();
        protected bool Primera { get; set; } = true;
        protected bool Editando { get; set; } = false;
        public string Msn { get; set; } = "";
        public bool LeyendoDet { get; set; } = false;
        public int IndexTabNom { get; set; } = 0;
        public Z300_Nombres ElNombre { get; set; } = new();
        public bool ShowData { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            if (Primera)
            {
                Primera = false;
                Leer();
                Z190_Bitacora bitaT = new(ElUser.UserId,
                    $"{TBita}, se consulto el listado de nombres", ElUser.OrgId);
                BitacoraMas(bitaT);
            }
        }

        protected async Task CalMostrarSub(Z300_Nombres elNombre, bool mostrar)
        {
            try
            {
                ShowData = mostrar;
                LeyendoDet = true;
                if (mostrar)
                {
                    ElNombre = elNombre;
                    var dTmp = await DomicilioRepo.Get(x => x.NombreId == elNombre.NombreId &&
                                    x.Status == (ElUser.Nivel < 6 ? true : x.Status));
                    LosDomiciliosAll = !dTmp.Any() ? new List<Z304_Domicilio>() :
                        dTmp.OrderBy(x => x.Pais).ThenBy(x => x.Estado).ThenBy(x => x.Ciudad).ToList();

                    var cTmp = await ContactoRepo.Get(x => x.NombreId == elNombre.NombreId &&
                                    x.Status == (ElUser.Nivel < 6 ? true : x.Status));
                    //var cTmp = await ContactoRepo.GetAll();
                    LosContactosAll = !cTmp.Any() ? new List<Z302_Contactos>() :
                        cTmp.OrderBy(x => x.Tipo).ToList();

                    var pTmp = await ParienteRepo.Get(x => x.NombreId == elNombre.NombreId &&
                                    x.Status == (ElUser.Nivel < 6 ? true : x.Status));
                    LosParientesAll = !pTmp.Any() ? new List<Z309_Pariente>() :
                        pTmp.OrderBy(x=>x.Parentesco).ToList() ;

                    Z190_Bitacora bitaTmp = new(ElUser.UserId, $"{TBita}, Se consulto el detalle de informacion de {elNombre.Completo} ", ElUser.OrgId);
                    BitacoraMas(bitaTmp);
                }
                else
                { elNombre = new(); }
                
            }
            catch (Exception ex)
            {
                Z192_Logs logTmp = new("Sistema_user", $"Error al intentar leer los detalles de {elNombre.Completo} {ex}", false);
                await LogAll(logTmp);
            }
            LeyendoDet = false;

        }

        protected void Leer()
        {
            string[] sexoArg = Constantes.Sexo.Split(",");
            foreach (var l in sexoArg)
            {
                Sexos.Add(new KeyValuePair<string, string>(l.Substring(0, 1), l));
            }
            LosPaises = LosPaises.Any() ? LosPaises.OrderBy(x => x.Favorito).ThenBy(y => y.Corto).ToList() :
                new List<Z390_Pais>();
        }


        protected async Task DetReadNombres()
        {
            await ReadNENombres.InvokeAsync();
        }

        protected async Task DetReadConfig()
        {
            await ReadNEConfig.InvokeAsync();
        }

        protected async Task DetReadPais()
        {
            await ReadNEPaises.InvokeAsync();
        }
 
        public async Task ReadLosContactosAll(Z300_Nombres elNombre)
        {
            try
            {
                var cTmp = await ContactoRepo.Get(x => x.NombreId == elNombre.NombreId);
                LosContactosAll = cTmp.Any() ? cTmp.OrderBy(X => X.Tipo).ToList() : new List<Z302_Contactos>();
            }
            catch (Exception ex)
            {
                Z192_Logs lTmp = new("Sistema_user", $"{TBita}, No fue posible leer datos de contacto de {elNombre.Completo} {ex}", false);
                await LogAll(lTmp);
            }

        }

        public async Task ReadLosDomiciliosAll(Z300_Nombres elNombre)
        {
            try
            {
                var dTmp = await DomicilioRepo.Get(x => x.NombreId == elNombre.NombreId);
                LosDomiciliosAll = dTmp.Any() ? dTmp.ToList() : new List<Z304_Domicilio>();
            }
            catch (Exception ex)
            {
                Z192_Logs lTmp = new("Sistema_user", $"{TBita}, No fue posible leer datos de domicilio de {elNombre.Completo} {ex}", false);
                await LogAll(lTmp);
            }
        }

        public async Task ReadLosParientesAll(Z300_Nombres elNombre)
        {
            try
            {
                var pTmp = await ParienteRepo.Get(x => x.NombreId == ElNombre.NombreId);
                LosParientesAll = pTmp.Any() ? pTmp.ToList() : new List<Z309_Pariente>();
            }
            catch (Exception ex)
            {
                Z192_Logs lTmp = new("Sistema_user", $"{TBita}, No fue posible leer datos de parientes de {elNombre.Completo} {ex}", false);
                await LogAll(lTmp);
            }
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

