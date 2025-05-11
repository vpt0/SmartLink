import json
import uuid
from datetime import datetime
from cpu_detection import get_cpu_info

def generate_config_summary(selected_cpu_cores, selected_ram_amount):
    """
    Generate a configuration summary based on selected CPU and RAM resources.
    
    Args:
        selected_cpu_cores (int or list): Number of CPU cores or list of specific core IDs
        selected_ram_amount (float): Amount of RAM in GB
        
    Returns:
        dict: Configuration summary as a dictionary
    """
    # Create timestamp for the configuration
    timestamp = datetime.now().strftime("%Y-%m-%d %H:%M:%S")
    
    # Get CPU system information
    cpu_name, total_cores, logical_processors, manufacturer = get_cpu_info()
    
    # Format CPU cores information
    if isinstance(selected_cpu_cores, list):
        cpu_info = {
            "count": len(selected_cpu_cores),
            "specific_cores": selected_cpu_cores,
            "processor_name": cpu_name,
            "total_cores": total_cores,
            "logical_processors": logical_processors,
            "manufacturer": manufacturer
        }
    else:
        cpu_info = {
            "count": selected_cpu_cores,
            "specific_cores": None,
            "processor_name": cpu_name,
            "total_cores": total_cores,
            "logical_processors": logical_processors,
            "manufacturer": manufacturer
        }
    
    # Create the configuration dictionary
    config = {
        "timestamp": timestamp,
        "resources": {
            "cpu": cpu_info,
            "ram": {
                "amount_gb": selected_ram_amount
            }
        },
        "status": "pending",  # Initial status before hypervisor initialization
        "session_id": str(uuid.uuid4())
    }
    
    return config

def format_config_as_json(config):
    """
    Format the configuration as a JSON string with indentation for readability.
    
    Args:
        config (dict): Configuration dictionary
        
    Returns:
        str: Formatted JSON string
    """
    return json.dumps(config, indent=4)

# Example usage
if __name__ == "__main__":
    # Example with number of cores
    config = generate_config_summary(4, 8.0)
    print(format_config_as_json(config))