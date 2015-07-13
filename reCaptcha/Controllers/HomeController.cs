using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using reCaptcha.Models;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.IO;

namespace reCaptcha.Controllers
{
    public class HomeController : Controller
    {
        // VARIABLES
        private static Users _userList = new Users();


        // GET: /Home/
        public ActionResult Index()
        {
            return View(_userList._usrList);
        }


        // GET: /Home/Login
        public ActionResult Login()
        {
            return View();
        }


        // POST: /Home/Login/UserModel
        [HttpPost]
        public async Task<ViewResult> Login(UserModel um)
        {
            if (ModelState.IsValid)
            {
                if (await RecaptchaIsValid(Request.Form["g-recaptcha-response"]))
                {
                    // do login process here
                    return View("Success");
                }

                ModelState.AddModelError(
                    "invalid-recaptcha-response",
                    "Please answer the recaptcha challenge.");
            }
            //return View("Error");
            return View();
        }


        private async Task<bool> RecaptchaIsValid(string captchaResponse)
        {
            var requestUrl =
                String.Format(
                    "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}",
                    "6LcdQgkTAAAAAIKMbGwc-ZN5ubqhDsh3bF9Lpgy2",
                    captchaResponse);
            
            string result = null;
            
            //HttpHandler
            HttpClientHandler myHandler = new HttpClientHandler();
            myHandler.UseProxy = true;
            myHandler.Proxy = WebRequest.GetSystemWebProxy();


            System.Diagnostics.Debug.WriteLine("Proxy: " + myHandler.Proxy.ToString());
            //HttpClient
            
            using (var client = new HttpClient(myHandler))
            {
                var serverResult = await client.GetStringAsync(requestUrl);
                result = serverResult.ToString();
                client.Dispose();
            }
            

            /*
            var httpClient = new HttpClient(myHandler);
            try
            {
                var serverResult = await httpClient.GetStringAsync(requestUrl);
                result = serverResult.ToString();
                httpClient.Dispose();
            }
            catch(Exception ex)
            {
                httpClient.Dispose();
                System.Diagnostics.Debug.WriteLine("ERROR: " + ex.ToString());
            }*/
            


            if (!String.IsNullOrWhiteSpace(result))
            {
                var obj = JsonConvert.DeserializeObject<RecaptchaResponse>(result);
                if (obj.Success)
                {
                    return true;
                }
            }
            return false;
        }


        private class RecaptchaResponse
        {
            public bool Success { get; set; }

            [JsonProperty("error-codes")]
            public ICollection<string> ErrorCodes { get; set; }
            
            public RecaptchaResponse()
            {
                ErrorCodes = new HashSet<string>();
            }
        }


    }
}
