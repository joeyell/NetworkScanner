using System.Net.NetworkInformation;

namespace NetworkScanner
{
   internal static class Program
    {
        private static async Task Main(string[] args)
        {
            Console.WriteLine("Welcome to my Network Scanner");

            //Ask user for IP range
            Console.WriteLine("Enter the base IP range to scan e.g. (192.168.1):");
            var baseIp = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(baseIp))
            {
                Console.WriteLine("Invalid input. Please restart the program");
                return;
            };

            Console.WriteLine($"Scanning IP range: {baseIp}.1 to {baseIp}.254...");

            var tasks = new Task[254];
            for (var i = 1; i <= 254; i++)
            {
                var ip = $"{baseIp}.{i}";
                tasks[i - 1] = Task.Run(() => ScanIp(ip));
            }

            await Task.WhenAll(tasks);
            Console.WriteLine("Scanning completed.");
        }

        private static void ScanIp(string ipAddress)
        {
            try
            {
                if (!System.Net.IPAddress.TryParse(ipAddress, out _))
                {
                    Console.WriteLine($"Invalid IP address: {ipAddress}");
                    return;
                };
                
                Ping ping = new();
                var reply = ping.Send(ipAddress, 1000);
                if (reply.Status == IPStatus.Success)
                {
                    Console.WriteLine($"Device found: {ipAddress} (Ping: {reply.RoundtripTime} ms)");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error scanning {ipAddress}: {ex.Message}");
            }
        }
    }
}
