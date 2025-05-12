using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Text.Json;
using System.Collections.Generic;

namespace SmartLink.Host
{
    public class SystemMonitorService
    {
        private readonly HttpClient _httpClient;
        private readonly DispatcherTimer _refreshTimer;
        private const string BaseApiUrl = "http://localhost:5000/api";

        public event EventHandler<SystemStatsEventArgs> SystemStatsUpdated;

        public SystemMonitorService()
        {
            _httpClient = new HttpClient();
            _refreshTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(5) // Update every 5 seconds
            };
            _refreshTimer.Tick += async (s, e) => await RefreshSystemStats();
        }

        public void StartMonitoring()
        {
            _refreshTimer.Start();
            // Initial refresh
            Task.Run(async () => await RefreshSystemStats());
        }

        public void StopMonitoring()
        {
            _refreshTimer.Stop();
        }

        private async Task RefreshSystemStats()
        {
            try
            {
                // Get RAM information
                var ramInfo = await GetRamInfo();
                
                // Raise event with the updated information
                SystemStatsUpdated?.Invoke(this, new SystemStatsEventArgs
                {
                    RamInfo = ramInfo
                });
            }
            catch (Exception ex)
            {
                // Log error or show message
                Console.WriteLine($"Error refreshing system stats: {ex.Message}");
            }
        }

        public async Task<RamInfo> GetRamInfo()
        {
            try
            {
                var response = await _httpClient.GetStringAsync($"{BaseApiUrl}/ram");
                var jsonResponse = JsonSerializer.Deserialize<ApiResponse<RamInfoData>>(response);

                if (jsonResponse?.Status == "success" && jsonResponse.Data != null)
                {
                    return new RamInfo
                    {
                        TotalRam = jsonResponse.Data.TotalRam,
                        FreeRam = jsonResponse.Data.FreeRam,
                        UsedRam = jsonResponse.Data.UsedRam,
                        UsagePercentage = jsonResponse.Data.UsagePercentage
                    };
                }

                return new RamInfo { TotalRam = "N/A", FreeRam = "N/A", UsedRam = "N/A", UsagePercentage = 0 };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting RAM info: {ex.Message}");
                return new RamInfo { TotalRam = "N/A", FreeRam = "N/A", UsedRam = "N/A", UsagePercentage = 0 };
            }
        }
    }

    public class SystemStatsEventArgs : EventArgs
    {
        public RamInfo RamInfo { get; set; }
    }

    public class RamInfo
    {
        public string TotalRam { get; set; }
        public string FreeRam { get; set; }
        public string UsedRam { get; set; }
        public double UsagePercentage { get; set; }
    }

    public class ApiResponse<T>
    {
        public string Status { get; set; }
        public T Data { get; set; }
    }

    public class RamInfoData
    {
        public string TotalRam { get; set; }
        public string FreeRam { get; set; }
        public string UsedRam { get; set; }
        public double UsagePercentage { get; set; }
    }
}