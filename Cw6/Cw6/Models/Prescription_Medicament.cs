using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cw6.Models
{
    public partial class Prescription_Medicament
    {
        public int IdMedicament { get; set; }
        public int IdPrescription { get; set; }
        public int Dose { get; set; }
        public String Details { get; set; }
        public Medicament Medicament { get; set; }
        public Prescription Prescription { get; set; }
    }
}
