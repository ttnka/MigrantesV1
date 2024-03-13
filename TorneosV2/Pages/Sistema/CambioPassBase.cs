
using System;
using System.Text;
using MathNet.Numerics.Distributions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using NPOI.SS.Formula.Functions;
using Radzen;
using Radzen.Blazor;
using TorneosV2.Data;
using TorneosV2.Modelos;
using static TorneosV2.Pages.Sistema.EntradaBase;

namespace TorneosV2.Pages.Sistema
{
    public class CambioPassBase : ComponentBase
	{
        public const string TBita = "Cambio de Password";

        [Inject]
        public UserManager<IdentityUser> UManager { get; set; } = default!;
        [Inject]
        public Repo<Z110_User, ApplicationDbContext> UserRepo { get; set; } = default!;
        [Inject]
        public Repo<Z100_Org, ApplicationDbContext> OrgRepo { get; set; } = default!;

        [Parameter]
        public string Code { get; set; } = "Vacio";
        [Parameter]
        public string UserId { get; set; } = "Vacio";
        [Parameter]
        public string T { get; set; } = "Vacio";
        public PassClase PassData { get; set; } = new();
        public IdentityUser Usuario { get; set; } = default!;
        protected string Msn { get; set; } = "";
        protected bool Primera { get; set; } = true;
        public RadzenTemplateForm<PassClase>? PassForm { get; set; } = new RadzenTemplateForm<PassClase>();

        protected override async Task OnInitializedAsync()
        {
            if (Primera)
            {
                Primera = false;
                
            }
            
        }

        protected override async Task OnParametersSetAsync()
        {
            if (Code == null || Code == "Vacio" || Code.Length < 10 ||
                    UserId == null || UserId == "Vacio" || UserId.Length < 10) NM.NavigateTo("/", true);
            Usuario = await UManager.FindByIdAsync(UserId);
            if (Usuario == null) NM.NavigateTo("/", true);

            PassData.Email = Usuario!.Email!;
        }

        public async Task PassF(PassClase data)
        {
            try
            {
                string ElCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(Code));
                var resp = await UManager.ResetPasswordAsync(Usuario!, ElCode, data.Pass);
                if (resp.Succeeded)
                {
                    Z110_User elUserT = await UserRepo.GetById(Usuario!.Id);
                    if (elUserT.Estado == 3)
                    {
                        elUserT.Estado = 1;
                        await UserRepo.Update(elUserT);
                    }
                    Z190_Bitacora bitT = new(elUserT.UserId, $"Se cambio de password {TBita}", elUserT.OrgId);
                    Z100_Org orgTmp = await OrgRepo.GetById(elUserT.OrgId);
                    BitacoraMas(bitT);
                    await BitacoraWrite();
                }
                else
                {
                    throw new Exception();
                }
                    NM.NavigateTo("/exitocambiopass", true);
            }
            catch (Exception ex)
            {
                Z192_Logs logT = new("Sistema_User",
                    $"Error al intentar un cambio de password {TBita} : {ex}", false);
                await LogAll(logT);
            }
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

        public class PassClase
		{
            public string Email { get; set; } = "";
			public string Pass { get; set; } = "";
			public string Confirm { get; set; } = "";
		}
	}
}

