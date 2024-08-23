using System.Net.Sockets;
using System.Text;

namespace MD.RPM.UI.Communication._Internal;

/// <summary>
/// Class for connecting to a TCP server and sending data.
/// </summary>
public class Connector
{
    private readonly TcpClient _client;

    public Connector(string ipAddress, int port)
    {
        _client = new TcpClient(ipAddress, port);
    }

    /// <summary>
    /// Send data to the connected TCP server.
    /// </summary>
    /// <param name="jsonData">The Json Data to send.</param>
    public void SendData(string jsonData)
    {
        try
        {
            NetworkStream stream = _client.GetStream();
            
            byte[] data = Encoding.UTF8.GetBytes(jsonData);
            stream.Write(data, 0, data.Length);
            Console.WriteLine("Sent data to the server.");
        }
        catch (SocketException ex)
        {
            Console.WriteLine($"Socket error while sanding data: {ex.Message}");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"IO error while sending data: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while sending data: {ex.Message}");
        }
    }
}