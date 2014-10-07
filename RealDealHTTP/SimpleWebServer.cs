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
        for ( ; ; )
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
        String first = SocketReadLine(socket);
        if (first == null || first == "")
            return;
        Console.WriteLine("**New Request**");
        Console.WriteLine(first);

        // read in headers and post data
        String headerString = SocketRead(socket);
        Console.WriteLine(headerString);

        // write the HTTP response
        char[] delim = { ' ' };
        String[] tok = first.Split(delim);
        String verb = tok[0];
        String path = tok[1];
        String version = tok[2];

        if (String.Compare(verb, "GET", true) == 0)
            SendFile(socket, path);
        else
            Error(socket, 500, "Unsupported command");
        // close everything up
        socket.Close();
    }
    private void Transmit(Socket socket, int code, String message, byte[] body, String contentType)
    {
        StringBuilder headers = new StringBuilder();
        headers.Append("HTTP/1.1 ");
        headers.Append(code);
        headers.Append(' ');
        headers.Append(message);
        headers.Append("\n");
        headers.Append("Content-Length: " + body.Length + "\n");
        headers.Append("Server: Heaton Research Example Server\n");
        headers.Append("Connection: close\n");
        headers.Append("Content-Type: " + contentType + "\n");
        headers.Append("\n");
        socket.Send(encoding.GetBytes(headers.ToString()));
        socket.Send(body);
    }
    private void Error(Socket socket, int code, String message)
    {
        StringBuilder body = new StringBuilder();
        body.Append("<html><head><title>");
        body.Append(code + ":" + message);
        body.Append("</title></head><body><p>An error occurred.</p><h1>");
        body.Append(code);
        body.Append("</h1><p>");
        body.Append(message);
        body.Append("</p></body></html>");
        Transmit(socket, code, message, encoding.GetBytes(body.ToString()), "text/html");
    }
    private void AddSlash(StringBuilder path)
    {
        if (!path.ToString().EndsWith("\\"))
            path.Append("\\");
    }
    private String GetContent(String path)
    {
        path = path.ToLower();
        if (path.EndsWith(".jpg") || path.EndsWith(".jpeg"))
            return "image/jpeg";
        else if (path.EndsWith(".gif"))
            return "image/gif";
        else if (path.EndsWith(".png"))
            return "image/png";
        else
            return "text/html";
    }
    private void SendFile(Socket socket, String path)
    {
        char[] delim = { '/' };


        // parse the file by /'s and build a local file
        String[] tok = path.Split(delim);
        Console.WriteLine(path);
        StringBuilder physicalPath = new StringBuilder(httproot);
        AddSlash(physicalPath);

        foreach (String e in tok)
        {
            if (!e.Trim().Equals("\\"))
            {
                if (e.Equals("..") || e.Equals("."))
                {
                    Error(socket, 500, "Invalid request");
                    return;
                }
                AddSlash(physicalPath);
                physicalPath.Append(e);
            }
        }

        if (physicalPath.ToString().EndsWith("\\"))
        {
            physicalPath.Append("index.html");
        }

        String filename = physicalPath.ToString();
        // open the file and send it if it exists
        FileInfo file = new FileInfo(filename);
        if (file.Exists)
        {
            // send the file
            FileStream fis = File.Open(filename, FileMode.Open);
            byte[] buffer = new byte[(int)file.Length];
            fis.Read(buffer, 0, buffer.Length);
            fis.Close();
            this.Transmit(socket, 200, "OK", buffer, GetContent(filename));
        }
        // file does not exist, so send file not found
        else
        {
            this.Error(socket, 404, "File Not Found");
        }
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