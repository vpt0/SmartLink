using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace SmartLink.Host.Services
{
    public class ResourceIsolationService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ResourceIsolationService> _logger;
        private const string BaseUrl = "http://localhost:8000";

        public ResourceIsolationService(ILogger<ResourceIsolationService> logger)
        {
            _httpClient = new HttpClient();
            _logger = logger;
        }

        public async Task<bool> CreateContainerAsync(string containerId, int maxMemoryMb, int maxCpuPercent)
        {
            try
            {
                var request = new
                {
                    max_memory_mb = maxMemoryMb,
                    max_cpu_percent = maxCpuPercent
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(request),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.PostAsync($"{BaseUrl}/containers/{containerId}", content);
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating container {ContainerId}", containerId);
                return false;
            }
        }

        public async Task<bool> StartContainerAsync(string containerId, string command)
        {
            try
            {
                var request = new { command };
                var content = new StringContent(
                    JsonSerializer.Serialize(request),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.PostAsync($"{BaseUrl}/containers/{containerId}/start", content);
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error starting container {ContainerId}", containerId);
                return false;
            }
        }

        public async Task<bool> StopContainerAsync(string containerId)
        {
            try
            {
                var response = await _httpClient.PostAsync($"{BaseUrl}/containers/{containerId}/stop", null);
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error stopping container {ContainerId}", containerId);
                return false;
            }
        }

        public async Task<ContainerStatus> GetContainerStatusAsync(string containerId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/containers/{containerId}");
                response.EnsureSuccessStatusCode();
                
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ContainerStatus>(content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting container status {ContainerId}", containerId);
                return null;
            }
        }

        public async Task<bool> RemoveContainerAsync(string containerId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{BaseUrl}/containers/{containerId}");
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing container {ContainerId}", containerId);
                return false;
            }
        }
    }

    public class ContainerStatus
    {
        public string Status { get; set; }
        public double MemoryUsageMb { get; set; }
        public double CpuPercent { get; set; }
        public int Pid { get; set; }
        public double RuntimeSeconds { get; set; }
        public ResourceLimits Limits { get; set; }
    }

    public class ResourceLimits
    {
        public int MaxMemoryMb { get; set; }
        public int MaxCpuPercent { get; set; }
    }
} 