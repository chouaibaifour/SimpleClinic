using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class clsMedicalRecord
    {
        private enum enMode { AddNew = 1, Update = 2 };
        enMode Mode = enMode.AddNew;

        public int MedicalRecordID {  get; set; }
        public string VisitDescription { get; set; }
        public string Diagnosis {  get; set; }
        public string AdditionalNotes { get; set; }

        public clsMedicalRecord()
        {
            Mode = enMode.AddNew;

            MedicalRecordID = -1;

            VisitDescription = "";

            Diagnosis = "";

            AdditionalNotes = "";

        }

        private clsMedicalRecord( int MedicalRecordID,  string VisitDescription, string Diagnosis, string AdditionalNotes)
        {
            this.MedicalRecordID = MedicalRecordID;
            this.VisitDescription = VisitDescription;
            this.Diagnosis = Diagnosis;
            this.AdditionalNotes = AdditionalNotes;
            Mode = enMode.Update;
        }

        static public clsMedicalRecord Find(int MedicalRecordID)
        {
            string VisitDescription = "", Diagnosis = "", AdditionalNotes = "";

            if (clsMedicalRecordDataAccess.GetMedicalRecordByID(ref MedicalRecordID, ref VisitDescription, ref Diagnosis, ref AdditionalNotes))
            {
                return new clsMedicalRecord(MedicalRecordID, VisitDescription, Diagnosis, AdditionalNotes);

            }
            else
                return null;
        }

        private bool _AddNewMedicalRecord()
        {
            this.MedicalRecordID=clsMedicalRecordDataAccess.AddNewMedicalRecord(VisitDescription, Diagnosis, AdditionalNotes);

            return (this.MedicalRecordID != -1);
        }

        private bool _UpdateMedicalRecord()
        {
            return clsMedicalRecordDataAccess.UpdateMedicalRecord(MedicalRecordID, VisitDescription, Diagnosis, AdditionalNotes);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewMedicalRecord())
                    {
                        Mode = enMode.Update;

                        return true;
                    }
                    else
                        return false;

                case enMode.Update:

                    return _UpdateMedicalRecord();

            }
            return false;
        }

        static public bool DeleteMedicalRecord(int MedicalRecordID)
        {

            return clsMedicalRecordDataAccess.DeleteMedicalRecord(MedicalRecordID);

        }

        static public DataTable GetAllMedicalRecords()
        {

            return clsMedicalRecordDataAccess.GetAllMedicalRecords();

        }

        static public bool isMedicalRecordExists(int MedicalRecordID)
        {

            return clsMedicalRecordDataAccess.isMedicalRecordExists(MedicalRecordID);

        }
    }
}
