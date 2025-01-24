import random
import time

def get_soil_moisture():
    # Simulates soil moisture percentage (0-100%)
    return round(random.uniform(10, 90), 2)

def get_rain_sensor():
    # Simulates rain detection: 0 for no rain, 1 for rain
    return random.choice([0, 1])

def get_temperature():
    # Simulates temperature in Celsius (-10 to 40 degrees)
    return round(random.uniform(-10, 40), 2)

def get_humidity():
    # Simulates relative humidity percentage (20-100%)
    return round(random.uniform(20, 100), 2)

def get_air_quality():
    # Simulates air quality index (0-500, lower is better)
    return round(random.uniform(0, 500), 2)

def get_water_temperature():
    # Simulates water temperature in Celsius (0-30 degrees)
    return round(random.uniform(0, 30), 2)

def get_waterflow():
    # Simulates water flow in liters per minute (0-100 L/min)
    return round(random.uniform(0, 100), 2)

def print_mock_data():
    print("Mock Sensor Data:")
    print(f"Soil Moisture: {get_soil_moisture()}%")
    print(f"Rain Sensor: {'Rain Detected' if get_rain_sensor() else 'No Rain'}")
    print(f"Temperature: {get_temperature()} °C")
    print(f"Humidity: {get_humidity()}%")
    print(f"Air Quality Index: {get_air_quality()}")
    print(f"Water Temperature: {get_water_temperature()} °C")
    print(f"Water Flow: {get_waterflow()} L/min")

if __name__ == "__main__":
    while True:
        print_mock_data()
        print("-" * 30)
        time.sleep(5)  # Wait for 5 seconds before generating new data
