from fastapi import FastAPI, HTTPException
from pydantic import BaseModel
from typing import Optional
from ..container.container_manager import ContainerManager, ResourceLimits

app = FastAPI()
container_manager = ContainerManager()

class CreateContainerRequest(BaseModel):
    max_memory_mb: int
    max_cpu_percent: int

class StartContainerRequest(BaseModel):
    command: str

@app.post("/containers/{container_id}")
async def create_container(container_id: str, request: CreateContainerRequest):
    resource_limits = ResourceLimits(
        max_memory_mb=request.max_memory_mb,
        max_cpu_percent=request.max_cpu_percent
    )
    success = container_manager.create_container(container_id, resource_limits)
    if not success:
        raise HTTPException(status_code=400, detail="Failed to create container")
    return {"message": "Container created successfully"}

@app.post("/containers/{container_id}/start")
async def start_container(container_id: str, request: StartContainerRequest):
    success = container_manager.start_container(container_id, request.command)
    if not success:
        raise HTTPException(status_code=400, detail="Failed to start container")
    return {"message": "Container started successfully"}

@app.post("/containers/{container_id}/stop")
async def stop_container(container_id: str):
    success = container_manager.stop_container(container_id)
    if not success:
        raise HTTPException(status_code=400, detail="Failed to stop container")
    return {"message": "Container stopped successfully"}

@app.get("/containers/{container_id}")
async def get_container_status(container_id: str):
    status = container_manager.get_container_status(container_id)
    if status is None:
        raise HTTPException(status_code=404, detail="Container not found")
    return status

@app.delete("/containers/{container_id}")
async def remove_container(container_id: str):
    success = container_manager.remove_container(container_id)
    if not success:
        raise HTTPException(status_code=400, detail="Failed to remove container")
    return {"message": "Container removed successfully"}