using Microsoft.AspNetCore.Mvc;
using SmartLink.Host.Services;
using System.Threading.Tasks;

namespace SmartLink.Host.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResourceController : ControllerBase
    {
        private readonly ResourceIsolationService _resourceService;

        public ResourceController(ResourceIsolationService resourceService)
        {
            _resourceService = resourceService;
        }

        [HttpPost("containers/{containerId}")]
        public async Task<IActionResult> CreateContainer(string containerId, [FromBody] CreateContainerRequest request)
        {
            var success = await _resourceService.CreateContainerAsync(containerId, request.MaxMemoryMb, request.MaxCpuPercent);
            if (success)
                return Ok(new { message = "Container created successfully" });
            return BadRequest(new { message = "Failed to create container" });
        }

        [HttpPost("containers/{containerId}/start")]
        public async Task<IActionResult> StartContainer(string containerId, [FromBody] StartContainerRequest request)
        {
            var success = await _resourceService.StartContainerAsync(containerId, request.Command);
            if (success)
                return Ok(new { message = "Container started successfully" });
            return BadRequest(new { message = "Failed to start container" });
        }

        [HttpPost("containers/{containerId}/stop")]
        public async Task<IActionResult> StopContainer(string containerId)
        {
            var success = await _resourceService.StopContainerAsync(containerId);
            if (success)
                return Ok(new { message = "Container stopped successfully" });
            return BadRequest(new { message = "Failed to stop container" });
        }

        [HttpGet("containers/{containerId}")]
        public async Task<IActionResult> GetContainerStatus(string containerId)
        {
            var status = await _resourceService.GetContainerStatusAsync(containerId);
            if (status != null)
                return Ok(status);
            return NotFound(new { message = "Container not found" });
        }

        [HttpDelete("containers/{containerId}")]
        public async Task<IActionResult> RemoveContainer(string containerId)
        {
            var success = await _resourceService.RemoveContainerAsync(containerId);
            if (success)
                return Ok(new { message = "Container removed successfully" });
            return BadRequest(new { message = "Failed to remove container" });
        }
    }

    public class CreateContainerRequest
    {
        public int MaxMemoryMb { get; set; }
        public int MaxCpuPercent { get; set; }
    }

    public class StartContainerRequest
    {
        public string Command { get; set; }
    }
} 