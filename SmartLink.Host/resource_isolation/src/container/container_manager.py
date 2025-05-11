import psutil
import os
import sys
import joblib
from typing import Dict, Optional
from dataclasses import dataclass
import logging
import traceback
import subprocess
import time

@dataclass
class ResourceLimits:
    max_memory_mb: int
    max_cpu_percent: int

class ContainerManager:
    def __init__(self):
        self.containers: Dict[str, Dict] = {}
        self.logger = logging.getLogger(__name__)
        self.logger.setLevel(logging.DEBUG)
        # Add console handler
        console_handler = logging.StreamHandler()
        console_handler.setLevel(logging.DEBUG)
        formatter = logging.Formatter('%(asctime)s - %(name)s - %(levelname)s - %(message)s')
        console_handler.setFormatter(formatter)
        self.logger.addHandler(console_handler)
        self.logger.info("ContainerManager initialized")

    def create_container(self, container_id: str, resource_limits: ResourceLimits) -> bool:
        """Create a new container with specified resource limits."""
        try:
            self.logger.info(f"Creating container {container_id} with limits: {resource_limits}")
            if container_id in self.containers:
                self.logger.error(f"Container {container_id} already exists")
                return False

            self.containers[container_id] = {
                'limits': resource_limits,
                'process': None,
                'status': 'created',
                'start_time': None,
                'subprocess': None
            }
            self.logger.info(f"Container {container_id} created successfully. Current containers: {list(self.containers.keys())}")
            return True
        except Exception as e:
            self.logger.error(f"Error creating container {container_id}: {str(e)}")
            self.logger.error(f"Traceback: {traceback.format_exc()}")
            return False

    def start_container(self, container_id: str, command: str) -> bool:
        """Start a container with the specified command."""
        try:
            self.logger.info(f"Starting container {container_id} with command: {command}")
            if container_id not in self.containers:
                self.logger.error(f"Container {container_id} does not exist. Available containers: {list(self.containers.keys())}")
                return False

            container = self.containers[container_id]
            limits = container['limits']
            
            self.logger.info(f"Resource limits: {limits}")

            # Create a new process with resource limits using subprocess
            process = subprocess.Popen(
                command,
                shell=True,
                stdout=subprocess.PIPE,
                stderr=subprocess.PIPE,
                text=True,
                creationflags=subprocess.CREATE_NEW_PROCESS_GROUP
            )

            # Store both the subprocess and psutil process
            container['subprocess'] = process
            container['process'] = psutil.Process(process.pid)
            container['status'] = 'running'
            container['start_time'] = time.time()
            
            self.logger.info(f"Container {container_id} started successfully with PID: {process.pid}")
            self.logger.info(f"Current containers: {list(self.containers.keys())}")
            return True
        except Exception as e:
            self.logger.error(f"Error starting container {container_id}: {str(e)}")
            self.logger.error(f"Traceback: {traceback.format_exc()}")
            return False

    def get_container_status(self, container_id: str) -> Optional[Dict]:
        """Get the current status and resource usage of a container."""
        try:
            self.logger.info(f"Getting status for container {container_id}")
            self.logger.info(f"Available containers: {list(self.containers.keys())}")
            
            if container_id not in self.containers:
                self.logger.error(f"Container {container_id} does not exist")
                return None

            container = self.containers[container_id]
            process = container['process']
            subprocess = container['subprocess']

            self.logger.info(f"Container state: {container}")

            # Check if process is still running
            if subprocess and subprocess.poll() is None and process and process.is_running():
                try:
                    # Get memory info
                    memory_info = process.memory_info()
                    memory_mb = memory_info.rss / (1024 * 1024)  # Convert to MB
                    
                    # Get CPU percent
                    cpu_percent = process.cpu_percent(interval=0.1)
                    
                    # Get process details
                    status = {
                        'status': container['status'],
                        'memory_usage_mb': round(memory_mb, 2),
                        'cpu_percent': round(cpu_percent, 2),
                        'limits': {
                            'max_memory_mb': container['limits'].max_memory_mb,
                            'max_cpu_percent': container['limits'].max_cpu_percent
                        },
                        'pid': process.pid,
                        'runtime_seconds': round(time.time() - container['start_time'], 2)
                    }
                    self.logger.info(f"Container {container_id} status: {status}")
                    return status
                except (psutil.NoSuchProcess, psutil.AccessDenied) as e:
                    self.logger.error(f"Error getting process info: {str(e)}")
                    container['status'] = 'error'
                    return {'status': 'error', 'error': str(e)}
            else:
                container['status'] = 'stopped'
                return {'status': 'stopped'}
            
        except Exception as e:
            self.logger.error(f"Error getting container status {container_id}: {str(e)}")
            self.logger.error(f"Traceback: {traceback.format_exc()}")
            return None

    def stop_container(self, container_id: str) -> bool:
        """Stop a running container."""
        try:
            self.logger.info(f"Stopping container {container_id}")
            self.logger.info(f"Available containers: {list(self.containers.keys())}")
            
            if container_id not in self.containers:
                self.logger.error(f"Container {container_id} does not exist")
                return False

            container = self.containers[container_id]
            if container['subprocess']:
                self.logger.info(f"Terminating process for container {container_id}")
                container['subprocess'].terminate()
                container['subprocess'].wait(timeout=5)
                container['status'] = 'stopped'
                self.logger.info(f"Container {container_id} stopped successfully")
                return True
            self.logger.warning(f"No process found for container {container_id}")
            return False
        except Exception as e:
            self.logger.error(f"Error stopping container {container_id}: {str(e)}")
            self.logger.error(f"Traceback: {traceback.format_exc()}")
            return False

    def remove_container(self, container_id: str) -> bool:
        """Remove a container completely."""
        try:
            self.logger.info(f"Removing container {container_id}")
            self.logger.info(f"Available containers: {list(self.containers.keys())}")
            
            if container_id not in self.containers:
                self.logger.error(f"Container {container_id} does not exist")
                return False

            # Stop the container if it's running
            self.stop_container(container_id)
            
            # Remove from containers dict
            del self.containers[container_id]
            self.logger.info(f"Container {container_id} removed successfully")
            self.logger.info(f"Remaining containers: {list(self.containers.keys())}")
            return True
        except Exception as e:
            self.logger.error(f"Error removing container {container_id}: {str(e)}")
            self.logger.error(f"Traceback: {traceback.format_exc()}")
            return False