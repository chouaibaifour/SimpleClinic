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
            enGender Gender, string PhoneNumber, string Password, string Email, string Address) : base(PersonID,FirstName,LastName,DateOfBirth,Gender,PhoneNumber,Password,Email,Address)
        {
            
            this.PatientID = PatientID;

            this.Mode = enMode.Update;
        }

        static public new clsPatient Find(int PatientID)
        {
            string FirstName = "", LastName = "", PhoneNumber = "", Email = "", Password = "", Address = "";

            clsPersonDataAccess.enGender Gender = clsPersonDataAccess.enGender.NotSet;

            DateTime DateOfBirth = DateTime.Now;

            int PersonID = -1;

            if (clsPatientDataAccess.GetPatientByID(ref PatientID, ref PersonID))
            {
                if (clsPersonDataAccess.GetPersonByID(ref PersonID, ref FirstName, ref LastName, ref DateOfBirth, ref Gender,
                ref PhoneNumber, ref Password, ref Email, ref Address))
                {
                    return new clsPatient(PatientID, PersonID, FirstName, LastName, DateOfBirth,
                        (enGender)Gender, PhoneNumber, Password, Email, Address);
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
            this.PatientID = clsPatientDataAccess.AddNewPatient(FirstName, LastName, DateOfBirth,
                (clsPersonDataAccess.enGender)Gender, PhoneNumber, Password, Email, Address);
                return (this.PatientID!=0); 
        }

        private bool _UpdatePatient()
        {
            return clsPatientDataAccess.UpdatePatient(PatientID, PersonID, FirstName, LastName, DateOfBirth, 
                (clsPersonDataAccess.enGender)Gender, PhoneNumber, Password, Email, Address);
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

        public  bool DeletePatient()
        {
            return clsPatientDataAccess.DeletePatient(PatientID, PersonID);
        }

        public DataTable GetAllPatients()
        {
            return clsPatientDataAccess.GetAllPatients();
        }

        static public bool IsPatientExist(int Patient)
        {
            return clsPatientDataAccess.IsPatientExist(Patient);
        }

    }
}
