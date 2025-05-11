# Resource Isolation System

This system provides a simulated container environment with resource isolation capabilities. It allows you to create, manage, and monitor isolated processes with specific resource limits.

## Features

- Create isolated containers with memory and CPU limits
- Start/stop containers with custom commands
- Monitor resource usage (CPU and memory)
- RESTful API for container management
- Process isolation and resource limiting

## Setup

1. Create a virtual environment:
```bash
python -m venv venv
source venv/bin/activate  # On Windows: venv\Scripts\activate
```

2. Install dependencies:
```bash
pip install -r requirements.txt
```

3. Run the application:
```bash
python main.py
```

The API will be available at `http://localhost:8000`

## API Endpoints

- `POST /containers/{container_id}` - Create a new container
- `POST /containers/{container_id}/start` - Start a container
- `POST /containers/{container_id}/stop` - Stop a container
- `GET /containers/{container_id}` - Get container status
- `GET /containers` - List all containers
- `DELETE /containers/{container_id}` - Remove a container

## Example Usage

1. Create a container:
```bash
curl -X POST "http://localhost:8000/containers/test-container" \
     -H "Content-Type: application/json" \
     -d '{"max_memory_mb": 512, "max_cpu_percent": 50}'
```

2. Start a container:
```bash
curl -X POST "http://localhost:8000/containers/test-container/start" \
     -H "Content-Type: application/json" \
     -d '{"command": "python your_script.py"}'
```

3. Check container status:
```bash
curl "http://localhost:8000/containers/test-container"
```

## Notes

- This is a simulation of container isolation and does not provide the same level of security as real containers
- Resource limits are enforced at the process level
- The system uses Windows Job Objects for process management on Windows systems 