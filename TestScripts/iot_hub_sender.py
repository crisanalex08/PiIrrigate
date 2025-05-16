from azure.iot.device import IoTHubDeviceClient, Message

# "C:%d, ID:%02X%02X%02X%02X%02X%02X, T:%d, H:%d, S:%d, R:%d\n"
def get_mock_data():
    # Mock data for testing
    data = {
        "C": 1,
        "ID": [0x08, 0xF9, 0xE0, 0xCE, 0x7B, 0x9C],  # Example ID: 08F9E0CE7B8C
        # 08F9E0CE7B8C
        "T": 25,
        "H": 60,
        "S": 100,
        "R": 200
    }
    return data
def send_message(client, data):
    # Create a message with the mock data
    message = Message(f"C:{data['C']}, ID:{data['ID'][0]:02X}{data['ID'][1]:02X}{data['ID'][2]:02X}{data['ID'][3]:02X}{data['ID'][4]:02X}{data['ID'][5]:02X}, T:{data['T']}, H:{data['H']}, S:{data['S']}, R:{data['R']}\n")
    
    # Send the message
    client.send_message(message)
    print("Message sent")


if __name__ == "__main__":
    # Connection string to your IoT Hub device
    connection_string = "HostName=AlexStudentIotHub.azure-devices.net;DeviceId=8dbcce2c-d64f-4c01-b2f4-f8285cd44a32;SharedAccessKey=T7GSTPNXFd2c8hVvOiaOchVfSP11T6hMKyfbiesD/Vk="

    # Create a client
    client = IoTHubDeviceClient.create_from_connection_string(connection_string)
    print("Client created")

    data = get_mock_data()
    send_message(client, data)
