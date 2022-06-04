using System;
using System.Collections.Generic;

namespace Cw5.Models.DTO
{
    public class TripGetter
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int MaxPeople { get; set; }
        public IEnumerable<CountryGetter> Countries { get; set; }
        public IEnumerable<NameGetter> Clients { get; set; }
        

    }
}
