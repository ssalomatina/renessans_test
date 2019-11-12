using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TestTask.Models;
using System.Xml;
using System.Net;
using System.IO;
using System.Text;

namespace TestTask.Controllers
{
    public class HomeController : Controller
    {
        List<ListValuts> valuts;
        List<Dynamic> dynamic;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(string date, string dateBegin, string dateEnd, string ID)
        {
            string dateRequestListValuts;
            DateTime Today = DateTime.Now;
            if (date != null)
            {
                dateRequestListValuts = date;
            }
            else
            {
                dateRequestListValuts = Today.ToString("dd/MM/yyyy");
            }
            DateTime date_for_input = DateTime.Parse(dateRequestListValuts);
            ViewData["Date_for_input"] = date_for_input.ToString("yyyy-MM-dd");
            if (ID == null)
            {
                ViewData["valuts-list"] = "true";
                RequestListValutsFromCB(dateRequestListValuts);
                return View(valuts);
            }
            else
            {
                ViewData["valuts-list"] = "false";
                RequestDynamicOfValuts(dateBegin, dateEnd, ID);
                return View(dynamic);
            }
        }
        public void RequestListValutsFromCB(string date)
        {
            valuts = new List<ListValuts>();
            XmlDocument xDoc = new XmlDocument();
            string ID = "";
            string NumCode = "";
            string CharCode = "";
            string Nominal = "";
            string Name = "";
            string Value = "";
            string url = "http://www.cbr.ru/scripts/XML_daily.asp?date_req=" + date;

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            HttpWebRequest req = WebRequest.CreateHttp(url);

            WebResponse resp = req.GetResponse();
            var stream = resp.GetResponseStream();
            StreamReader reader = new StreamReader(stream, Encoding.GetEncoding("windows-1251"));
            {
                xDoc.Load(reader);

            }
            XmlElement valCurs = xDoc.DocumentElement;
            XmlNode attrDate = valCurs.Attributes.GetNamedItem("Date");
            if (attrDate != null)
                ViewData["Date"] = attrDate.Value.ToString();
            foreach (XmlNode xnode in valCurs)
            {
                if (xnode.Attributes.Count > 0)
                {
                    XmlNode attrID = xnode.Attributes.GetNamedItem("ID");
                    if (attrID != null)
                    {
                        ID = attrID.Value.ToString();
                    }
                }
                foreach (XmlNode childNodeValute in xnode.ChildNodes)
                {
                    if (childNodeValute.Name == "NumCode")
                    {
                        NumCode = childNodeValute.InnerText;
                    }
                    if (childNodeValute.Name == "CharCode")
                    {
                        CharCode = childNodeValute.InnerText;
                    }
                    if (childNodeValute.Name == "Nominal")
                    {
                        Nominal = childNodeValute.InnerText;
                    }
                    if (childNodeValute.Name == "Name")
                    {
                        Name = childNodeValute.InnerText;
                    }
                    if (childNodeValute.Name == "Value")
                    {
                        Value = childNodeValute.InnerText;
                    }

                }
                valuts.Add(new ListValuts { ID = ID, NumCode = NumCode, CharCode = CharCode, Nominal = Nominal, Name = Name, Value = Value });
            }
        }

        public void RequestDynamicOfValuts(string dateBegin, string dateEnd, string IDValut)
        {
            dynamic = new List<Dynamic>();
            XmlDocument xDocDyn = new XmlDocument();
            string Date;
            string NominalDynamic = "";
            string ValueDynamic = "";
            string ID;
            DateTime dateRange1;
            DateTime dateRange2;
            string urlDynamic = "http://www.cbr.ru/scripts/XML_dynamic.asp?date_req1=" + dateBegin + "&date_req2=" + dateEnd + "&VAL_NM_RQ=" + IDValut;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            HttpWebRequest reqDynamic = WebRequest.CreateHttp(urlDynamic);

            using (var respDynamic = reqDynamic.GetResponse())
            using (var streamDynamic = respDynamic.GetResponseStream())
            using (var readerDynamic = new StreamReader(streamDynamic, Encoding.GetEncoding("windows-1251")))
            {
                xDocDyn.Load(readerDynamic);

            }
            XmlElement valCursDynamic = xDocDyn.DocumentElement;
            XmlNode attrDateRange1Dynamic = valCursDynamic.Attributes.GetNamedItem("DateRange1");
            dateRange1 = DateTime.Parse(attrDateRange1Dynamic.Value.ToString());
            XmlNode attrDateRange2Dynamic = valCursDynamic.Attributes.GetNamedItem("DateRange2");
            dateRange2 = DateTime.Parse(attrDateRange2Dynamic.Value.ToString());

            ViewData["DateRange1"] = dateRange1.ToString("yyyy-MM-dd");
            ViewData["DateRange2"] = dateRange2.ToString("yyyy-MM-dd");

            foreach (XmlNode xnodeDynamic in valCursDynamic)
            {
                XmlNode attrDateDynamic = xnodeDynamic.Attributes.GetNamedItem("Date");
                Date = attrDateDynamic.Value.ToString();
                XmlNode attrIDDynamic = xnodeDynamic.Attributes.GetNamedItem("Id");
                ID = attrIDDynamic.Value.ToString();
                foreach (XmlNode childNodeValute in xnodeDynamic.ChildNodes)
                {
                    if (childNodeValute.Name == "Nominal")
                    {
                        NominalDynamic = childNodeValute.InnerText;
                    }
                    if (childNodeValute.Name == "Value")
                    {
                        ValueDynamic = childNodeValute.InnerText;
                    }

                }
                dynamic.Add(new Dynamic { Date = Date, Nominal = NominalDynamic, ValueDynamic = ValueDynamic, ID = ID });
            }

        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
