# Resource Isolation System

This is a Python-based resource isolation system that provides container-like functionality with memory and CPU limits.

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

## Running the Server

Start the FastAPI server:
```bash
python main.py
```

The server will run on `http://localhost:8000`

## API Endpoints

1. Create Container:
```bash
POST /containers/{container_id}
{
    "max_memory_mb": 512,
    "max_cpu_percent": 50
}
```

2. Start Container:
```bash
POST /containers/{container_id}/start
{
    "command": "python script.py"
}
```

3. Stop Container:
```bash
POST /containers/{container_id}/stop
```

4. Get Container Status:
```bash
GET /containers/{container_id}
```

5. Remove Container:
```bash
DELETE /containers/{container_id}
```

## API Documentation

Once the server is running, you can access the interactive API documentation at:
- Swagger UI: `http://localhost:8000/docs`
- ReDoc: `http://localhost:8000/redoc` 