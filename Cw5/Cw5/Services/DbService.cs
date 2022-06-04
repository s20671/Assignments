using Cw5.Models;
using Cw5.Models.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw5.Services
{
    public class DbService : IDbService
    {
        private readonly _2019SBDContext _dbContext;
        public DbService(_2019SBDContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<TripGetter>> GetTripList()
        {
            return await _dbContext.Trips
                .Include(e => e.CountryTrips)
                .Include(e => e.ClientTrips)
                .Select(e => new TripGetter
                {
                    Name = e.Name,
                    Description = e.Description,
                    MaxPeople = e.MaxPeople,
                    DateFrom = e.DateFrom,
                    DateTo = e.DateTo,
                    Countries = e.CountryTrips.Select(e => new CountryGetter { Name = e.IdCountryNavigation.Name }).ToList(),
                    Clients = e.ClientTrips.Select(e => new NameGetter { FirstName = e.IdClientNavigation.FirstName, LastName = e.IdClientNavigation.LastName }).ToList()
                }).OrderByDescending(e => e.DateFrom).ToListAsync();
        }
        public async Task<int> RemoveClient(int idClient)
        {
            int result;
            var tripsforClient = _dbContext.ClientTrips.Where(e => e.IdClient == idClient).FirstOrDefault();
            if (tripsforClient == null)
            {
                var client = _dbContext.Clients.Where(e => e.IdClient == idClient).FirstOrDefault();
                if (client != null) {
                    _dbContext.Clients.Remove(client);
                    await _dbContext.SaveChangesAsync();
                    result = 0;
                }
                else {
                    result = 1;
                }
            }
            else
            {
                result = 1;
            }
            return result;

        }

        public async Task<int> AddClienttoTrip(ClientTrips clientTrip)
        {
            int result;
            var e = _dbContext.Clients.Where(c => c.Pesel == clientTrip.Pesel).FirstOrDefault();
            if (e == null)
            {
                var addClient = new Client()
                {
                    FirstName = clientTrip.FirstName,
                    LastName = clientTrip.LastName,
                    Email = clientTrip.Email,
                    Telephone = clientTrip.Telephone,
                    Pesel = clientTrip.Pesel
                };
                _dbContext.Clients.Add(addClient);
                await _dbContext.SaveChangesAsync();
            }
            var tripExist = _dbContext.Trips.Where(t => t.IdTrip == clientTrip.IdTrip).FirstOrDefault();
            if (tripExist != null)
            {
                var idData = _dbContext.Clients.FirstOrDefault(c => c.Pesel == clientTrip.Pesel);
                var tripData = _dbContext.ClientTrips.Where(e => e.IdTrip == clientTrip.IdTrip && e.IdClient == idData.IdClient).FirstOrDefault();
                if (tripData == null)
                {
                    var createClientTrip = new ClientTrip()
                    {
                        IdTrip = clientTrip.IdTrip,
                        IdClient = idData.IdClient,
                        RegisteredAt = DateTime.Now,
                        PaymentDate = clientTrip.PaymentDate
                        
                    };
                    _dbContext.ClientTrips.Add(createClientTrip);
                    await _dbContext.SaveChangesAsync();
                    result = 2;
                }
                else
                {
                    result = 1;
                }
            }
            else 
            {
                result = 0;
            }
            return result;
        }
    }
}
