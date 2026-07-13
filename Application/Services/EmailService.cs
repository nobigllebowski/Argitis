using Application.DTOs;
using Application.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;

namespace CreditPortal.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPass;
        private readonly string _fromEmail;
        private readonly string _fromName;

        public EmailService(string smtpServer, int smtpPort, string smtpUser, string smtpPass, string fromEmail, string fromName)
        {
            _smtpServer = smtpServer;
            _smtpPort = smtpPort;
            _smtpUser = smtpUser;
            _smtpPass = smtpPass;
            _fromEmail = fromEmail;
            _fromName = fromName;
        }

        public async Task SendEmailAsync(EmailDto email)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_fromName, _fromEmail));
            message.To.Add(new MailboxAddress(email.ToName, email.ToEmail));
            message.Subject = email.Subject;
            message.MessageId = $"{Guid.NewGuid()}@creditportal.com";

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = email.Body,
                TextBody = StripHtml(email.Body)
            };
            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(_smtpServer, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_smtpUser, _smtpPass);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        public string BuildAdminNotificationEmail(LoanConfirmationDto loan, string clientIp)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background: #28a745; color: white; padding: 20px; text-align: center; border-radius: 10px 10px 0 0; }}
        .content {{ background: #f8f9fa; padding: 30px; border-radius: 0 0 10px 10px; }}
        .section {{ background: white; padding: 20px; border-radius: 10px; margin: 20px 0; }}
        .details-table {{ width: 100%; border-collapse: collapse; margin: 20px 0; }}
        .details-table td {{ padding: 12px; border-bottom: 1px solid #dee2e6; }}
        .details-table td:first-child {{ font-weight: bold; width: 50%; }}
        .footer {{ text-align: center; margin-top: 30px; color: #6c757d; font-size: 14px; }}
        .divider {{ border-top: 1px solid #dee2e6; margin: 20px 0; }}
        .badge {{ background: #28a745; color: white; padding: 4px 8px; border-radius: 4px; font-size: 12px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h2>📋 NOUVELLE DEMANDE</h2>
        </div>
        
        <div class='content'>
            <div class='section'>
                <h3>📊 INFORMATIONS CRÉDIT</h3>
                <table class='details-table'>
                    <tr>
                        <td>Montant demandé</td>
                        <td><strong>{loan.Amount:N0} {loan.Currency}</strong></td>
                    </tr>
                    <tr>
                        <td>Durée</td>
                        <td>{loan.Months} mois</td>
                    </tr>
                    <tr>
                        <td>Taux d'intérêt</td>
                        <td>{loan.InterestRate}%</td>
                    </tr>
                    <tr>
                        <td>Mensualité</td>
                        <td>{loan.MonthlyPayment:N2} {loan.Currency}</td>
                    </tr>
                    <tr>
                        <td>Total à rembourser</td>
                        <td><strong>{loan.TotalAmount:N2} {loan.Currency}</strong></td>
                    </tr>
                    <tr>
                        <td>Référence</td>
                        <td><span class='badge'>{loan.RequestId}</span></td>
                    </tr>
                </table>
            </div>
            
            <div class='section'>
                <h3>👤 INFORMATIONS CLIENT</h3>
                <table class='details-table'>
                    <tr>
                        <td>Nom complet</td>
                        <td><strong>{loan.FullName}</strong></td>
                    </tr>
                    <tr>
                        <td>Téléphone</td>
                        <td>{loan.Phone}</td>
                    </tr>
                    <tr>
                        <td>Email</td>
                        <td>{loan.Email}</td>
                    </tr>
                    <tr>
                        <td>Adresse</td>
                        <td>{loan.Address}</td>
                    </tr>
                    <tr>
                        <td>Pays</td>
                        <td>{loan.Country}</td>
                    </tr>
                </table>
            </div>
            
            <div class='section'>
                <p>📅 <strong>Date de soumission :</strong> {DateTime.Now:dd/MM/yyyy à HH:mm}</p>
                <p>🌐 <strong>IP client :</strong> {clientIp}</p>
            </div>
        </div>
        
        <div class='footer'>
            <div class='divider'></div>
            <p>Cette demande a été générée automatiquement via le formulaire de contact du site.</p>
            <p>Cordialement,</p>
            <p><strong>Système de notification automatique</strong></p>
            <p><strong>CreditPortal</strong></p>
        </div>
    </div>
</body>
</html>";
        }

        public string BuildLoanConfirmationEmail(LoanConfirmationDto loan, string companyName, string companyEmail, string language)
        {
            var texts = GetEmailTexts(language);
            var monthsText = GetMonthsText(loan.Months, language);

            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background: #007bff; color: white; padding: 20px; text-align: center; border-radius: 10px 10px 0 0; }}
        .content {{ background: #f8f9fa; padding: 30px; border-radius: 0 0 10px 10px; }}
        .details-table {{ width: 100%; border-collapse: collapse; margin: 20px 0; }}
        .details-table td {{ padding: 12px; border-bottom: 1px solid #dee2e6; }}
        .details-table td:first-child {{ font-weight: bold; width: 50%; }}
        .section {{ background: white; padding: 20px; border-radius: 10px; margin: 20px 0; }}
        .footer {{ text-align: center; margin-top: 30px; color: #6c757d; font-size: 14px; }}
        .divider {{ border-top: 1px solid #dee2e6; margin: 20px 0; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h2>{texts["Title"]}</h2>
        </div>
        
        <div class='content'>
            <p>{texts["Greeting"]} <strong>{loan.FullName}</strong>!</p>
            <p>{texts["ThankYou"]}</p>
            
            <div class='section'>
                <h3>{texts["DetailsHeader"]}</h3>
                <table class='details-table'>
                    <tr>
                        <td>{texts["RequestId"]}</td>
                        <td>{loan.RequestId}</td>
                    </tr>
                    <tr>
                        <td>{texts["Amount"]}</td>
                        <td>{loan.Amount:N0} {loan.Currency}</td>
                    </tr>
                    <tr>
                        <td>{texts["Term"]}</td>
                        <td>{loan.Months} {monthsText}</td>
                    </tr>
                    <tr>
                        <td>{texts["Rate"]}</td>
                        <td>{loan.InterestRate}% {texts["PerYear"]}</td>
                    </tr>
                    <tr>
                        <td>{texts["MonthlyPayment"]}</td>
                        <td>{loan.MonthlyPayment:N2} {loan.Currency}</td>
                    </tr>
                    <tr>
                        <td>{texts["TotalInterest"]}</td>
                        <td>{(loan.TotalAmount - loan.Amount):N2} {loan.Currency}</td>
                    </tr>
                    <tr>
                        <td>{texts["TotalAmount"]}</td>
                        <td><strong>{loan.TotalAmount:N2} {loan.Currency}</strong></td>
                    </tr>
                </table>
            </div>
            
            <div class='section'>
                <h3>{texts["LoanDetails"]}</h3>
                <p>{string.Format(texts["LoanDescription"], loan.Amount, loan.Currency, loan.Months, monthsText, loan.InterestRate, loan.MonthlyPayment, loan.Currency, loan.Months, monthsText)}</p>
                <p>{texts["FirstPayment"]}</p>
            </div>
            
            <div class='section'>
                <h3>{texts["DocumentsHeader"]}</h3>
                <ul>
                    <li>{texts["Doc1"]}</li>
                    <li>{texts["Doc2"]}</li>
                </ul>
                <p>{texts["AfterDocs"]}</p>
            </div>
            
            <p>{texts["Support"]}</p>
            <p>{texts["Regards"]}</p>
        </div>
        
        <div class='footer'>
            <div class='divider'></div>
            <p><strong>{texts["Team"]} {companyName}</strong></p>
            <p>{companyEmail}</p>
            <p>{texts["Date"]}: {DateTime.Now:dd-MM-yyyy HH:mm:ss}</p>
        </div>
    </div>
</body>
</html>";
        }

        private Dictionary<string, string> GetEmailTexts(string language)
        {
            return language switch
            {
                "ro" => new Dictionary<string, string>
                {
                    ["Title"] = "Confirmarea cererii de credit",
                    ["Greeting"] = "Bună ziua",
                    ["ThankYou"] = "Am primit cererea dvs. de credit și vă mulțumim pentru încredere.",
                    ["DetailsHeader"] = "📋 Detalii cerere",
                    ["RequestId"] = "ID cerere",
                    ["Amount"] = "Suma solicitată",
                    ["Term"] = "Termen",
                    ["Rate"] = "Rata dobânzii",
                    ["PerYear"] = "pe an",
                    ["MonthlyPayment"] = "Plată lunară",
                    ["TotalInterest"] = "Dobânda totală",
                    ["TotalAmount"] = "Suma totală de returnat",
                    ["LoanDetails"] = "Detalii credit",
                    ["LoanDescription"] = "Solicitați un credit de {0:N0} {1}, pe o perioadă de {2} {3} cu o rată de {4}%. Veți plăti o rată lunară de {5:N2} {6} la sfârșitul fiecărei luni timp de {7} {8}.",
                    ["FirstPayment"] = "Prima plată va fi efectuată la 3 luni după primirea creditului în contul dvs.",
                    ["DocumentsHeader"] = "Documente necesare",
                    ["Doc1"] = "Copie a actului de identitate sau pașaport valabil",
                    ["Doc2"] = "Dovada venitului (fișă de salariu, aviz fiscal...)",
                    ["AfterDocs"] = "După primirea acestor documente, vom examina cererea și vă vom contacta în 48 de ore.",
                    ["Support"] = "Serviciul nostru de asistență clienți vă stă la dispoziție pentru orice informații suplimentare.",
                    ["Regards"] = "Cu stimă",
                    ["Team"] = "Echipa",
                    ["Date"] = "Data"
                },
                "de" => new Dictionary<string, string>
                {
                    ["Title"] = "Kreditantrag Bestätigung",
                    ["Greeting"] = "Guten Tag",
                    ["ThankYou"] = "Wir haben Ihren Kreditantrag erhalten und danken Ihnen für Ihr Vertrauen.",
                    ["DetailsHeader"] = "📋 Antragsdetails",
                    ["RequestId"] = "Antrags-ID",
                    ["Amount"] = "Beantragter Betrag",
                    ["Term"] = "Laufzeit",
                    ["Rate"] = "Zinssatz",
                    ["PerYear"] = "pro Jahr",
                    ["MonthlyPayment"] = "Monatliche Rate",
                    ["TotalInterest"] = "Gesamtzinsen",
                    ["TotalAmount"] = "Gesamtrückzahlung",
                    ["LoanDetails"] = "Kreditdetails",
                    ["LoanDescription"] = "Sie beantragen einen Kredit über {0:N0} {1} mit einer Laufzeit von {2} {3} zu einem Zinssatz von {4}%. Sie zahlen eine monatliche Rate von {5:N2} {6} am Ende jedes Monats für {7} {8}.",
                    ["FirstPayment"] = "Die erste Zahlung erfolgt 3 Monate nach Erhalt des Kredits auf Ihr Konto.",
                    ["DocumentsHeader"] = "Erforderliche Dokumente",
                    ["Doc1"] = "Kopie des Personalausweises oder Reisepasses",
                    ["Doc2"] = "Einkommensnachweis (Gehaltsabrechnung, Steuerbescheid...)",
                    ["AfterDocs"] = "Nach Erhalt dieser Dokumente werden wir Ihren Antrag prüfen und Sie innerhalb von 48 Stunden kontaktieren.",
                    ["Support"] = "Unser Kundenservice steht Ihnen für weitere Informationen zur Verfügung.",
                    ["Regards"] = "Mit freundlichen Grüßen",
                    ["Team"] = "Team",
                    ["Date"] = "Datum"
                },
                "fr" => new Dictionary<string, string>
                {
                    ["Title"] = "Confirmation de demande de crédit",
                    ["Greeting"] = "Bonjour",
                    ["ThankYou"] = "Nous avons reçu votre demande de crédit et vous remercions de votre confiance.",
                    ["DetailsHeader"] = "📋 Détails de la demande",
                    ["RequestId"] = "ID de demande",
                    ["Amount"] = "Montant demandé",
                    ["Term"] = "Durée",
                    ["Rate"] = "Taux d'intérêt",
                    ["PerYear"] = "par an",
                    ["MonthlyPayment"] = "Paiement mensuel",
                    ["TotalInterest"] = "Intérêts totaux",
                    ["TotalAmount"] = "Montant total à rembourser",
                    ["LoanDetails"] = "Détails du crédit",
                    ["LoanDescription"] = "Vous demandez un crédit de {0:N0} {1} sur une durée de {2} {3} au taux de {4}%. Vous paierez une mensualité de {5:N2} {6} à la fin de chaque mois pendant {7} {8}.",
                    ["FirstPayment"] = "Le premier paiement sera effectué 3 mois après réception du crédit sur votre compte.",
                    ["DocumentsHeader"] = "Documents requis",
                    ["Doc1"] = "Copie de la carte d'identité ou passeport valide",
                    ["Doc2"] = "Justificatif de revenus (fiche de paie, avis d'imposition...)",
                    ["AfterDocs"] = "Après réception de ces documents, nous examinerons votre demande et vous contacterons dans les 48 heures.",
                    ["Support"] = "Notre service client reste à votre disposition pour toute information complémentaire.",
                    ["Regards"] = "Cordialement",
                    ["Team"] = "L'équipe",
                    ["Date"] = "Date"
                },
                "it" => new Dictionary<string, string>
                {
                    ["Title"] = "Conferma richiesta di prestito",
                    ["Greeting"] = "Buongiorno",
                    ["ThankYou"] = "Abbiamo ricevuto la sua richiesta di prestito e la ringraziamo per la fiducia.",
                    ["DetailsHeader"] = "📋 Dettagli richiesta",
                    ["RequestId"] = "ID richiesta",
                    ["Amount"] = "Importo richiesto",
                    ["Term"] = "Durata",
                    ["Rate"] = "Tasso di interesse",
                    ["PerYear"] = "all'anno",
                    ["MonthlyPayment"] = "Pagamento mensile",
                    ["TotalInterest"] = "Interessi totali",
                    ["TotalAmount"] = "Importo totale da restituire",
                    ["LoanDetails"] = "Dettagli prestito",
                    ["LoanDescription"] = "Richiede un prestito di {0:N0} {1} per una durata di {2} {3} al tasso del {4}%. Pagherà una rata mensile di {5:N2} {6} alla fine di ogni mese per {7} {8}.",
                    ["FirstPayment"] = "Il primo pagamento sarà effettuato 3 mesi dopo l'accredito del prestito sul suo conto.",
                    ["DocumentsHeader"] = "Documenti richiesti",
                    ["Doc1"] = "Copia del documento d'identità o passaporto valido",
                    ["Doc2"] = "Prova di reddito (busta paga, avviso fiscale...)",
                    ["AfterDocs"] = "Dopo aver ricevuto questi documenti, esamineremo la sua richiesta e la contatteremo entro 48 ore.",
                    ["Support"] = "Il nostro servizio clienti rimane a sua disposizione per qualsiasi informazione.",
                    ["Regards"] = "Cordiali saluti",
                    ["Team"] = "Team",
                    ["Date"] = "Data"
                },
                "es" => new Dictionary<string, string>
                {
                    ["Title"] = "Confirmación de solicitud de préstamo",
                    ["Greeting"] = "Buenos días",
                    ["ThankYou"] = "Hemos recibido su solicitud de préstamo y le agradecemos su confianza.",
                    ["DetailsHeader"] = "📋 Detalles de la solicitud",
                    ["RequestId"] = "ID de solicitud",
                    ["Amount"] = "Importe solicitado",
                    ["Term"] = "Plazo",
                    ["Rate"] = "Tasa de interés",
                    ["PerYear"] = "anual",
                    ["MonthlyPayment"] = "Pago mensual",
                    ["TotalInterest"] = "Intereses totales",
                    ["TotalAmount"] = "Importe total a devolver",
                    ["LoanDetails"] = "Detalles del préstamo",
                    ["LoanDescription"] = "Solicita un préstamo de {0:N0} {1} a un plazo de {2} {3} con una tasa del {4}%. Pagará una cuota mensual de {5:N2} {6} al final de cada mes durante {7} {8}.",
                    ["FirstPayment"] = "El primer pago se realizará 3 meses después de recibir el préstamo en su cuenta.",
                    ["DocumentsHeader"] = "Documentos requeridos",
                    ["Doc1"] = "Copia del documento de identidad o pasaporte válido",
                    ["Doc2"] = "Comprobante de ingresos (nómina, aviso fiscal...)",
                    ["AfterDocs"] = "Después de recibir estos documentos, examinaremos su solicitud y nos pondremos en contacto en 48 horas.",
                    ["Support"] = "Nuestro servicio de atención al cliente está a su disposición para cualquier información.",
                    ["Regards"] = "Atentamente",
                    ["Team"] = "Equipo",
                    ["Date"] = "Fecha"
                },
                "pt" => new Dictionary<string, string>
                {
                    ["Title"] = "Confirmação do pedido de empréstimo",
                    ["Greeting"] = "Bom dia",
                    ["ThankYou"] = "Recebemos o seu pedido de empréstimo e agradecemos a sua confiança.",
                    ["DetailsHeader"] = "📋 Detalhes do pedido",
                    ["RequestId"] = "ID do pedido",
                    ["Amount"] = "Montante solicitado",
                    ["Term"] = "Prazo",
                    ["Rate"] = "Taxa de juros",
                    ["PerYear"] = "ao ano",
                    ["MonthlyPayment"] = "Pagamento mensal",
                    ["TotalInterest"] = "Juros totais",
                    ["TotalAmount"] = "Montante total a devolver",
                    ["LoanDetails"] = "Detalhes do empréstimo",
                    ["LoanDescription"] = "Solicita um empréstimo de {0:N0} {1} com prazo de {2} {3} à taxa de {4}%. Pagará uma prestação mensal de {5:N2} {6} no final de cada mês durante {7} {8}.",
                    ["FirstPayment"] = "O primeiro pagamento será efetuado 3 meses após receber o empréstimo na sua conta.",
                    ["DocumentsHeader"] = "Documentos necessários",
                    ["Doc1"] = "Cópia do documento de identidade ou passaporte válido",
                    ["Doc2"] = "Comprovativo de rendimentos (recibo de vencimento, aviso fiscal...)",
                    ["AfterDocs"] = "Após receber estes documentos, analisaremos o seu pedido e entraremos em contacto dentro de 48 horas.",
                    ["Support"] = "O nosso serviço de apoio ao cliente está à sua disposição para qualquer informação.",
                    ["Regards"] = "Atenciosamente",
                    ["Team"] = "Equipa",
                    ["Date"] = "Data"
                },
                "tr" => new Dictionary<string, string>
                {
                    ["Title"] = "Kredi Başvurusu Onayı",
                    ["Greeting"] = "Merhaba",
                    ["ThankYou"] = "Kredi başvurunuzu aldık ve güveniniz için teşekkür ederiz.",
                    ["DetailsHeader"] = "📋 Başvuru Detayları",
                    ["RequestId"] = "Başvuru ID",
                    ["Amount"] = "Talep edilen tutar",
                    ["Term"] = "Vade",
                    ["Rate"] = "Faiz oranı",
                    ["PerYear"] = "yıllık",
                    ["MonthlyPayment"] = "Aylık ödeme",
                    ["TotalInterest"] = "Toplam faiz",
                    ["TotalAmount"] = "Toplam geri ödeme",
                    ["LoanDetails"] = "Kredi Detayları",
                    ["LoanDescription"] = "{0:N0} {1} tutarında, {2} {3} vadeli, %{4} faiz oranlı kredi talep ediyorsunuz. {7} {8} boyunca her ay sonunda {5:N2} {6} aylık ödeme yapacaksınız.",
                    ["FirstPayment"] = "İlk ödeme, kredi hesabınıza geçtikten 3 ay sonra yapılacaktır.",
                    ["DocumentsHeader"] = "Gerekli Belgeler",
                    ["Doc1"] = "Kimlik veya geçerli pasaport fotokopisi",
                    ["Doc2"] = "Gelir belgesi (maaş bordrosu, vergi bildirimi...)",
                    ["AfterDocs"] = "Bu belgeleri aldıktan sonra başvurunuzu inceleyecek ve 48 saat içinde sizinle iletişime geçeceğiz.",
                    ["Support"] = "Müşteri hizmetlerimiz her türlü bilgi için hizmetinizdedir.",
                    ["Regards"] = "Saygılarımızla",
                    ["Team"] = "Ekip",
                    ["Date"] = "Tarih"
                },
                _ => new Dictionary<string, string> // English default
                {
                    ["Title"] = "Loan Application Confirmation",
                    ["Greeting"] = "Hello",
                    ["ThankYou"] = "We have received your loan application and thank you for your trust.",
                    ["DetailsHeader"] = "📋 Application Details",
                    ["RequestId"] = "Request ID",
                    ["Amount"] = "Requested Amount",
                    ["Term"] = "Term",
                    ["Rate"] = "Interest Rate",
                    ["PerYear"] = "per year",
                    ["MonthlyPayment"] = "Monthly Payment",
                    ["TotalInterest"] = "Total Interest",
                    ["TotalAmount"] = "Total Amount to Return",
                    ["LoanDetails"] = "Loan Details",
                    ["LoanDescription"] = "You are requesting a loan of {0:N0} {1} for a term of {2} {3} at a rate of {4}%. You will pay a monthly payment of {5:N2} {6} at the end of each month for {7} {8}.",
                    ["FirstPayment"] = "The first payment will be made 3 months after receiving the loan in your account.",
                    ["DocumentsHeader"] = "Required Documents",
                    ["Doc1"] = "Copy of identity card or valid passport",
                    ["Doc2"] = "Proof of income (payslip, tax notice...)",
                    ["AfterDocs"] = "After receiving these documents, we will review your application and contact you within 48 hours.",
                    ["Support"] = "Our customer service remains at your disposal for any further information.",
                    ["Regards"] = "Best regards",
                    ["Team"] = "Team",
                    ["Date"] = "Date"
                }
            };
        }

        private string GetMonthsText(int months, string language)
        {
            if (language == "ro")
                return months == 1 ? "lună" : "luni";
            if (language == "de")
                return months == 1 ? "Monat" : "Monate";
            if (language == "fr")
                return months == 1 ? "mois" : "mois";
            if (language == "it")
                return months == 1 ? "mese" : "mesi";
            if (language == "es")
                return months == 1 ? "mes" : "meses";
            if (language == "pt")
                return months == 1 ? "mês" : "meses";
            if (language == "tr")
                return "ay";

            // English default
            return months == 1 ? "month" : "months";
        }

        private string StripHtml(string html)
        {
            return System.Text.RegularExpressions.Regex.Replace(html, "<.*?>", string.Empty);
        }
    }
}