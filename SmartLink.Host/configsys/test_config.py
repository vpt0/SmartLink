from config_summary import generate_config_summary, format_config_as_json

# Test with different configurations
config1 = generate_config_summary(4, 8.0)  # 4 cores, 8GB RAM
config2 = generate_config_summary([0, 1, 2, 3], 16.0)  # Specific cores, 16GB RAM

print("\nConfiguration 1:")
print(format_config_as_json(config1))

print("\nConfiguration 2:")
print(format_config_as_json(config2))