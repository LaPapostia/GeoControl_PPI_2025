using Geocontrol_PPI_NET_9.Models.Mail;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Geocontrol_PPI_NET_9.Web.Tools.Mail
{
    public class EmailTemplated
    {

        public static async Task<(bool isSuccess, string errorMessage)> SendTemplatedEmail(List<string> toEmails, List<string> CCMails, EmailTemplate templateModel)
        {
            try
            {
                EmailApp emailSendEncrypted = new()
                {
                    Mail = "lepaposie@gmail.com",
                    Pass = "nwkh jzyb pyom jkyy"
                };

                var invalidEmails = toEmails.Where(email => !IsValidEmail(email)).ToList();

                if (invalidEmails.Count != 0)
                {
                    return (false, $"Los siguientes correos no son válidos: {string.Join(", ", invalidEmails)}");
                }


                string htmlBody = string.Empty;

                htmlBody = GenerateGenericTemplate(templateModel);

                CCMails ??= [];

                using (MailMessage mail = new())
                {
                    mail.From = new MailAddress(emailSendEncrypted.Mail);
                    foreach (var email in toEmails)
                    {
                        mail.To.Add(email);
                    }
                    foreach (var ccmail in CCMails)
                    {
                        mail.CC.Add(ccmail);
                    }
                    mail.Subject = templateModel.Subject;
                    mail.Body = htmlBody;
                    mail.IsBodyHtml = true;

                    using SmtpClient smtp = new("smtp.gmail.com", 587);
                    smtp.Credentials = new NetworkCredential(emailSendEncrypted.Mail, emailSendEncrypted.Pass);
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(mail);
                }

                return (true, string.Empty);
            }
            catch (SmtpFailedRecipientException ex)
            {
                return (false, $"El correo no pudo ser entregado a algunos destinatarios: {ex.Message}");
            }
            catch (SmtpException ex)
            {
                return (false, $"Error del servidor de correo: {ex.Message}");
            }
            catch (Exception ex)
            {
                return (false, $"Error al enviar el correo: {ex.Message}");
            }
        }

        private static string GenerateGenericTemplate(EmailTemplate model, object details = null)
        {
            string primaryColorHex = "#c3494d";

            // 1. Construcción modular de secciones
            var sections = new List<string>
            {
                // Encabezado
                $@"
    <div style='color:{primaryColorHex}; border-bottom:2px solid {primaryColorHex}; padding-bottom:10px; margin-bottom:20px;'>
        <h2 style='margin:0;'>{model.Title}</h2>
        <p style='margin:5px 0 0;'>Fecha: {DateTime.Now.ToString("dd/MM/yyyy")}</p>
    </div>"
            };

            // Saludo
            if (!string.IsNullOrEmpty(model.Greeting))
                sections.Add($"<p style='margin:15px 0;'>{model.Greeting}</p>");

            // Contenido principal
            if (!string.IsNullOrEmpty(model.MainContent))
                sections.Add($"<p style='margin:15px 0;'>{model.MainContent}</p>");

            // Instrucciones (si existen)
            if (!string.IsNullOrEmpty(model.Instructions))
                sections.Add($@"
        <div style='background:#f8f9fa; padding:15px; border-left:3px solid {primaryColorHex}; border-radius:4px; margin:15px 0;'>
            <h4 style='color:{primaryColorHex}; margin-top:0;'>Instrucciones</h4>
            <p>{model.Instructions}</p>
        </div>");

            // Contenido destacado
            if (!string.IsNullOrEmpty(model.HighlightedContent) || !string.IsNullOrEmpty(model.HighlightCode))
            {
                string highlight = model.HighlightedContent ?? model.HighlightCode?.ToString();
                sections.Add($@"
        <div style='background:#f5f5f5; border:1px solid #ddd; border-radius:4px; padding:15px; margin:20px 0; text-align:center; font-size:24px; font-weight:bold; color:#2c3e50;'>
            {highlight}
        </div>");
            }

            // Vigencia (si aplica)
            if (model.ExpirationDate is not null)
                sections.Add($@"
                    <div style='background:#f8f9fa; padding:15px; border-left:3px solid {primaryColorHex}; border-radius:4px; margin:15px 0;'>
                        <h4 style='color:{primaryColorHex}; margin-top:0;'>Vigencia</h4>
                        <p>Válido hasta: {model.ExpirationDate}</p>
                    </div>");


            // Tabla de detalles (si hay datos)
            string tableContent = GenerateGenericTableContent(details, primaryColorHex);
            if (!string.IsNullOrEmpty(tableContent))
                sections.Add($@"
        <div style='margin:20px 0;'>
            <h4 style='color:{primaryColorHex}; margin-bottom:10px;'>Detalles:</h4>
            <div style='overflow-x:auto;'>
                {tableContent}
            </div>
        </div>");

            // Nota adicional (si existe)
            if (!string.IsNullOrEmpty(model.Note))
                sections.Add($"<p style='font-size:14px; color:#666; margin:10px 0;'><em>{model.Note}</em></p>");

            // 2. Construcción del HTML final
            return $@"
    <!DOCTYPE html>
    <html>
    <head>
        <meta http-equiv='Content-Type' content='text/html; charset=utf-8'>
        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
        <title>{model.Title}</title>
        <style>
            body {{
                font-family: Arial, sans-serif;
                line-height: 1.6;
                color: #333;
                background-color: #f9f9f9;
                margin: 0;
                padding: 20px;
            }}
            .email-container {{
                margin: 0 auto;
                padding: 25px;
                background-color: white;
                box-shadow: 0 2px 10px rgba(0,0,0,0.05);
                border-radius: 8px;
            }}
            table {{
                width: 100%;
                border-collapse: collapse;
                margin: 10px 0;
            }}
            th {{
                background-color: {primaryColorHex};
                color: white;
                padding: 12px;
                text-align: left;
            }}
            td {{
                border: 1px solid #eee;
                padding: 10px;
            }}
        </style>
    </head>
    <body>
        <div class='email-container'>
            {string.Join("\n", sections)}
            
            <div style='margin-top:15px;'>
                <p>Cordialmente,</p>
                <h4 style='color:{primaryColorHex}; margin-bottom:3px;'>{model.Remitter}</h4>
            </div>
            
            <div style='margin-top:20px; font-size:12px; color:#777; border-top:1px solid #eee; padding-top:15px;'>
                <p style='margin:5px 0;'>{model.FooterNotice}</p>
                <p style='margin:5px 0;'>© {DateTime.Now.Year} {model.CompanyName} - Todos los derechos reservados</p>
            </div>
        </div>
    </body>
    </html>";
        }

        private static string GenerateGenericTableContent(object details, string primaryColorHex)
        {
            if (details == null) return string.Empty;

            var tableBuilder = new StringBuilder();
            var enumerable = details as System.Collections.IEnumerable;

            if (enumerable != null)
            {
                bool firstItem = true;

                foreach (var item in enumerable)
                {
                    var properties = item.GetType().GetProperties();

                    if (firstItem)
                    {
                        // Encabezados
                        tableBuilder.Append("<table><thead><tr>");
                        foreach (var prop in properties)
                        {
                            tableBuilder.Append($"<th>{prop.Name}</th>");
                        }
                        tableBuilder.Append("</tr></thead><tbody>");
                        firstItem = false;
                    }

                    // Filas de datos
                    tableBuilder.Append("<tr>");
                    foreach (var prop in properties)
                    {
                        var value = prop.GetValue(item)?.ToString() ?? "N/A";
                        tableBuilder.Append($"<td>{value}</td>");
                    }
                    tableBuilder.Append("</tr>");
                }

                if (!firstItem) // Si se agregaron datos
                {
                    tableBuilder.Append("</tbody></table>");
                }
            }

            return tableBuilder.ToString();
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
