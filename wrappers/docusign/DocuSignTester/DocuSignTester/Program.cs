using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocuSign.eSign.Api;
using DocuSign.eSign.Client;
using DocuSign.eSign.Model;
using Newtonsoft.Json;

namespace DocuSignTester
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting...");
            //var envelopeId = "bfcb6bfc-1997-4c18-a91d-3fafc8698398";
            //https://demo.docusign.net/Signing/startinsession.aspx?t=02321ba1-c37a-42eb-ba7a-bc630f347108
            DoLogin();
            var envelopeId = SendDocument();
            RetrieveDocuments(envelopeId);
        }

        private static void RetrieveDocuments(string envelopeId)
        {
            EnvelopesApi envelopesApi = new EnvelopesApi();

            var docListResult = envelopesApi.ListDocuments("1720530", "4b242b9d-8dd0-4cd5-88da-f730916ccef6");

            var documents = docListResult.EnvelopeDocuments;

            foreach (var document in documents)
            {
                Console.WriteLine(document.Uri);
            }
            var document2 = envelopesApi.GetDocument("1720530", envelopeId, "5");

            using (Stream file = File.Create(@"C:\temp\docutestsigned2.pdf"))
            {
                FileHelpers.CopyStream(document2, file);
            }
        }

        private static string SendDocument()
        {
            //https://demo.docusign.net/restapi/v2/accounts/1720530
			//1720530
			///Users/admin/Documents/docutest.pdf
			byte[] fileBytes = File.ReadAllBytes(@"c:\temp\docutest.pdf");
			EnvelopeDefinition envDef = new EnvelopeDefinition();
            envDef.EmailSubject = "[DocuSign C# SDK] - Please sign this doc2";

			// Add a document to the envelope
			Document doc = new Document();
            doc.DocumentBase64 = System.Convert.ToBase64String(fileBytes);
            doc.Name = "DocuTest5.pdf";
            doc.DocumentId = "5";
			envDef.Documents = new List<Document>();
            envDef.Documents.Add(doc);

			// Add a recipient to sign the documeent
			Signer signer = new Signer();
            signer.Name = "Subbarao";
            signer.Email = "subbarao.cse@gmail.com";
            signer.RecipientId = "1";
            signer.ClientUserId = "1234";

			// Create a |SignHere| tab somewhere on the document for the recipient to sign
			signer.Tabs = new Tabs();
            signer.Tabs.SignHereTabs = new List<SignHere>();
            SignHere signHere = new SignHere();
            signHere.DocumentId = "5";
            signHere.PageNumber = "1";
            signHere.RecipientId = "1";
            signHere.XPosition = "100";
            signHere.YPosition = "150";
            signer.Tabs.SignHereTabs.Add(signHere);

			envDef.Recipients = new Recipients();
            envDef.Recipients.Signers = new List<Signer>();
            envDef.Recipients.Signers.Add(signer);
			// set envelope status to "sent" to immediately send the signature request
			envDef.Status = "sent";

			// Use the EnvelopesApi to send the signature request!
			EnvelopesApi envelopesApi = new EnvelopesApi();
            EnvelopeSummary envelopeSummary = envelopesApi.CreateEnvelope("1720530", envDef);

			// print the JSON response
			Console.WriteLine("EnvelopeSummary:\n{0}", JsonConvert.SerializeObject(envelopeSummary));

			RecipientViewRequest viewOptions = new RecipientViewRequest()
            {
                ReturnUrl = "https://www.docusign.com/devcenter",
                ClientUserId = "1234",  // must match clientUserId set in step #2!
                AuthenticationMethod = "email",
                UserName = "Subbarao",
                Email = "subbarao.cse@gmail.com"
            };
			// create the recipient view (aka signing URL)
			ViewUrl recipientView = envelopesApi.CreateRecipientView("1720530", envelopeSummary.EnvelopeId, viewOptions);
			// print the JSON response
			Console.WriteLine("ViewUrl:\n{0}", JsonConvert.SerializeObject(recipientView));
            Console.WriteLine(envelopeSummary.EnvelopeId);
            return envelopeSummary.EnvelopeId;
        }

        private static void DoLogin()
        {
            string Username = "venkata@falafel.com";
            string Password = "ptn7pYHup8cTee";
            string IntegratorKey = "068a8288-e7be-466c-9103-31bb707a8cd0";
			// set demo (aka test) environment (for production change to www.docusign.net/restapi)
			string basePath = "https://demo.docusign.net/restapi";
			// instantiate a new api client and set desired environment
			ApiClient apiClient = new ApiClient(basePath);
			// set client in global config so we don't have to pass it to each API object.
			Configuration.Default.ApiClient = apiClient;
			// create JSON formatted auth header containing Username, Password, and Integrator Key
			string authHeader = "{\"Username\":\"" + Username + "\", \"Password\":\"" + Password + "\", \"IntegratorKey\":\"" + IntegratorKey + "\"}";
            Configuration.Default.AddDefaultHeader("X-DocuSign-Authentication", authHeader);
			// the authentication api uses the apiClient (and X-DocuSign-Authentication header) that are set in Configuration object
			AuthenticationApi authApi = new AuthenticationApi();
            LoginInformation loginInfo = authApi.Login();
			Console.WriteLine("LoginInformation: {0}", loginInfo.ToJson());
        }
    }
}
