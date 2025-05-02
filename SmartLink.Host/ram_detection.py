import win32com.client
import pythoncom

def get_ram_info():
    # Initialize COM library
    pythoncom.CoInitialize()

    wmi = win32com.client.Dispatch("WbemScripting.SWbemLocator")
    wmi_service = wmi.ConnectServer(".", "root\\cimv2")

    for os in wmi_service.ExecQuery("SELECT TotalVisibleMemorySize, FreePhysicalMemory FROM Win32_OperatingSystem"):
        total_ram_kb = int(os.TotalVisibleMemorySize)
        free_ram_kb = int(os.FreePhysicalMemory)

        # Convert from KB to GB
        total_ram_gb = total_ram_kb / 1024 / 1024
        free_ram_gb = free_ram_kb / 1024 / 1024

        # Format to 2 decimal places
        total_ram_gb = f"{total_ram_gb:.2f} GB"
        free_ram_gb = f"{free_ram_gb:.2f} GB"

        print(f"Total RAM: {total_ram_gb}")
        print(f"Available RAM: {free_ram_gb}")

        return total_ram_gb, free_ram_gb

if __name__ == "__main__":
    get_ram_info()