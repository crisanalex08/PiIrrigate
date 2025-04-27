using Microsoft.EntityFrameworkCore;
using PiIrrigateServer.Database;
using PiIrrigateServer.Exceptions;
using PiIrrigateServer.Models;

namespace PiIrrigateServer.Repositories
{
    public interface IDeviceRepository
    {
        Task<Device> GetByIdAsync(Guid id);
        Task<Device> GetByMacAsync(string mac);
        Task<IEnumerable<Device>> GetAllAsync();
        Task<bool> CreateAsync(Device device);
        Task<bool> UpdateAsync(Device device);
        Task<bool> DeleteAsync(Guid id);
    }
    public class DeviceRepository : IDeviceRepository
    {
        private readonly ILogger<IDeviceRepository> logger;
        private readonly IServiceScopeFactory serviceScopeFactory;
        public DeviceRepository(ILogger<IDeviceRepository> logger,
                    IServiceScopeFactory serviceScopeFactory)
        {
            this.logger = logger;
            this.serviceScopeFactory = serviceScopeFactory;
        }
        public async Task<bool> CreateAsync(Device device)
        {
            try
            {
                using var scope = serviceScopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Check if a device with the same MAC address already exists  
                bool deviceExists = await dbContext.Devices.AnyAsync(d => d.Mac == device.Mac).ConfigureAwait(false);
                if (deviceExists)
                {
                    throw new DeviceAlreadyExistsException("The device already exists");
                }

                // Add the new device  
                await dbContext.Devices.AddAsync(device).ConfigureAwait(false);
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }
            catch (DeviceAlreadyExistsException)
            {
                // Log specific exception without re-logging the message  
                logger.LogWarning("Attempted to create a device that already exists: {Mac}", device.Mac);
                return false;
            }
            catch (Exception e)
            {
                logger.LogError(e, "An error occurred while creating a device with MAC {Mac}", device.Mac);
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                using var scope = serviceScopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var device = await dbContext.Devices.FindAsync(id).ConfigureAwait(false);
                if (device == null)
                {
                    logger.LogWarning("Device with ID {DeviceId} not found for deletion.", id);
                    return false;
                }

                dbContext.Devices.Remove(device);
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
                logger.LogInformation("Device with ID {DeviceId} successfully deleted.", id);
                return true;
            }
            catch (Exception e)
            {
                logger.LogError(e, "An error occurred while deleting the device with ID {DeviceId}.", id);
                return false;
            }
        }

        public async Task<IEnumerable<Device>> GetAllAsync()
        {
            try
            {
                using var scope = serviceScopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Fetch all devices as a list to ensure proper disposal of the DbContext  
                var devices = await dbContext.Devices.ToListAsync().ConfigureAwait(false);
                return devices;
            }
            catch (Exception e)
            {
                logger.LogError(e, "An error occurred while fetching all devices.");
                return Enumerable.Empty<Device>();
            }
        }

        public async Task<Device> GetByIdAsync(Guid id)
        {
            try
            {
                using var scope = serviceScopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Fetch the device by ID
                var device = await dbContext.Devices.FirstOrDefaultAsync(d => d.ZoneId == id).ConfigureAwait(false);

                return device;
            }
            catch (Exception e)
            {
                logger.LogError(e, "An error occurred while fetching the device with ID {DeviceId}", id);
                throw;
            }
        }

        public async Task<Device> GetByMacAsync(string mac)
        {
            try
            {
                using var scope = serviceScopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Fetch the device by mac
                var device = await dbContext.Devices.FirstOrDefaultAsync(d => d.Mac == mac).ConfigureAwait(false);

                return device;
            }
            catch (Exception e)
            {
                logger.LogError(e, "An error occurred while fetching the device with mac {Mac}", mac);
                throw;
            }
        }

        public async Task<bool> UpdateAsync(Device device)
        {
            try
            {
                using var scope = serviceScopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Fetch the existing device  
                var existingDevice = await dbContext.Devices.FirstOrDefaultAsync(d => d.ZoneId == device.ZoneId).ConfigureAwait(false);
                if (existingDevice == null)
                {
                    return false; // Device not found  
                }

                // Update the properties  
                existingDevice.Mac = device.Mac;
                existingDevice.Name = device.Name;
                existingDevice.Location = device.Location;
                existingDevice.Owner = device.Owner;
                existingDevice.Description = device.Description;
                existingDevice.IsRegistered = device.IsRegistered;

                // Save changes  
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return false;
            }
        }
    }
}
