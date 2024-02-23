using System;
namespace TorneosV2.Modelos
{
    public interface IEnviarMail
    {
        Task<ApiRespuesta<MailCampos>> EnviarMail(MailCampos mailCampos, bool standar);
    }
}

