import requests
import time
import json
import os

BASE_URL = "http://localhost:8000"
TEST_CONTAINER_ID = "test-container-1"

def print_response(response):
    print(f"Status Code: {response.status_code}")
    print(f"Response: {json.dumps(response.json(), indent=2)}")
    print("-" * 50)

def test_create_container():
    print("\n1. Testing Container Creation...")
    try:
        response = requests.post(
            f"{BASE_URL}/containers/{TEST_CONTAINER_ID}",
            json={
                "max_memory_mb": 512,
                "max_cpu_percent": 50
            }
        )
        print_response(response)
        return response.status_code == 200
    except Exception as e:
        print(f"Error creating container: {str(e)}")
        return False

def test_start_container():
    print("\n2. Testing Container Start...")
    try:
        # Create a simple Python script that uses some resources
        script_path = os.path.join(os.path.dirname(__file__), "test_script.py")
        with open(script_path, "w") as f:
            f.write("""
import time
import numpy as np

# Create a large array to use memory
arr = np.zeros((1000, 1000))

# Use CPU
while True:
    arr = np.random.rand(1000, 1000)
    time.sleep(0.1)
""")
        
        print(f"Created test script at: {script_path}")
        response = requests.post(
            f"{BASE_URL}/containers/{TEST_CONTAINER_ID}/start",
            json={
                "command": f"python {script_path}"
            }
        )
        print_response(response)
        return response.status_code == 200
    except Exception as e:
        print(f"Error starting container: {str(e)}")
        return False

def test_get_status():
    print("\n3. Testing Container Status...")
    try:
        # Wait a bit for the container to start using resources
        time.sleep(2)
        
        response = requests.get(f"{BASE_URL}/containers/{TEST_CONTAINER_ID}")
        print_response(response)
        return response.status_code == 200
    except Exception as e:
        print(f"Error getting container status: {str(e)}")
        return False

def test_stop_container():
    print("\n4. Testing Container Stop...")
    try:
        response = requests.post(f"{BASE_URL}/containers/{TEST_CONTAINER_ID}/stop")
        print_response(response)
        return response.status_code == 200
    except Exception as e:
        print(f"Error stopping container: {str(e)}")
        return False

def test_remove_container():
    print("\n5. Testing Container Removal...")
    try:
        response = requests.delete(f"{BASE_URL}/containers/{TEST_CONTAINER_ID}")
        print_response(response)
        return response.status_code == 200
    except Exception as e:
        print(f"Error removing container: {str(e)}")
        return False

def main():
    print("Starting Backend Tests...")
    print("=" * 50)
    
    # Run tests in sequence
    tests = [
        ("Create Container", test_create_container),
        ("Start Container", test_start_container),
        ("Get Status", test_get_status),
        ("Stop Container", test_stop_container),
        ("Remove Container", test_remove_container)
    ]
    
    all_passed = True
    for test_name, test_func in tests:
        print(f"\nRunning test: {test_name}")
        try:
            if not test_func():
                print(f"❌ {test_name} failed!")
                all_passed = False
            else:
                print(f"✅ {test_name} passed!")
        except Exception as e:
            print(f"❌ {test_name} failed with error: {str(e)}")
            all_passed = False
    
    print("\nTest Summary:")
    print("=" * 50)
    if all_passed:
        print("✅ All tests passed!")
    else:
        print("❌ Some tests failed!")

if __name__ == "__main__":
    main()