using System;
using MailKit.Net.Smtp;
using MimeKit;

namespace TorneosV2.Modelos
{
    public class REnviarMail : IEnviarMail
    {
        public async Task<ApiRespuesta<MailCampos>> EnviarMail(MailCampos mC, bool standar)
        {
            ApiRespuesta<MailCampos> respuesta = new();

            #region EVALUAR Info de envio y cuentas
            if (mC == null)
            {
                respuesta.Exito = false;
                respuesta.MsnError.Add("No hay datos para enviar mail!");
                return respuesta;
            }
            if (!standar && string.IsNullOrEmpty(mC.SenderEmail))
                respuesta.MsnError.Add("No hay direccion de envio Sender");


            if (string.IsNullOrEmpty(mC.Titulo))
                respuesta.MsnError.Add("No hay titulo del mail!");

            if (string.IsNullOrEmpty(mC.Cuerpo))
                respuesta.MsnError.Add("No hay cuerpo del mail");

            if (respuesta.MsnError.Count > 0)
            {
                string texto = "";
                foreach (var me in respuesta.MsnError)
                {
                    texto += me.ToString() + " ";
                }
                return respuesta;
            }

            #endregion

            #region Evaluar si es correo de pruebas

            foreach (var m in mC.ParaEmail)
            {
                if (m.EndsWith(".com1") || m == Constantes.DeMail01Soporte)
                {
                    respuesta.Exito = true;
                    respuesta.Texto = "Email de prueba exitos!";

                    return respuesta;
                }
            }

            #endregion

            try
            {
                var message = new MimeMessage();

                message.From.Add(new MailboxAddress(Constantes.DeNombreMail01Soporte, Constantes.DeMail01Soporte));
                for (var i = 0; i < mC.ParaNombre.Count; i++)
                {
                    message.To.Add(new MailboxAddress(mC.ParaNombre[i], mC.ParaEmail[i]));
                }

                message.Subject = mC.Titulo;

                message.Body = new TextPart("html")
                {
                    Text = mC.Cuerpo
                };

                using (var client = new SmtpClient())
                {
                    if (standar)
                    {
                        message.ReplyTo.Add(new MailboxAddress(Constantes.DeNombreMail01Soporte, Constantes.DeMail01Soporte));  
                        mC.SenderName = Constantes.DeNombreMail01Soporte;
                        mC.SenderEmail = Constantes.DeMail01Soporte;
                        mC.Server = Constantes.ServerMail01;
                        mC.Port = Constantes.PortMail01;
                        mC.UserName = Constantes.UserNameMail01;
                        mC.Password = Constantes.PasswordMail01;
                    }
                    
                    await client.ConnectAsync(mC.Server, mC.Port, false);
                    await client.AuthenticateAsync(mC.UserName, mC.Password);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }

                respuesta.Exito = true;
                respuesta.Data = mC;

            }
            catch (Exception ex)
            {
                respuesta.MsnError.Add(ex.Message);
                respuesta.Exito = false;
                string text = $"Hubo un error al enviar MAIL {ex} Para {mC.ParaNombre} {mC.ParaEmail} ";
                respuesta.MsnError.Add(text);

            }
            respuesta.Exito = !respuesta.MsnError.Any();
            return respuesta;

        }

    }
}

