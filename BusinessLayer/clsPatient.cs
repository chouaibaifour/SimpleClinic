using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class clsPatient : clsPerson
    {
        private enum enMode { AddNew = 1, Update = 2 };
        enMode Mode = enMode.AddNew;
        public int PatientID { get; set; }

        clsPatient() : base()
        {
            this.PatientID = -1;
            this.Mode = enMode.AddNew;
            
        }

        private clsPatient(int PatientID, int PersonID, string FirstName, string LastName, DateTime DateOfBirth,
            clsPersonDataAccess.enGender Gender, string PhoneNumber, string Password, string Email, string Address) 
            : base(PersonID,FirstName,LastName,DateOfBirth,Gender,PhoneNumber,Password,Email,Address)
        {
            
            this.PatientID = PatientID;

            this.Mode = enMode.Update;
        }

        static public new clsPatient Find(int PatientID)
        {

            int PersonID = -1;

            if (clsPatientDataAccess.GetPatientByID(ref PatientID, ref PersonID))
            {   clsPerson person = clsPerson.Find(PersonID);
                if (person != null)
                {
                   
                    clsPatient patient = (clsPatient)person;
                    patient.PatientID = PatientID;

                    return new clsPatient(patient.PatientID, patient.PersonID, patient.FirstName, patient.LastName,
                        patient.DateOfBirth, patient.Gender, patient.PhoneNumber, patient.Password, patient.Email, patient.Address);
                }
                else
                {
                    return null;
                }
            }
            else
                return null;
        }

         private bool _AddNewPatient()
        {
            base.Save();
            this.PatientID = clsPatientDataAccess.AddNewPatient(base.PersonID);
                return (this.PatientID!=0); 
        }

        private bool _UpdatePatient()
        {
            base.Save();
            return clsPatientDataAccess.UpdatePatient(PatientID, base.PersonID);
        }

        public new bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewPatient())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdatePatient();

            }
            return false;
        }

        static public  bool DeletePatient(int PatientID)
        {
            int PersonID = clsPatientDataAccess.Person(PatientID);
             clsPatientDataAccess.DeletePatient(PatientID);
            return DeletePerson(PersonID);
        }

        static public DataTable GetAllPatients()
        {
            return clsPatientDataAccess.GetAllPatients();
        }

        static public bool IsPatientExist(int Patient)
        {
            return clsPatientDataAccess.IsPatientExist(Patient);
        }


    }
}
