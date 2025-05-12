import win32com.client
import pythoncom

def get_ram_info():
    """
    Get system RAM information using WMI.
    
    Returns:
        tuple: (total_ram, available_ram) as formatted strings with GB units
        
    Raises:
        Exception: If WMI query fails or returns no results
    """
    pythoncom.CoInitialize()

    try:
        wmi = win32com.client.Dispatch("WbemScripting.SWbemLocator")
        wmi_service = wmi.ConnectServer(".", "root\\cimv2")

        query_result = wmi_service.ExecQuery("SELECT TotalVisibleMemorySize, FreePhysicalMemory FROM Win32_OperatingSystem")

        # Check if we have results
        if len(query_result) == 0:
            raise Exception("No operating system information found")
            
        # Get the first OS instance
        os = query_result[0]
        
        # Convert from KB to GB
        total_ram_kb = int(os.TotalVisibleMemorySize)
        free_ram_kb = int(os.FreePhysicalMemory)
        
        # Convert from KB to GB
        total_ram_gb = total_ram_kb / 1024 / 1024
        free_ram_gb = free_ram_kb / 1024 / 1024
        
        # Format to 2 decimal places
        total_ram_formatted = f"{total_ram_gb:.2f} GB"
        free_ram_formatted = f"{free_ram_gb:.2f} GB"
        
        print(f"Total RAM: {total_ram_formatted}")
        print(f"Available RAM: {free_ram_formatted}")
        
        return total_ram_formatted, free_ram_formatted
    except Exception as e:
        print(f"Error retrieving RAM information: {e}")
        return None
    finally:
        pythoncom.CoUninitialize()

if __name__ == "__main__":
    get_ram_info()