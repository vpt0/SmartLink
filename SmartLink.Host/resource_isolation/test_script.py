
import time
import numpy as np

# Create a large array to use memory
arr = np.zeros((1000, 1000))

# Use CPU
while True:
    arr = np.random.rand(1000, 1000)
    time.sleep(0.1)
