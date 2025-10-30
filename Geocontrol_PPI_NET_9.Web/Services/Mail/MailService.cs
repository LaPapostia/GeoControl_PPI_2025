using Geocontrol_PPI_NET_9.Models.Mail;
using Geocontrol_PPI_NET_9.Web.Tools.Mail;
using System.Collections.ObjectModel;
using System.Security.Cryptography;

namespace Geocontrol_PPI_NET_9.Web.Services.Mail
{
    public class MailService
    {
        /// <summary>
        /// Method created for the send of the email verificaton code
        /// </summary>
        /// <param name="identification"></param>
        /// <param name="code"></param>
        /// <param name="expirationDate"></param>
        /// <returns></returns>
        public async Task<(bool status, string message)> SendEmail(string identification, string code, DateTime expirationDate)
        {
            try
            {
                var note = string.Empty;

                var emailModel = new EmailTemplate
                {
                    Subject = $"Geocontrol - Cambio Contraseña",
                    Title = $"Autorización de Cambio de Contraseña",
                    Greeting = "Estimado usuario,",
                    MainContent = $"Se solicitó un cambio de contraseña para el aplicativo de Geocontrol a las: {DateTime.Now:dd-MM-yyyy hh:mm:ss}",
                    HighlightCode = code,
                    Instructions = "Por favor utilice este código para autorizar la transacción: \n" +
                    "IMPORTANTE: Si usted no fue la persona que lo solicitó por favor haga caso omiso al mensaje y no comparta el código con nadie.",
                    FooterNotice = "Este es un mensaje automático, por favor no responda a este correo.",
                    CompanyName = "Aplicativo de Gestión de Localidad - Geocontrol",
                    ExpirationDate = expirationDate.ToString("dd-MM-yyyy hh:mm:ss"),
                    Remitter = "Aplicativo de Gestión de Localidad - Geocontrol",
                    Note = note
                };

                var (isSuccess, errorMessage) = await EmailTemplated.SendTemplatedEmail
                (
                    ["juanes041387@gmail.com"],
                    [],
                    emailModel
                );

                if (isSuccess)
                {
                    //await BillingDocumentRepository.InsertAuthorizationCode(Consecutive, documenttypecode, authorizationCode, emailModel.ExpirationDate.Value);

                    return (true, $"Correo enviado correctamente. El código expira a las {emailModel.ExpirationDate:HH:mm}");
                }
                else
                {
                    return (false, errorMessage);
                }
            }
            catch (Exception ex)
            {
                return (false, $"Ocurrió un error inesperado: {ex.Message}");
            }
        }

    }
}
