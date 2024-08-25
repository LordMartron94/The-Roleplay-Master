using System.Net.Sockets;
using System.Text;

namespace MD.RPM.UI.Communication._Internal;

/// <summary>
/// Class for connecting to a TCP server and sending data.
/// </summary>
internal class Connector
{
    private readonly TcpClient _client;
    private readonly ConnectionExceptionHandler _exceptionHandler;

    public Connector(string ipAddress, int port)
    {
        _client = new TcpClient(ipAddress, port);
        _exceptionHandler = new ConnectionExceptionHandler();
    }

    private async Task<string?> ReceiveData()
    {
        try
        {
            NetworkStream stream = _client.GetStream();
            byte[] data = new byte[1024];
            int bytesRead = await stream.ReadAsync(data);

            if (bytesRead == 0)
            {
                Console.WriteLine("The server has closed the connection.");
                return null;
            }

            string response = Encoding.UTF8.GetString(data, 0, bytesRead);
            return response;
        }
        catch (Exception ex)
        {
            _exceptionHandler.HandleException(ex);
        }
        
        return null;
    }

    /// <summary>
    /// Send data to the connected TCP server.
    /// </summary>
    /// <param name="jsonData">The Json Data to send.</param>
    public async Task<string?> SendData(string jsonData)
    {
        try
        {
            NetworkStream stream = _client.GetStream();

            byte[] data = Encoding.UTF8.GetBytes(jsonData);
            await stream.WriteAsync(data);
            Console.WriteLine("Sent data to the server.");

            string? response = await ReceiveData();
            return response;
        }
        catch (Exception ex)
        {
            _exceptionHandler.HandleException(ex);
        }

        return null;
    }
}