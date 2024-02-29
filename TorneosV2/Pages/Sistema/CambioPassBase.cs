
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
        [Parameter]
        public string Code { get; set; } = "Vacio";
        public PassClase PassData { get; set; } = new();

        protected string Msn { get; set; } = "";
        protected bool Primera { get; set; } = true;
        public RadzenTemplateForm<PassClase>? PassForm { get; set; } = new RadzenTemplateForm<PassClase>();

        protected override async Task OnInitializedAsync()
        {
            if (Primera)
            {
                Primera = false;
                if (Code == "Vacio" || Code.Length < 10) NM.NavigateTo("/", true);
            }
        }

        public async Task PassF(PassClase data)
        {
            try
            {
                var Usuario = await UManager.FindByEmailAsync(PassData.Email);
                if (Usuario == null) NM.NavigateTo("/", true);

                string ElCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(Code));
                
                var resp = await UManager.ResetPasswordAsync(Usuario!, ElCode, PassData.Pass);
                if (resp.Succeeded)
                {
                    Z110_User elUserT = await UserRepo.GetById(Usuario!.Id);
                    Z190_Bitacora bitT = new(elUserT.UserId, $"Se cambio de password {TBita}", elUserT.OrgId);
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
            try
            {
                if (bita.BitacoraId != LastBita.BitacoraId)
                {
                    LastBita = bita;
                    await BitaRepo.Insert(bita);
                }
            }
            catch (Exception ex)
            {
                Z192_Logs LogT = new(ElUser.UserId,
                    $"Error al intentar escribir BITACORA, {TBita},{ex}", true);
                await LogAll(LogT);
            }
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

        public class PassClase
		{
            public string Email { get; set; } = "";
			public string Pass { get; set; } = "";
			public string Confirm { get; set; } = "";
		}
	}
}

