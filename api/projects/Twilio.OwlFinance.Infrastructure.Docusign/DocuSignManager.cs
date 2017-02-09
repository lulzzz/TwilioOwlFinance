using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DocuSign.eSign.Api;
using DocuSign.eSign.Client;
using DocuSign.eSign.Model;
using Newtonsoft.Json;
using Twilio.OwlFinance.Domain.Model.Api;
using Twilio.OwlFinance.Domain.Model.Data;
using Twilio.OwlFinance.Domain.Model.Documents;

namespace Twilio.OwlFinance.Infrastructure.Docusign
{
    public class DocuSignManager
    {
        private readonly PdfGenerator pdfGenerator;

        public DocuSignManager() : this(new PdfGenerator())
        {
            
        }

        private DocuSignManager(PdfGenerator pdfGenerator)
        {
            this.pdfGenerator = pdfGenerator;
        }

        public DocuSignResponseModel SendDocument(DocumentSignModel model, Case caseModel, DocuSignAuthHeader authInformation, string serverPath)
        {
            var docusignResponse = new DocuSignResponseModel();

            var loginInfo = GetLoginInfo(authInformation);

            var documentModel = SendDocumentAndGetUrl(model, caseModel, loginInfo, serverPath);

            docusignResponse.SignUrl = documentModel.SignUrl;
            docusignResponse.CaseID = model.CaseID.ToString();
            docusignResponse.DocumentID = documentModel.DocumentID;
            docusignResponse.EnvelopeID = documentModel.EnvelopeID;

            return docusignResponse;
        }

        public DocuSignResponseModel GetDocument(DocuSignLog log, DocuSignAuthHeader authInformation, string serverPath, string host)
        {
            var docusignResponse = new DocuSignResponseModel();

            var loginInfo = GetLoginInfo(authInformation);

            EnvelopesApi envelopesApi = new EnvelopesApi();

            var document2 = envelopesApi.GetDocument(loginInfo.AccountId, log.EnvelopeID, log.DocumentID);

            var documentUrl =  "/documents/signed/signedocument-" + Guid.NewGuid() + ".pdf";

            var filePath = serverPath + documentUrl;
            using (Stream file = File.Create(filePath))
            {
                FileHelpers.CopyStream(document2, file);
            }


            docusignResponse.DocumentUrl = host + "/" + documentUrl;
                
            docusignResponse.CaseID = log.CaseID.ToString();
            docusignResponse.DocumentID = log.DocumentID;
            docusignResponse.EnvelopeID = log.EnvelopeID;

            return docusignResponse;


        }

        private DocumentModel SendDocumentAndGetUrl(DocumentSignModel model, Case caseModel, LoginAccount loginInfo, string serverPath)
        {
            var amountString = (caseModel.Transaction.Amount/100d).ToString("C");

            var email = Guid.NewGuid() + "@twilio.com";

            string generatedPdfFilePath = pdfGenerator.GenerateDocument(caseModel.Customer.FirstName + " " + caseModel.Customer.LastName, caseModel.Transaction.Description, amountString , serverPath);

            byte[] fileBytes = File.ReadAllBytes(generatedPdfFilePath);
            EnvelopeDefinition envDef = new EnvelopeDefinition { EmailSubject = "Owl Finance: Transaction" };

            var documentModel = new DocumentModel();

            var documentId = model.CaseID.ToString();
            documentModel.DocumentID = documentId;
            var clientId = caseModel.Customer.ID.ToString();

            // Add a document to the envelope
            Document doc = new Document
            {
                DocumentBase64 = Convert.ToBase64String(fileBytes),
                Name = "Case" + model.CaseID + ".pdf",
                DocumentId = documentId
            };
            envDef.Documents = new List<Document> { doc };

            // Add a recipient to sign the documeent
            Signer signer = new Signer
            {
                Name = model.SendTo,
                Email = email,
                RecipientId = "1",
                ClientUserId = clientId,
                Tabs = new Tabs { SignHereTabs = new List<SignHere>() }
            };

            // Create a |SignHere| tab somewhere on the document for the recipient to sign
            SignHere signHere = new SignHere
            {
                DocumentId = documentId,
                PageNumber = "1",
                RecipientId = "1",
                XPosition = "40",
                YPosition = "175"
            };
            signer.Tabs.SignHereTabs.Add(signHere);

            envDef.Recipients = new Recipients { Signers = new List<Signer> { signer } };
            // set envelope status to "sent" to immediately send the signature request
            envDef.Status = "sent";

            EnvelopesApi envelopesApi = new EnvelopesApi();
            EnvelopeSummary envelopeSummary = envelopesApi.CreateEnvelope(loginInfo.AccountId, envDef);

            RecipientViewRequest viewOptions = new RecipientViewRequest()
            {
                ReturnUrl = "https://owlfinance.azurewebsites.net/#/docusign",
                //ReturnUrl = "https://www.docusign.com/devcenter",
                ClientUserId = clientId,
                AuthenticationMethod = "email",
                UserName = model.SendTo,
                Email = email
            };
            // create the recipient view (aka signing URL)
            ViewUrl recipientView = envelopesApi.CreateRecipientView(loginInfo.AccountId, envelopeSummary.EnvelopeId,
                viewOptions);

            documentModel.SignUrl = recipientView.Url;
            documentModel.EnvelopeID = envelopeSummary.EnvelopeId;
            documentModel.DocumentID = documentId;

            return documentModel;
        }

        private LoginAccount GetLoginInfo(DocuSignAuthHeader headerOptions)
        {
            var basePath = "https://demo.docusign.net/restapi";
            var apiClient = new ApiClient(basePath);
            Configuration.Default.ApiClient = apiClient;

            var authHeader = JsonConvert.SerializeObject(headerOptions);
            if (!Configuration.Default.DefaultHeader.ContainsKey("X-DocuSign-Authentication"))
            {
                Configuration.Default.AddDefaultHeader("X-DocuSign-Authentication", authHeader);
            }

            var authApi = new AuthenticationApi();
            var loginInfoResult = authApi.Login();
            var loginInfo = loginInfoResult.LoginAccounts.First();

            return loginInfo;
        }
    }
}
