using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.IO;
class SimpleWebServer
{
    /// <summary>
    /// The server socket that will listen for connections
    /// </summary>
    private Socket server;
    private System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
    private String httproot;

    public SimpleWebServer(int port,string httproot)
    {
        server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
        server.Bind(new IPEndPoint(IPAddress.Loopback, port));
        this.httproot = httproot;
    }

    public void run()
    {
        server.Listen(10);
        while (true)
        {
            Socket socket = server.Accept();
            HandleClientSession(socket);
        }
    }
    private String SocketReadLine(Socket socket)
    {
        StringBuilder result = new StringBuilder();
        byte[] buffer = new byte[1];

        while (socket.Receive(buffer) > 0)
        {
            char ch = (char)buffer[0];
            if (ch == '\n')
                break;
            if (ch != '\r')
                result.Append(ch);
        }

        return result.ToString();

    }
    private String SocketRead(Socket socket) {
        StringBuilder result = new StringBuilder();
        StringBuilder line = new StringBuilder();
        byte[] buffer = new byte[1];
        do
        {
            while (socket.Receive(buffer) > 0)
            {
                char ch = (char)buffer[0];
                if (ch == '\n')
                    break;
                if (ch != '\r')
                    line.Append(ch);

            }

            if (line.ToString().Length>0 && line!=null)
            {
                line.Append('\n');
                result.Append(line);
                line.Clear();
            }
            else
            {
                break;
            }

        } while (true);
        return result.ToString();
    }

    private void SocketWrite(Socket socket, String str)
    {
        socket.Send(encoding.GetBytes(str));
        socket.Send(encoding.GetBytes("\r\n"));
    }
    private void HandleClientSession(Socket socket)
    {
        Console.WriteLine("**New Request**");
        String first = SocketReadLine(socket);
        Console.WriteLine(first);

        // read in headers and post data
        String headerString = SocketRead(socket);
        Console.WriteLine(headerString);

        // write the HTTP response
        SocketWrite(socket, "HTTP/1.1 200 OK");
        SocketWrite(socket, "");
        SocketWrite(socket, "<html>");
        SocketWrite(socket, "<head><title>Simple Web Server</title></head>");
        SocketWrite(socket, "<body>");
        SocketWrite(socket, "<h1> KONICHIWA SEKAI </h1>");
        SocketWrite(socket, "</body>");
        SocketWrite(socket, "</html>");

        // close everything up
        socket.Close();
    }

    [STAThread]
    static void Main(string[] args)
    {
        if (args.Length < 2) // invalid input if there are less than 2 arguments
            Console.WriteLine("Invalid Arguments! [http port root]");
        else
        {
            try
            {
                //main stuff

                SimpleWebServer webServer = new SimpleWebServer(Int32.Parse(args[0]), args[1]);
                webServer.run();

            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("Invalid port number");
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid port number");
            }
            catch (OverflowException)
            {
                Console.WriteLine("Invalid port number");
            }
        }
    }
}