using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Radzen;
using TorneosV2.Data;
using TorneosV2.Modelos;

namespace TorneosV2.Pages.WebApp
{
	public class IndexApp2Base : ComponentBase 
	{
        public const string TBita = "Index app";

        // Servicios Insertados
        [Inject]
        public Repo<Z100_Org, ApplicationDbContext> OrgRepo { get; set; } = default!;
        [Inject]
        public Repo<Z110_User, ApplicationDbContext> UserRepo { get; set; } = default!;
        [Inject]
        public Repo<Z300_Nombres, ApplicationDbContext> NombreRepo { get; set; } = default!;
        [Inject]
        public Repo<Z380_Servicios, ApplicationDbContext> ServiciosRepo { get; set; } = default!;
        [Inject]
        public Repo<Z390_Pais, ApplicationDbContext> PaisRepo { get; set; } = default!;
        [Inject]
        public Repo<ZConfig, ApplicationDbContext> ZConfigRepo { get; set; } = default!;
        

        // Listado de variables
        public List<Z100_Org> LasOrgsAll { get; set; } = new List<Z100_Org>();
        public List<Z300_Nombres> LosNombresAll { get; set; } = new List<Z300_Nombres>();
        public List<Z380_Servicios> LosServiciosAll { get; set; } = new List<Z380_Servicios>();
        public List<Z390_Pais> LosPaisesAll { get; set; } = new List<Z390_Pais>();
        public List<ZConfig> LosConfigsAll { get; set; } = new List<ZConfig>();

        protected List<Z190_Bitacora> LasBitacoras { get; set; } = new List<Z190_Bitacora>();
        protected bool Primera { get; set; } = true;
        protected bool Editando { get; set; } = false;
        public int IndexTabAll { get; set; } = 0;
        

        protected override async Task OnInitializedAsync()
        {
            if (Primera)
            {
                Primera = false;
                await LeerElUser();
     
                await ReadPaisesAll();
                await ReadLasOrgsAll();
                await ReadLosNombresAll();
                await ReadLosServiciosAll();
                await ReadLosConfigsAll();

                Z190_Bitacora bitaTmp = new(ElUser.UserId, $"{TBita}, Se entro IndexApp ", ElUser.OrgId);
                BitacoraMas(bitaTmp);
                await BitacoraWrite();
            }
        }
        
        public async Task ReadLasOrgsAll()
        {
            try
            {
                var oTmp = await OrgRepo.GetAll();
                LasOrgsAll = oTmp.Any() ? oTmp.OrderBy(x => x.Comercial).ToList() : new List<Z100_Org>();
            }
            catch (Exception ex)
            {
                Z192_Logs lTmp = new("Sistema_user", $"{TBita}, No fue posible leer listado de las organizaciones {ex}", false);
                await LogAll(lTmp);
            }
        }

        public async Task ReadLosNombresAll()
        {
            try
            {
                var nTmp = await NombreRepo.Get(x => x.Status == (ElUser.Nivel > 5 ? x.Status : true));
                LosNombresAll = nTmp.Any() ? nTmp.OrderBy(x => x.Paterno).ThenBy(y => y.Materno).
                                                    ThenBy(z => z.Nombre).ToList() : LosNombresAll;
            }
            catch (Exception ex)
            {
                Z192_Logs lTmp = new("Sistema_user", $"{TBita}, No fue posible leer listado de Nombres {ex}", false);
                await LogAll(lTmp);
            }
        }

        public async Task ReadLosServiciosAll()
        {
            try
            {
                var servTmp = await ServiciosRepo.Get(x => x.OrgId == (ElUser.Nivel < 6 ?
                    ElUser.OrgId : x.OrgId));
                if (servTmp.Any())
                {
                    LosServiciosAll = servTmp.ToList();
                    if (!LasOrgsAll.Any()) return;

                    LosServiciosAll.ForEach(x =>
                    {
                        var o = LasOrgsAll.FirstOrDefault(z => z.OrgId == x.OrgId);
                        if (o != null) { x.OrgAdd(o); }
                    });
                }

            }
            catch (Exception ex)
            {
                Z192_Logs lTmp = new("Sistema_user", $"{TBita}, No fue posible leer listado de Servicios {ex}", false);
                await LogAll(lTmp);
            }
        }

        public async Task ReadPaisesAll()
        {
            try
            {
                var rTmp = await PaisRepo.Get(x => x.Status == (ElUser.Nivel < 5 ? true : x.Status));
                LosPaisesAll = rTmp.Any() ? rTmp.ToList() : new List<Z390_Pais>();
            }
            catch (Exception ex)
            {
                Z192_Logs lTmp = new("Sistema_user", $"{TBita}, No fue posible leer listado de paises {ex}", false);
                await LogAll(lTmp);
            }

        }

        public async Task ReadLosConfigsAll()
        {
            try
            {
                var confTmp = await ZConfigRepo.GetAll();
                LosConfigsAll = !confTmp.Any() ? new List<ZConfig>() :
                    confTmp.OrderBy(x => x.Grupo).ThenBy(x => x.Tipo).ToList();

            }
            catch (Exception ex)
            {
                Z192_Logs lTmp = new("Sistema_user", $"{TBita}, No fue posible leer listado de las etiquetas {ex}", false);
                await LogAll(lTmp);
            }
        }

        

        #region Usuario y Bitacora

        [CascadingParameter(Name = "ElUserAll")]
        public Z110_User ElUser { get; set; } = default!;
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
            try
            {
                if (log.LogId != LastLog.LogId)
                {
                    LastLog = log;
                    await LogRepo.Insert(log);
                }
            }
            catch (Exception ex)
            {
                Z192_Logs LogT = new(ElUser.UserId,
                    $"Error al intentar escribir BITACORA, {TBita},{ex}", true);
                await LogAll(LogT);
            }
        }
        #endregion

        #region Preguntar USER
        [Inject]
        public UserManager<IdentityUser> UserMger { get; set; } = default!;

        [CascadingParameter]
        public Task<AuthenticationState> AuthStateTask { get; set; } = default!;

        public async Task LeerElUser()
        {
            try
            {
                ClaimsPrincipal user = (await AuthStateTask).User;
                if (user != null && user.Identity != null && user.Identity.IsAuthenticated)
                {
                    string elId = UserMger.GetUserId(user) ?? "Vacio";
                    if (elId == null || elId == "Vacio") NM.NavigateTo("Identity/Account/Login?ReturnUrl=/", true);
                    var resp = await UserRepo.Get(x => x.UserId == elId, propiedades: "Org");
                    //Niveles = "1Registrado,2Proveedor,3Cliente,4Cliente_Admin,5Alija,6Alija_Admin";
                    if (resp == null || !resp.Any())
                    {
                        NM.NavigateTo("Identity/Account/Login?ReturnUrl=/", true);
                    }

                    else
                    {
                        ElUser = resp.FirstOrDefault()!;
                        LaOrg = ElUser.Org;
                    }
                }
                else
                {
                    NM.NavigateTo("Identity/Account/Login?ReturnUrl=/", true);
                }

            }
            catch (Exception ex)
            {
                Z192_Logs LogT = new("Sistema_user",
                    $"Error al intentar leer EL USER USUARIO, {TBita},{ex}", true);
                await LogAll(LogT);
            }
        }

        #endregion

    }
}

