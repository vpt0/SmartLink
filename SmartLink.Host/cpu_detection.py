import win32com.client
import pythoncom

def get_cpu_info():
    """
    Get CPU information using WMI.

    Returns:
        tuple: CPU name, number of cores, number of logical processors, and manufacturer.
    """
    pythoncom.CoInitialize()

    try:
        wmi = win32com.client.Dispatch("WbemScripting.SWbemLocator")
        wmi_service = wmi.ConnectServer(".", "root\\cimv2")

        query_result = wmi_service.ExecQuery(
            "SELECT Name, NumberOfCores, NumberOfLogicalProcessors, Manufacturer FROM Win32_Processor"
        )

        if len(query_result) == 0:
            raise Exception("No processor information found")

        cpu = query_result[0]
        name = cpu.Name.strip()
        cores = cpu.NumberOfCores
        logical_processors = cpu.NumberOfLogicalProcessors
        manufacturer = cpu.Manufacturer.strip()

        print(f"CPU: {name}")
        print(f"Cores: {cores}")
        print(f"Logical Processors: {logical_processors}")
        print(f"Manufacturer: {manufacturer}")

        if "Intel" in manufacturer:
            print("Processor Type: Intel")
        elif "AMD" in manufacturer:
            print("Processor Type: AMD")
        else:
            print("Processor Type: Unknown or Other")

        return name, cores, logical_processors, manufacturer
    except Exception as e:
        print(f"Error retrieving CPU information: {e}")
        return None
    finally:
        pythoncom.CoUninitialize()

if __name__ == "__main__":
    get_cpu_info()
