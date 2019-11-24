using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EntGlobus.Controllers
{
    public class TestController : Controller
    {
    //    const string url = "https://www.test.wooppay.com/api/wsdl?ws=1";
    //    const string action = "";
    //    HttpWebRequest webRequest = CreateWebRequest(url, action);

        public String SOAPManual(string nn)
        {
            const string url = "https://www.test.wooppay.com/api/wsdl?ws=1";
            const string action = "";

            XmlDocument soapEnvelopeXml = CreateSoapEnvelope(nn);
            HttpWebRequest webRequest = CreateWebRequest(url, action);

            InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

            string result;
            using (WebResponse response = webRequest.GetResponse())
            {
                using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                {
                    result = rd.ReadToEnd();
                }
            }
            return result;
        }

        private static HttpWebRequest CreateWebRequest(string url, string action)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Headers.Add("SOAPAction", action);
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }
        
        private  static  XmlDocument CreateSoapEnvelope(string hh)
        {
            XmlDocument soapEnvelopeXml = new XmlDocument();
            soapEnvelopeXml.LoadXml(hh);
            return soapEnvelopeXml;
        }

        private static void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        {
            using (Stream stream = webRequest.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }
        }
    
        public ActionResult Index()
        {
            //ServiceReference1.CoreLoginRequest coreLogin = new ServiceReference1.CoreLoginRequest();

            //coreLogin.username = "test_merch";
            //coreLogin.password = "A12345678a";
            //CoreLoginResponseData data = new CoreLoginResponseData();

            HttpContext.Session.SetString("actions", "d");

            var dd = SOAPManual(@"<soapenv:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:urn=""urn:XmlControllerwsdl"">
   <soapenv:Header/>
    <soapenv:Body>
<urn:core_login soapenv:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">
            <coreLoginRequest xsi:type=""urn:CoreLoginRequest"" xs:type=""type:CoreLoginRequest"" xmlns:xs=""http://www.w3.org/2000/XMLSchema-instance"">
                        <username xsi:type=""xsd:string"" xs:type=""type:string"">test_merch</username>
                             <password xsi:type=""xsd:string"" xs:type=""type:string"">A12345678a</password>
                                  <captcha xsi:type=""xsd:string"" xs:type=""type:string"">?</captcha>
                                    </coreLoginRequest>
                                 </urn:core_login>
                               </soapenv:Body>
                            </soapenv:Envelope>");




            XmlDocument Xml = new XmlDocument();

              Xml.LoadXml(dd);


            return Content(dd,"text/xml");
        }

        public string Index2()
        {
            return DateTime.Now.ToString("T");
        }

        // GET: Test/Details/5
        public ActionResult Details()
        {

            var dd = SOAPManual(@"<?xml version=""1.0"" encoding=""utf-8""?>
            <soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:soapenc=""http://schemas.xmlsoap.org/soap/encoding/"" xmlns:tns=""urn:XmlControllerwsdl"" xmlns:types=""urn:XmlControllerwsdl/encodedTypes"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
            <soap:Body soap:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">
            <tns:core_login>
            <coreLoginRequest href=""#id1""/>
            </tns:core_login>
            <tns:CoreLoginRequest id=""id1"" xsi:type=""tns:CoreLoginRequest"">
            <username xsi:type=""xsd:string"">test_merch</username>
            <password xsi:type=""xsd:string"">A12345678a</password>
            </tns:CoreLoginRequest>
            </soap:Body>
            </soap:Envelope>");


            XmlDocument xml = new XmlDocument();
            xml.LoadXml(dd);
            XDocument xDoc = XDocument.Load(new StringReader(dd));
            string personId = xDoc.Descendants("session").First().Value;




            return Content(personId.ToString());
        }
        //<SOAP-ENV:Header>
        //    <t:Transaction  xmlns:t =""https://www.test.wooppay.com/api/wsdl?ws=1"" SOAP-ENV:mustUnderstand =""true"">5</t:Transaction>
        //    </SOAP-ENV:Header>
        public ActionResult Order()
        {
            var sop = SOAPManual(@"<soapenv:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:urn=""urn:XmlControllerwsdl"">
            <soapenv:Header/>
             <soapenv:Body>
        <urn:cash_createInvoice2Extended soapenv:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">
            <cashCreateInvoiceExtended2Request xsi:type=""urn:CashCreateInvoiceExtended2Request"" xs:type =""type:CashCreateInvoiceExtended2Request"" xmlns:xs=""http://www.w3.org/2000/XMLSchema-instance"">
                        <cardForbidden xsi:type=""xsd:int"" xs:type=""type:int"">0</cardForbidden>
                             <userEmail xsi:type=""xsd:string"" xs:type=""type:string"">sample@test.com</userEmail>
                                      <userPhone xsi:type=""xsd:string"" xs:type=""type:string"">7765431515</userPhone>                              
                                           <referenceId xsi:type=""xsd:string"" xs:type=""type:string"">Qlksdjflskj03984209</referenceId>
                       <backUrl xsi:type=""xsd:string"" xs:type=""type:string"">https://www.test.wooppay.com/</backUrl>
            <requestUrl xsi:type=""xsd:string"" xs:type=""type:string"">https://www.test.wooppay.com/</requestUrl>
            <addInfo xsi:type=""xsd:string"" xs:type=""type:string""/>
                 <amount xsi:type=""xsd:float"" xs:type=""type:float"">100</amount>
                      <deathDate xsi:type=""xsd:string"" xs:type =""type:string""/>               
                           <serviceType xsi:type=""xsd:int"" xs:type=""type:int"">4</serviceType>
                                <description xsi:type=""xsd:string"" xs:type=""type:string"">nothing</description>
                                     <orderNumber xsi:type=""xsd:int"" xs:type=""type:int""/>
                                       </cashCreateInvoiceExtended2Request>
                                    </urn:cash_createInvoice2Extended>
                                  </soapenv:Body>
                                </soapenv:Envelope>");



            var dd = SOAPManual(@"<?xml version=""1.0"" encoding=""UTF-8""?>
<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ns1 =""urn:XmlControllerwsdl"" xmlns:xsd =""http://www.w3.org/2001/XMLSchema"" xmlns:xsi =""http://www.w3.org/2001/XMLSchema-instance"" xmlns:SOAP-ENC =""http://schemas.xmlsoap.org/soap/encoding/"" SOAP-ENV:encodingStyle =""http://schemas.xmlsoap.org/soap/encoding/"">
  <SOAP-ENV:Header>
    <ns1:RequestHeader
         SOAP-ENV:actor=""http://schemas.xmlsoap.org/soap/actor/next"" SOAP-ENV:mustUnderstand=""0"" xmlns:ns1=""urn:XmlControllerwsdl"">
       <ns1:session>fce534e1deafc4c281af41edcd9701e6</ns1:session>                 
    </ns1:RequestHeader>
  </SOAP-ENV:Header>
  <SOAP-ENV:Body>
     <ns1:cash_createInvoice>
      <cashCreateInvoiceRequest xsi:type =""ns1:CashCreateInvoiceRequest"">
        <referenceId xsi:type =""xsd:string"">384310</referenceId>
          <backUrl xsi:type =""xsd:string""></backUrl>
            <requestUrl xsi:type =""xsd:string""></requestUrl>
              <addInfo xsi:type=""xsd:string""></addInfo>
                <amount xsi:type= ""xsd:float"">100</amount>
                  <deathDate xsi:type=""xsd:string""> 2014 - 11 - 01 00:00:00 </deathDate>
                            <serviceType xsi:nil =""true""/>
                              <description xsi:type =""xsd:string""></description>
                                <orderNumber xsi:nil =""true""/>
                                  </cashCreateInvoiceRequest>
                                  </ns1:cash_createInvoice>
                                   </SOAP-ENV:Body>
                                      </SOAP-ENV:Envelope>");

            return Content(sop,"text/xml");
        }


        // GET: Test/Create
        public ActionResult Create()
        {
            string nulaccount = $@"<?xml version=""1.0"" encoding=""UTF-8""?>
            <response>
            <osmp_txn_id>5</osmp_txn_id>
            <result>5</result>
            <comment></comment>
            </response>";
            return Content(nulaccount, "text/xml");
        }

        // POST: Test/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Test/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Test/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Test/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Test/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}