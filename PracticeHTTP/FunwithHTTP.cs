using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PracticeHTTP
{
    static class FunwithHTTP
    {
        public static void DoWebRequest()
        {
            WebRequest req = WebRequest.Create("http://www.albahari.com/nutshell/code.html");
            Console.WriteLine(req);

            HttpWebRequest wr = req as HttpWebRequest;
            Console.WriteLine(wr.Date);
            Console.WriteLine(wr.Headers);
            req.Proxy = null;
            using (WebResponse res = req.GetResponse())
            {
                //TODO find out whats in the get header in the request
                Console.WriteLine(res.Headers);

                HttpWebResponse wro = res as HttpWebResponse;


                using (Stream rs = res.GetResponseStream())
                {
                    using (FileStream fs = File.Create("C://code.html"))
                    {
                        rs.CopyTo(fs);
                        //System.Diagnostics.Process.Start(System.AppDomain.CurrentDomain.BaseDirectory+"website.html");
                    }
                }
            }
        }

        /// <summary>
        /// oloha
        /// </summary>
        public static void doHTTPClient()
        {
            FunwithHTTP.DoHTTP();
        }
        
        public static async void DoHTTP()
        {
            string html = await new HttpClient().GetStringAsync("http://linqpad.net");

            File.WriteAllText("C:/response.html", html);
            Console.WriteLine(html);
        }

        public static async void doHttpRequestMessage()
        {
            HttpClient client = new HttpClient();

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "http://www.reddit.com");

            HttpResponseMessage response = await client.SendAsync(request);

            response.EnsureSuccessStatusCode();

            using (FileStream fs = File.Create("C://async2String.html"))
            {
                await response.Content.CopyToAsync(fs);
                Console.WriteLine("Done writing");
                System.Diagnostics.Process.Start("C://async2String.html");
            }
        }

        public static async void doHTTPPost() {

            HttpClient client = new HttpClient(new HttpClientHandler() { UseProxy = false });

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:8080/test.php");

            var dict = new Dictionary<string, string>
{
{ "name", "cortex" },
{ "pass", "la'la" }
};

            request.Content = new FormUrlEncodedContent(dict);

            HttpResponseMessage response = await client.SendAsync(request);

            response.EnsureSuccessStatusCode();

            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }

        public static void DoSimpleRequest()
        {
            Uri u1 = new Uri("http://www.httprecipes.com");

            WebRequest http = HttpWebRequest.Create(u1);
            HttpWebResponse response = (HttpWebResponse)http.GetResponse();
            Stream stream = response.GetResponseStream();            
            StreamReader sr = new StreamReader(stream, System.Text.Encoding.ASCII);

            string result = sr.ReadToEnd();
            Console.WriteLine(result); 
        }


    }
}
