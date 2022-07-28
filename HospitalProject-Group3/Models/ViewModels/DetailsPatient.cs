using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProject_Group3.Models.ViewModels
{
    public class DetailsPatient
    {
        public PatientDto SelectedPatient { get; set; }


        
        public IEnumerable<AppointmentDto> MadeAppointments { get; set; }

        public IEnumerable<BillDto> MadeBills { get; set; }
        
        public IEnumerable<DonorTransplantDto> PlanedDonorTransplants { get; set; }

        public IEnumerable<MedicalRecordDto> HadMedicalRecords { get; set; }


        public IEnumerable<PrescriptionDto> HadPrescriptions { get; set; }

    }
}