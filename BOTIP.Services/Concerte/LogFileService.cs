using BOTIP.Services.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BOTIP.Services.Concerte
{
    public class LogFileService : IBOTService
    {
        private readonly ILogger<LogFileService> _logger;
        private readonly IConfiguration configuration;

        // Here for process bot ipaddress we can use startegic design pattern
        public LogFileService(ILogger<LogFileService> logger,IConfiguration configuration)
        {
            this._logger = logger;
            this.configuration = configuration;
        }

        public Dictionary<string, int> Process(string path)
        {
            try
            {
                int requestThreshold = Convert.ToInt32(configuration["Setting:RequestThreshold"]);//100; // Threshold for bot detection
                int chunkSize = Convert.ToInt32(configuration["Setting:ChunkSize"]); //1024 * 1024; // 1 MB chunk size for reading

                var suspiciousIps = new Dictionary<string, int>();
             

                // Read log file in a memory-efficient way
                using (var reader = new StreamReader(path))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        // Extract IPs (handling multiple IPs in a line)
                        var parts = line.Split(' ')[0].Split(',');
                        foreach (var ip in parts)
                        {
                            var trimmedIp = ip.Trim();
                            if (!string.IsNullOrEmpty(trimmedIp))
                            {
                                if (suspiciousIps.ContainsKey(trimmedIp))
                                {
                                    suspiciousIps[trimmedIp]++;
                                }
                                else
                                {
                                    suspiciousIps[trimmedIp] = 1;
                                }
                            }
                        }
                    }
                }
                var suspiciousIpsFinal = new Dictionary<string, int>();
                // Identify and display suspicious IPs
                Console.WriteLine("Suspicious IPs (potential bots):");
                foreach (var ip in suspiciousIps.Where(kvp => kvp.Value > requestThreshold))
                {
                    // Console.WriteLine($"{ip.Key}: {ip.Value} requests");
                    suspiciousIpsFinal.Add(ip.Key,ip.Value);
                }

                return suspiciousIpsFinal;
            }
            catch(Exception ex)
            {
                this._logger.LogError("Issue in Processing for " + path);
                throw;
            }
        }

        // For processing in chunck if large but i didnot use in my implementation
        static void ProcessChunk(string chunk, Dictionary<string, int> suspiciousIps)
        {
            try
            {
                // Split the chunk into lines
                var lines = chunk.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var line in lines)
                {
                    // Extract IPs (handling multiple IPs in a line)
                    var parts = line.Split(' ')[0].Split(',');
                    foreach (var ip in parts)
                    {
                        var trimmedIp = ip.Trim();
                        if (!string.IsNullOrEmpty(trimmedIp))
                        {
                            if (suspiciousIps.ContainsKey(trimmedIp))
                            {
                                suspiciousIps[trimmedIp]++;
                            }
                            else
                            {
                                suspiciousIps[trimmedIp] = 1;
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw;
            }
        }


    }
}