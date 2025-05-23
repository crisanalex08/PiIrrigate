﻿using Microsoft.EntityFrameworkCore;
using PiIrrigateServer.Database;
using PiIrrigateServer.Exceptions;
using PiIrrigateServer.Models;

namespace PiIrrigateServer.Repositories
{
    public interface IZoneRepository
    {
        public Task<bool> CreateZone(Zone zone);
        public Task<bool> UpdateZone(Guid Id, string Name, Guid userId);
        public Task<bool> DeleteZone(Guid Id);
        public Task<Zone> GetZoneById(Guid Id);
        public Task<Zone> GetZoneByName(string name);
        public Task<IEnumerable<Zone>> GetAllByUserId(Guid Id);

    }
    public class ZoneRepository : IZoneRepository
    {
        private ILogger<IZoneRepository> logger;
        private IServiceScopeFactory serviceScopeFactory;

        public ZoneRepository(ILogger<IZoneRepository> logger,
                    IServiceScopeFactory serviceScopeFactory)
        {
            this.logger = logger;
            this.serviceScopeFactory = serviceScopeFactory;
        }
        public async Task<bool> CreateZone(Zone zone)
        {
            try
            {
                using var scope = serviceScopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Check if a zone with the same ID already exists  
                bool zoneExists = await dbContext.Zones.AnyAsync(z => z.ZoneId == zone.ZoneId).ConfigureAwait(false);
                if (zoneExists)
                {
                    throw new ZoneAlreadyExistsException("The zone already exists");
                }

                // Add the new zone  
                await dbContext.Zones.AddAsync(zone).ConfigureAwait(false);
                await dbContext.SaveChangesAsync().ConfigureAwait(false);

                return true;
            }
            catch (ZoneAlreadyExistsException)
            {
                logger.LogWarning("Attempted to create a zone that already exists: {ZoneId}", zone.ZoneId);
                return false;
            }
            catch (Exception e)
            {
                logger.LogError(e, "An error occurred while creating a zone with ID {ZoneId}", zone.ZoneId);
                return false;
            }
        }

        public async Task<bool> DeleteZone(Guid Id)
        {
            try
            {
                using var scope = serviceScopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Find the zone by ID
                var zone = await dbContext.Zones.FirstOrDefaultAsync(z => z.ZoneId == Id).ConfigureAwait(false);
                if (zone == null)
                {
                    throw new ZoneNotFoundException($"Zone with ID {Id} not found");
                }

                // Remove the zone
                dbContext.Zones.Remove(zone);
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }
            catch (ZoneNotFoundException ex)
            {
                logger.LogWarning(ex, "Attempted to delete a zone that does not exist: {ZoneId}", Id);
                return false;
            }
            catch (Exception e)
            {
                logger.LogError(e, "An error occurred while deleting a zone with ID {ZoneId}", Id);
                return false;
            }
        }

        public async Task<IEnumerable<Zone>> GetAllByUserId(Guid Id)
        {
            try
            {
                using var scope = serviceScopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Find the zone by ID
                var zones = await dbContext.Zones.Where(z => z.UserId == Id).ToListAsync();
                if (zones.Count == 0)
                {
                    throw new ZoneNotFoundException($"Zone with ID {Id} not found");
                }

                return zones;
            }
            catch (ZoneNotFoundException ex)
            {
                logger.LogWarning(ex, "Zone with ID {ZoneId} not found", Id);
                throw;
            }
            catch (Exception e)
            {
                logger.LogError(e, "An error occurred while retrieving a zone with ID {ZoneId}", Id);
                throw;
            }
        }

        public async Task<Zone> GetZoneById(Guid Id)
        {
            try
            {
                using var scope = serviceScopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Find the zone by ID
                var zone = await dbContext.Zones.FirstOrDefaultAsync(z => z.ZoneId == Id).ConfigureAwait(false);
                if (zone == null)
                {
                    throw new ZoneNotFoundException($"Zone with ID {Id} not found");
                }

                return zone;
            }
            catch (Exception e)
            {
                logger.LogError(e, "An error occurred while retrieving a zone with ID {ZoneId}", Id);
                throw;
            }
        }

        public async Task<Zone> GetZoneByName(string name)
        {
            try
            {
                using var scope = serviceScopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Find the zone by ID
                var zone = await dbContext.Zones.FirstOrDefaultAsync(z => z.Name == name).ConfigureAwait(false);
                if (zone == null)
                {
                    throw new ZoneNotFoundException($"Zone with name {name} not found");
                }

                return zone;
            }
            catch (ZoneNotFoundException ex)
            {
                logger.LogWarning(ex, "Zone with name {ZoneId} not found", name);
                throw;
            }
            catch (Exception e)
            {
                logger.LogError(e, "An error occurred while retrieving a zone with name {ZoneId}", name);
                throw;
            }
        }

        public async Task<bool> UpdateZone(Guid Id, string Name, Guid userId)
        {
            try
            {
                using var scope = serviceScopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Find the zone by ID
                var zone = await dbContext.Zones.FirstOrDefaultAsync(z => z.ZoneId == Id).ConfigureAwait(false);
                if (zone == null)
                {
                    throw new ZoneNotFoundException($"Zone with ID {Id} not found");
                }

                // Update the zone's name
                zone.Name = Name;
                zone.UserId = userId;
                dbContext.Zones.Update(zone);
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }
            catch (ZoneNotFoundException ex)
            {
                logger.LogWarning(ex, "Attempted to update a zone that does not exist: {ZoneId}", Id);
                return false;
            }
            catch (Exception e)
            {
                logger.LogError(e, "An error occurred while updating a zone with ID {ZoneId}", Id);
                return false;
            }
        }
    }
}
