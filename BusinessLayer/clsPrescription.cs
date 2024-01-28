using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class clsPrescription
    {
        private enum enMode { AddNew = 1, Update = 2 };
        enMode Mode = enMode.AddNew;
        public int PrescriptionID { get; set; }
        public clsMedicalRecord MedicalRecord { get; set; }
        public string MedicationName{ get; set; }
        public string Dosage { get; set; }
        public string Frequency { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string SpecialInstructions { get; set; }

        public clsPrescription()
        {
            PrescriptionID = -1;

            MedicalRecord = null;

            MedicationName = "";

            Dosage = "";

            Frequency = "";

            StartDate = DateTime.Now;

            EndDate = DateTime.Now;

            SpecialInstructions = "";

            Mode = enMode.AddNew;
        }

        private clsPrescription(int PrescriptionID, clsMedicalRecord MedicalRecord, string MedicationName, string Dosage,
             string Frequency, DateTime StartDate, DateTime EndDate, string SpecialInstructions)
        {
            this.Mode = enMode.Update;

            this.PrescriptionID = PrescriptionID;

            this.MedicalRecord = MedicalRecord;

            this.MedicationName = MedicationName;

            this.Dosage = Dosage;

            this.Frequency = Frequency;

            this.StartDate = StartDate;

            this.EndDate = EndDate;

            this.SpecialInstructions = SpecialInstructions;

        }

        static public clsPrescription Find(int PrescriptionID)
        {
            int MedicalRecordID = -1;

            string MedicationName = "",Dosage="",Frequency="",SpecialInstructions="";

            DateTime StartDate=DateTime.Now, EndDate = DateTime.Now;

            if (clsPrescriptionDataAccess.GetPrescriptionByID(ref PrescriptionID, ref MedicalRecordID, ref MedicationName, ref Dosage, ref Frequency,
                ref StartDate, ref EndDate, ref SpecialInstructions))

                return new clsPrescription(PrescriptionID, clsMedicalRecord.Find(MedicalRecordID), MedicationName, Dosage,
                        Frequency, StartDate, EndDate, SpecialInstructions);

            else

                return null;

        }

        private bool _AddNewPrescription()
        {
            this.PrescriptionID = clsPrescriptionDataAccess.AddNewPrescription(MedicalRecord.MedicalRecordID, MedicationName, Dosage,
                Frequency, StartDate, EndDate, SpecialInstructions);

            return (this.PrescriptionID != -1);

        }

        private bool _UpdatePrescription()
        {

            return clsPrescriptionDataAccess.UpdatePrescription(PrescriptionID, MedicalRecord.MedicalRecordID, MedicationName, Dosage,
                Frequency, StartDate, EndDate, SpecialInstructions);

        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewPrescription())
                    {
                        Mode = enMode.Update;

                        return true;
                    }
                    else
                        return false;

                case enMode.Update:

                    return _UpdatePrescription();

            }
            return false;
        }

        static public bool DeletePrescription(int PrescriptionID)
        {

            return clsPrescriptionDataAccess.DeletePrescription(PrescriptionID);

        }

        static public DataTable GetAllPrescriptions()
        {

            return clsPrescriptionDataAccess.GetAllPrescriptions();

        }

        static public bool isPrescriptionExists(int PrescriptionID)
        {

            return clsPrescriptionDataAccess.isPrescriptionExists(PrescriptionID);

        }


    }
}
