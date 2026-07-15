using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Argitis.DTOs;

namespace Argitis.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailDto email);
        string BuildLoanConfirmationEmail(LoanConfirmationDto loan, string companyName, string companyEmail, string language);
        string BuildAdminNotificationEmail(LoanConfirmationDto loan, string clientIp);
    }

    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(EmailSettings settings)
        {
            _settings = settings;
        }

        public async Task SendEmailAsync(EmailDto email)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
            message.To.Add(new MailboxAddress(email.ToName, email.ToEmail));
            message.Subject = email.Subject;
            message.MessageId = $"{Guid.NewGuid()}@veltisgroup.com";

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = email.Body,
                TextBody = StripHtml(email.Body)
            };
            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();

            // Настройки подключения к Namecheap через порт 587 (TLS)
            // Для порта 465 замените StartTls на SslOnConnect
            try
            {
                Console.WriteLine($"[LOG] Attempting to connect to {_settings.SmtpServer}:{_settings.SmtpPort}...");

                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
                await client.ConnectAsync(_settings.SmtpServer, _settings.SmtpPort, SecureSocketOptions.StartTls, cts.Token);

                Console.WriteLine($"[LOG] Connected! Authenticating...");
                await client.AuthenticateAsync(_settings.SenderEmail, _settings.SenderPassword, cts.Token);

                Console.WriteLine($"[LOG] Authenticated! Sending email...");
                await client.SendAsync(message, cts.Token);

                Console.WriteLine($"[LOG] Email sent successfully!");
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] CAUGHT EXCEPTION: {ex.GetType().FullName}");
                Console.WriteLine($"[ERROR] Message: {ex.Message}");
                Console.WriteLine($"[ERROR] StackTrace: {ex.StackTrace}");

                // Если это SocketException, вы увидите в логах
                throw;
            }
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
            <p><strong>VeltisGroup</strong></p>
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
                "en" => new Dictionary<string, string>
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
                "fi" => new Dictionary<string, string>
                {
                    ["Title"] = "Lainahakemuksen vahvistus",
                    ["Greeting"] = "Hei",
                    ["ThankYou"] = "Olemme vastaanottaneet lainahakemuksesi ja kiitämme luottamuksestasi.",
                    ["DetailsHeader"] = "📋 Hakemuksen tiedot",
                    ["RequestId"] = "Hakemus-ID",
                    ["Amount"] = "Haettu summa",
                    ["Term"] = "Laina-aika",
                    ["Rate"] = "Korko",
                    ["PerYear"] = "vuodessa",
                    ["MonthlyPayment"] = "Kuukausimaksu",
                    ["TotalInterest"] = "Kokonaiskorot",
                    ["TotalAmount"] = "Takaisinmaksettava kokonaissumma",
                    ["LoanDetails"] = "Lainan tiedot",
                    ["LoanDescription"] = "Haet lainaa {0:N0} {1} laina-ajalla {2} {3} korolla {4}%. Maksat kuukausittain {5:N2} {6} jokaisen kuukauden lopussa {7} {8} ajan.",
                    ["FirstPayment"] = "Ensimmäinen maksu suoritetaan 3 kuukautta lainan saamisesta tilillesi.",
                    ["DocumentsHeader"] = "Vaaditut asiakirjat",
                    ["Doc1"] = "Kopio henkilöllisyystodistuksesta tai voimassa olevasta passista",
                    ["Doc2"] = "Todistus tuloista (palkkalaskelma, veroilmoitus...)",
                    ["AfterDocs"] = "Kun olemme vastaanottaneet nämä asiakirjat, tarkistamme hakemuksesi ja otamme sinuun yhteyttä 48 tunnin kuluessa.",
                    ["Support"] = "Asiakaspalvelumme on käytettävissäsi kaikissa lisäkysymyksissä.",
                    ["Regards"] = "Ystävällisin terveisin",
                    ["Team"] = "Tiimi",
                    ["Date"] = "Päivämäärä"
                },
                "hr" => new Dictionary<string, string>
                {
                    ["Title"] = "Potvrda zahtjeva za kredit",
                    ["Greeting"] = "Pozdrav",
                    ["ThankYou"] = "Zaprimili smo vaš zahtjev za kredit i zahvaljujemo vam na povjerenju.",
                    ["DetailsHeader"] = "📋 Detalji zahtjeva",
                    ["RequestId"] = "ID zahtjeva",
                    ["Amount"] = "Traženi iznos",
                    ["Term"] = "Rok",
                    ["Rate"] = "Kamatna stopa",
                    ["PerYear"] = "godišnje",
                    ["MonthlyPayment"] = "Mjesečna rata",
                    ["TotalInterest"] = "Ukupne kamate",
                    ["TotalAmount"] = "Ukupni iznos za povrat",
                    ["LoanDetails"] = "Detalji kredita",
                    ["LoanDescription"] = "Tražite kredit od {0:N0} {1} na rok od {2} {3} uz kamatnu stopu od {4}%. Plaćat ćete mjesečnu ratu od {5:N2} {6} na kraju svakog mjeseca kroz {7} {8}.",
                    ["FirstPayment"] = "Prva uplata bit će izvršena 3 mjeseca nakon primitka kredita na vaš račun.",
                    ["DocumentsHeader"] = "Potrebni dokumenti",
                    ["Doc1"] = "Kopija osobne iskaznice ili važeće putovnice",
                    ["Doc2"] = "Dokaz o prihodima (platna lista, porezna prijava...)",
                    ["AfterDocs"] = "Nakon primitka ovih dokumenata, pregledat ćemo vaš zahtjev i kontaktirati vas u roku od 48 sati.",
                    ["Support"] = "Naša korisnička podrška stoji vam na raspolaganju za sve dodatne informacije.",
                    ["Regards"] = "S poštovanjem",
                    ["Team"] = "Tim",
                    ["Date"] = "Datum"
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
                "lt" => new Dictionary<string, string>
                {
                    ["Title"] = "Paskolos paraiškos patvirtinimas",
                    ["Greeting"] = "Labas",
                    ["ThankYou"] = "Gavome jūsų paskolos paraišką ir dėkojame už pasitikėjimą.",
                    ["DetailsHeader"] = "📋 Paraiškos detalės",
                    ["RequestId"] = "Paraiškos ID",
                    ["Amount"] = "Prašoma suma",
                    ["Term"] = "Terminas",
                    ["Rate"] = "Palūkanų norma",
                    ["PerYear"] = "per metus",
                    ["MonthlyPayment"] = "Mėnesinė įmoka",
                    ["TotalInterest"] = "Bendros palūkanos",
                    ["TotalAmount"] = "Bendra grąžintina suma",
                    ["LoanDetails"] = "Paskolos detalės",
                    ["LoanDescription"] = "Prašote {0:N0} {1} paskolos terminui {2} {3} su {4}% palūkanų norma. Mokėsite mėnesinę įmoką {5:N2} {6} kiekvieno mėnesio pabaigoje {7} {8}.",
                    ["FirstPayment"] = "Pirmasis mokėjimas bus atliktas praėjus 3 mėnesiams nuo paskolos gavimo į jūsų sąskaitą.",
                    ["DocumentsHeader"] = "Reikalingi dokumentai",
                    ["Doc1"] = "Asmens tapatybės kortelės arba galiojančio paso kopija",
                    ["Doc2"] = "Pajamų įrodymas (darbo užmokesčio lapelis, mokesčių pranešimas...)",
                    ["AfterDocs"] = "Gavę šiuos dokumentus, peržiūrėsime jūsų paraišką ir susisieksime su jumis per 48 valandas.",
                    ["Support"] = "Mūsų klientų aptarnavimo tarnyba yra jūsų žinioje visais papildomais klausimais.",
                    ["Regards"] = "Pagarbiai",
                    ["Team"] = "Komanda",
                    ["Date"] = "Data"
                },
                "lv" => new Dictionary<string, string>
                {
                    ["Title"] = "Aizdevuma pieteikuma apstiprinājums",
                    ["Greeting"] = "Sveiki",
                    ["ThankYou"] = "Esam saņēmuši jūsu aizdevuma pieteikumu un pateicamies par uzticību.",
                    ["DetailsHeader"] = "📋 Pieteikuma detaļas",
                    ["RequestId"] = "Pieteikuma ID",
                    ["Amount"] = "Pieprasītā summa",
                    ["Term"] = "Termiņš",
                    ["Rate"] = "Procentu likme",
                    ["PerYear"] = "gadā",
                    ["MonthlyPayment"] = "Mēneša maksājums",
                    ["TotalInterest"] = "Kopējie procenti",
                    ["TotalAmount"] = "Kopējā atdodamā summa",
                    ["LoanDetails"] = "Aizdevuma detaļas",
                    ["LoanDescription"] = "Jūs pieprasāt aizdevumu {0:N0} {1} uz termiņu {2} {3} ar {4}% procentu likmi. Jūs maksāsiet mēneša maksājumu {5:N2} {6} katra mēneša beigās {7} {8}.",
                    ["FirstPayment"] = "Pirmais maksājums tiks veikts 3 mēnešus pēc aizdevuma saņemšanas jūsu kontā.",
                    ["DocumentsHeader"] = "Nepieciešamie dokumenti",
                    ["Doc1"] = "Personas apliecības vai derīgas pases kopija",
                    ["Doc2"] = "Ienākumu apliecinājums (algas lapa, nodokļu paziņojums...)",
                    ["AfterDocs"] = "Pēc šo dokumentu saņemšanas mēs izskatīsim jūsu pieteikumu un sazināsimies ar jums 48 stundu laikā.",
                    ["Support"] = "Mūsu klientu apkalpošanas dienests ir jūsu rīcībā jebkurai papildu informācijai.",
                    ["Regards"] = "Ar cieņu",
                    ["Team"] = "Komanda",
                    ["Date"] = "Datums"
                },
                "mt" => new Dictionary<string, string>
                {
                    ["Title"] = "Konferma tat-talba għas-self",
                    ["Greeting"] = "Bongu",
                    ["ThankYou"] = "Irċevejna t-talba tiegħek għas-self u nirringrazzjawlek tal-fiduċja tiegħek.",
                    ["DetailsHeader"] = "📋 Dettalji tat-talba",
                    ["RequestId"] = "ID tat-talba",
                    ["Amount"] = "Ammont mitlub",
                    ["Term"] = "Terminu",
                    ["Rate"] = "Rata tal-imgħax",
                    ["PerYear"] = "fis-sena",
                    ["MonthlyPayment"] = "Ħlas mensili",
                    ["TotalInterest"] = "Imgħax totali",
                    ["TotalAmount"] = "Ammont totali biex jitħallas lura",
                    ["LoanDetails"] = "Dettalji tas-self",
                    ["LoanDescription"] = "Qed titlob self ta' {0:N0} {1} għal terminu ta' {2} {3} b'rata ta' {4}%. Tħallas ħlas mensili ta' {5:N2} {6} fl-aħħar ta' kull xahar għal {7} {8}.",
                    ["FirstPayment"] = "L-ewwel ħlas isir 3 xhur wara li tirċievi s-self fil-kont tiegħek.",
                    ["DocumentsHeader"] = "Dokumenti meħtieġa",
                    ["Doc1"] = "Kopja tal-karta tal-identità jew passaport validu",
                    ["Doc2"] = "Prova tad-dħul (pagament tal-paga, avviż tat-taxxa...)",
                    ["AfterDocs"] = "Wara li nirċievu dawn id-dokumenti, nirrevedu t-talba tiegħek u nikkuntattjawlek fi żmien 48 siegħa.",
                    ["Support"] = "Is-servizz tal-konsumatur tagħna jibqa' għad-dispożizzjoni tiegħek għal kull informazzjoni addizzjonali.",
                    ["Regards"] = "Dejjem tiegħek",
                    ["Team"] = "Tim",
                    ["Date"] = "Data"
                },
                "nl" => new Dictionary<string, string>
                {
                    ["Title"] = "Bevestiging van leningaanvraag",
                    ["Greeting"] = "Hallo",
                    ["ThankYou"] = "We hebben uw leningaanvraag ontvangen en bedanken u voor uw vertrouwen.",
                    ["DetailsHeader"] = "📋 Aanvraaggegevens",
                    ["RequestId"] = "Aanvraag-ID",
                    ["Amount"] = "Aangevraagd bedrag",
                    ["Term"] = "Looptijd",
                    ["Rate"] = "Rente",
                    ["PerYear"] = "per jaar",
                    ["MonthlyPayment"] = "Maandelijkse betaling",
                    ["TotalInterest"] = "Totale rente",
                    ["TotalAmount"] = "Totaal terug te betalen bedrag",
                    ["LoanDetails"] = "Leningdetails",
                    ["LoanDescription"] = "U vraagt een lening aan van {0:N0} {1} voor een looptijd van {2} {3} tegen een rentepercentage van {4}%. U betaalt een maandelijks bedrag van {5:N2} {6} aan het einde van elke maand gedurende {7} {8}.",
                    ["FirstPayment"] = "De eerste betaling vindt plaats 3 maanden na ontvangst van de lening op uw rekening.",
                    ["DocumentsHeader"] = "Vereiste documenten",
                    ["Doc1"] = "Kopie van identiteitsbewijs of geldig paspoort",
                    ["Doc2"] = "Inkomensbewijs (loonstrook, belastingaanslag...)",
                    ["AfterDocs"] = "Na ontvangst van deze documenten zullen we uw aanvraag beoordelen en binnen 48 uur contact met u opnemen.",
                    ["Support"] = "Onze klantenservice staat tot uw beschikking voor verdere informatie.",
                    ["Regards"] = "Met vriendelijke groet",
                    ["Team"] = "Team",
                    ["Date"] = "Datum"
                },
                "pl" => new Dictionary<string, string>
                {
                    ["Title"] = "Potwierdzenie wniosku o pożyczkę",
                    ["Greeting"] = "Dzień dobry",
                    ["ThankYou"] = "Otrzymaliśmy Twój wniosek o pożyczkę i dziękujemy za zaufanie.",
                    ["DetailsHeader"] = "📋 Szczegóły wniosku",
                    ["RequestId"] = "ID wniosku",
                    ["Amount"] = "Wnioskowana kwota",
                    ["Term"] = "Okres",
                    ["Rate"] = "Oprocentowanie",
                    ["PerYear"] = "w skali roku",
                    ["MonthlyPayment"] = "Miesięczna rata",
                    ["TotalInterest"] = "Całkowite odsetki",
                    ["TotalAmount"] = "Całkowita kwota do zwrotu",
                    ["LoanDetails"] = "Szczegóły pożyczki",
                    ["LoanDescription"] = "Wnioskujesz o pożyczkę w kwocie {0:N0} {1} na okres {2} {3} z oprocentowaniem {4}%. Będziesz płacić miesięczną ratę w wysokości {5:N2} {6} na koniec każdego miesiąca przez {7} {8}.",
                    ["FirstPayment"] = "Pierwsza płatność zostanie dokonana 3 miesiące po otrzymaniu pożyczki na Twoje konto.",
                    ["DocumentsHeader"] = "Wymagane dokumenty",
                    ["Doc1"] = "Kopia dowodu osobistego lub ważnego paszportu",
                    ["Doc2"] = "Dokument potwierdzający dochód (pasek wypłaty, decyzja podatkowa...)",
                    ["AfterDocs"] = "Po otrzymaniu tych dokumentów rozpatrzymy Twój wniosek i skontaktujemy się z Tobą w ciągu 48 godzin.",
                    ["Support"] = "Nasza obsługa klienta pozostaje do Twojej dyspozycji w przypadku wszelkich dodatkowych informacji.",
                    ["Regards"] = "Z poważaniem",
                    ["Team"] = "Zespół",
                    ["Date"] = "Data"
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
                "sk" => new Dictionary<string, string>
                {
                    ["Title"] = "Potvrdenie žiadosti o úver",
                    ["Greeting"] = "Dobrý deň",
                    ["ThankYou"] = "Prijali sme vašu žiadosť o úver a ďakujeme vám za dôveru.",
                    ["DetailsHeader"] = "📋 Podrobnosti žiadosti",
                    ["RequestId"] = "ID žiadosti",
                    ["Amount"] = "Požadovaná suma",
                    ["Term"] = "Lehota",
                    ["Rate"] = "Úroková sadzba",
                    ["PerYear"] = "ročne",
                    ["MonthlyPayment"] = "Mesačná splátka",
                    ["TotalInterest"] = "Celkový úrok",
                    ["TotalAmount"] = "Celková suma na vrátenie",
                    ["LoanDetails"] = "Podrobnosti úveru",
                    ["LoanDescription"] = "Žiadate o úver vo výške {0:N0} {1} na dobu {2} {3} s úrokovou sadzbou {4}%. Budete platiť mesačnú splátku {5:N2} {6} na konci každého mesiaca počas {7} {8}.",
                    ["FirstPayment"] = "Prvá platba sa uskutoční 3 mesiace po pripísaní úveru na váš účet.",
                    ["DocumentsHeader"] = "Požadované dokumenty",
                    ["Doc1"] = "Kópia občianskeho preukazu alebo platného pasu",
                    ["Doc2"] = "Potvrdenie o príjme (výplatná páska, daňové oznámenie...)",
                    ["AfterDocs"] = "Po prijatí týchto dokumentov preskúmame vašu žiadosť a ozveme sa vám do 48 hodín.",
                    ["Support"] = "Náš zákaznícky servis je vám k dispozícii pre akékoľvek ďalšie informácie.",
                    ["Regards"] = "S úctou",
                    ["Team"] = "Tím",
                    ["Date"] = "Dátum"
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
            return language switch
            {
                "fr" => months == 1 ? "mois" : "mois",
                "de" => months == 1 ? "Monat" : "Monate",
                "es" => months == 1 ? "mes" : "meses",
                "it" => months == 1 ? "mese" : "mesi",
                "pt" => months == 1 ? "mês" : "meses",
                "ro" => months == 1 ? "lună" : "luni",
                "nl" => months == 1 ? "maand" : "maanden",
                "pl" => months == 1 ? "miesiąc" : "miesięcy",
                "fi" => months == 1 ? "kuukausi" : "kuukautta",
                "hr" => months == 1 ? "mjesec" : "mjeseci",
                "lt" => months == 1 ? "mėnuo" : "mėnesių",
                "lv" => months == 1 ? "mēnesis" : "mēnešu",
                "mt" => months == 1 ? "xahar" : "xhahru",
                "sk" => months == 1 ? "mesiac" : "mesiacov",
                _ => months == 1 ? "month" : "months" // English default
            };
        }

        private string StripHtml(string html)
        {
            return System.Text.RegularExpressions.Regex.Replace(html, "<.*?>", string.Empty);
        }
    }

    public class EmailSettings
    {
        public string SmtpServer { get; set; } = "";
        public int SmtpPort { get; set; }
        public string SenderEmail { get; set; } = "";
        public string SenderName { get; set; } = "";
        public string SenderPassword { get; set; } = "";
        public bool UseSsl { get; set; }
    }
}