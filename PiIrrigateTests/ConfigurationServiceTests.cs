using PiIrrigateServer.Services;

namespace PiIrrigateServer.Tests.Services
{
    [TestClass]
    public class ConfigurationServiceTests
    {
        [TestMethod]
        public void GetConfiguration_ShouldReturnDefaultConfiguration()
        {
            // Arrange
            var configurationService = new ConfigurationService();

            // Act
            var configuration = configurationService.GetConfiguration();

            // Assert
            Xunit.Assert.NotNull(configuration);
            Xunit.Assert.True(configuration.enableNotifications);
            Xunit.Assert.False(configuration.irrigationRunning);
            Xunit.Assert.Equal(70, configuration.soilMoistureHighThreshold);
            Xunit.Assert.Equal(30, configuration.soilMoistureLowThreshold);
            Xunit.Assert.Equal(10, configuration.rainDropLowThreshold);
            Xunit.Assert.Equal(50, configuration.rainDropHighThreshold);
            Xunit.Assert.Equal(5, configuration.waterFlowLowThreshold);
            Xunit.Assert.Equal(20, configuration.waterFlowHighThreshold);
            Xunit.Assert.Equal(30, configuration.waterTemperatureHighThreshold);
            Xunit.Assert.Equal(10, configuration.waterTemperatureLowThreshold);
        }
    }
}
