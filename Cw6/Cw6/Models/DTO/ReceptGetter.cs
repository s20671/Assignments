using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Cw6.Models.DTO
{
    public class ReceptGetter
    {
        public string FirstNameClient { get; set; }
        public string LastNameClient { get; set; }
        public DateTime Birthdate { get; set; }
        public string FirstNameDoctor { get; set; }
        public string LastNameDoctor { get; set; }
        public IEnumerable<MedicineGetter> Medicines { get; set; }
        

    }
}
