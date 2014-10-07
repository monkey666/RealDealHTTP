using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.IO;
class Program
{
    static void Main(string[] args)
    {
        WebClient wc = new WebClient();

        NameValueCollection data = new NameValueCollection(2);
        data.Add("user", "kwoth");
        data.Add("passwd", "lalal");
        byte[] response;
        response = wc.UploadValues("http://reddit.com", "POST", data);
        Console.WriteLine("done");

        Program.SaveToFile(Encoding.ASCII.GetString(response));
        Console.ReadKey();
    }
    public static void SaveToFile(string text){
       // if (File.Exists(@"D:\response.html"))
      File.Delete(@"D:\response.html");
      File.WriteAllText(@"D:\response.html", text);
    }
}
