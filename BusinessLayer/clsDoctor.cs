using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class clsDoctor : clsPerson
    {
        private enum enMode { AddNew = 1, Update = 2 };
        enMode Mode = enMode.AddNew;
        public int DoctorID { get; set; }
        public string Specialization {  get; set; }

        clsDoctor() : base()
        {
            this.DoctorID = -1;
            this.Mode = enMode.AddNew;
            Specialization = "";

        }

        private clsDoctor(int PatientID, int PersonID, string FirstName, string LastName, DateTime DateOfBirth,
            enGender Gender, string PhoneNumber, string Password, string Email, string Address, string specialization)
            : base(PersonID, FirstName, LastName, DateOfBirth, Gender, PhoneNumber, Password, Email, Address)
        {

            this.DoctorID = PatientID;

            this.Mode = enMode.Update;
            this.Specialization = specialization;
        }

        static public new clsDoctor Find(int DoctorID)
        {

            int PersonID = -1;
            string Specialization = "";

            if (clsDoctorDataAccess.GetDoctorByID(ref DoctorID, ref PersonID,ref Specialization))
            {
                clsPerson person = clsPerson.Find(PersonID);
                if (person != null)
                {

                    clsDoctor doctor = (clsDoctor)person;
                    doctor.DoctorID = DoctorID;

                    return new clsDoctor(doctor.DoctorID, doctor.PersonID, doctor.FirstName, doctor.LastName,
                        doctor.DateOfBirth, doctor.Gender, doctor.PhoneNumber, doctor.Password, doctor.Email, doctor.Address,doctor.Specialization);
                }
                else
                {
                    return null;
                }
            }
            else
                return null;
        }

        private bool _AddNewDoctor()
        {
            base.Save();
            this.DoctorID = clsDoctorDataAccess.AddNewDoctor(base.PersonID,Specialization);
            return (this.DoctorID != 0);
        }

        private bool _UpdateDoctor()
        {
            base.Save();
            return clsDoctorDataAccess.UpdateDoctor(DoctorID, base.PersonID,Specialization);
        }

        public new bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewDoctor())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateDoctor();

            }
            return false;
        }

        static public bool DeleteDoctor(int DoctorID)
        {
            int PersonID = clsDoctorDataAccess.Person(DoctorID);
            clsDoctorDataAccess.DeleteDoctor(DoctorID);
            return DeletePerson(PersonID);
        }

        static public DataTable GetAllDoctors()
        {
            return clsDoctorDataAccess.GetAllDoctors();
        }

        static public bool IsDoctorExist(int Doctor)
        {
            return clsDoctorDataAccess.IsDoctorExist(Doctor);
        }
    }
}
