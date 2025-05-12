import time
import psutil
from ram_detection import get_ram_info
from cpu_detection import get_cpu_info
from storage_detection import get_storage_info

def get_system_stats():
    """
    Get comprehensive system statistics including CPU, RAM, and storage information.
    
    Returns:
        dict: Dictionary containing system statistics
    """
    try:
        # Get RAM information
        total_ram, free_ram = get_ram_info()
        total_num = float(total_ram.split()[0])
        free_num = float(free_ram.split()[0])
        used_ram = f"{round(total_num - free_num, 2)} GB"
        ram_usage_percentage = round(((total_num - free_num) / total_num) * 100, 1)
        
        # Get CPU information
        cpu_name, cpu_cores, logical_processors, manufacturer = get_cpu_info()
        cpu_usage = round(psutil.cpu_percent(), 1)
        
        # Get storage information
        storage_drives = get_storage_info()
        
        # Format storage information for dashboard display
        storage_info = []
        for drive in storage_drives:
            device_id, total_size, free_space = drive
            total_num = float(total_size.split()[0])
            free_num = float(free_space.split()[0])
            used_space = f"{round(total_num - free_num, 2)} GB"
            usage_percentage = round(((total_num - free_num) / total_num) * 100, 1)
            
            storage_info.append({
                'drive': device_id,
                'total_size': total_size,
                'free_space': free_space,
                'used_space': used_space,
                'usage_percentage': usage_percentage
            })
        
        # Compile all system stats
        return {
            'timestamp': time.time(),
            'ram': {
                'total': total_ram,
                'free': free_ram,
                'used': used_ram,
                'usage_percentage': ram_usage_percentage
            },
            'cpu': {
                'name': cpu_name,
                'cores': cpu_cores,
                'logical_processors': logical_processors,
                'manufacturer': manufacturer,
                'usage_percentage': cpu_usage
            },
            'storage': storage_info
        }
    except Exception as e:
        print(f"Error getting system stats: {e}")
        return {
            'timestamp': time.time(),
            'error': str(e)
        }

if __name__ == "__main__":
    # Test the function
    stats = get_system_stats()
    print("System Stats:")
    print(f"RAM: {stats['ram']['used']} / {stats['ram']['total']} ({stats['ram']['usage_percentage']}%)")
    print(f"CPU: {stats['cpu']['name']} - Usage: {stats['cpu']['usage_percentage']}%")
    print("Storage:")
    for drive in stats['storage']:
        print(f"{drive['drive']}: {drive['used_space']} / {drive['total_size']} ({drive['usage_percentage']}%)")