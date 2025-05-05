import win32com.client
import pythoncom

def get_storage_info():
    """
    Get storage information for all logical drives.

    Returns:
        list: List of tuples with (DeviceID, TotalSizeGB, FreeSpaceGB)
    """
    pythoncom.CoInitialize()

    try:
        wmi = win32com.client.Dispatch("WbemScripting.SWbemLocator")
        wmi_service = wmi.ConnectServer(".", "root\\cimv2")

        query_result = wmi_service.ExecQuery("SELECT DeviceID, Size, FreeSpace FROM Win32_LogicalDisk WHERE DriveType=3")

        if len(query_result) == 0:
            raise Exception("No logical disks found")

        drives = []
        for disk in query_result:
            device_id = disk.DeviceID
            size_gb = int(disk.Size) / 1024 / 1024 / 1024 if disk.Size else 0
            free_gb = int(disk.FreeSpace) / 1024 / 1024 / 1024 if disk.FreeSpace else 0
            print(f"{device_id} - Total: {size_gb:.2f} GB, Free: {free_gb:.2f} GB")
            drives.append((device_id, f"{size_gb:.2f} GB", f"{free_gb:.2f} GB"))

        return drives
    except Exception as e:
        print(f"Error retrieving storage information: {e}")
        return None
    finally:
        pythoncom.CoUninitialize()

if __name__ == "__main__":
    get_storage_info()
