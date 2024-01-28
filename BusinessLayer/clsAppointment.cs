using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class clsAppointment
    {
        public enum enAppointmentStatus
        {

            Scheduled = 1,

            Confirmed = 2,

            CheckedIn = 3,

            InProgress = 4,

            Completed = 5,

            Canceled = 6,

            NoShow = 7,

            NotSet = 8

        }

        private enum enMode { AddNew = 1, Update = 2 };

        enMode Mode = enMode.AddNew;

        public int AppointmentID { get; set; }

        public clsPatient Patient { get; set; }

        public clsDoctor Doctor { get; set; }

        public enAppointmentStatus AppointmentStatus { get; set;}

        public DateTime AppointmentDateTime { get; set; } 
        
        public int MedicalRecord {  get; set; }

        public clsPayment Payment { get; set; }

        public clsAppointment()
        {
            AppointmentID = -1;

            Patient = null;

            Doctor = null;

            AppointmentStatus = enAppointmentStatus.NotSet;

            AppointmentDateTime = DateTime.Now;

            MedicalRecord = -1;

            Payment = null;

            Mode = enMode.AddNew;

        }

        private clsAppointment( int AppointmentID, clsPatient Patient, clsDoctor Doctor, enAppointmentStatus AppointmentStatus,
            DateTime AppointmentDateTime, int MedicalRecord, clsPayment Payment)
        {
           
            this.AppointmentID = AppointmentID;

            this.Patient = Patient;

            this.Doctor = Doctor;

            this.AppointmentStatus = AppointmentStatus;

            this.AppointmentDateTime = AppointmentDateTime;

            this.MedicalRecord = MedicalRecord;

            this.Payment = Payment;

            this.Mode = enMode.Update;
        }

        static public  clsAppointment Find(int AppointmentID)
        {
            int PatientID = -1, DoctorID = -1, MedicalRecordID = -1, PaymentID = -1;
            DateTime AppointmentDateTime = DateTime.Now;
            byte AppointmentStatus = 8;

            if(clsAppointmentDataAccess.GetAppointmentByID(ref AppointmentID,ref PatientID,ref DoctorID,ref AppointmentDateTime,
                ref AppointmentStatus,ref MedicalRecordID,ref PaymentID))
            {
                return new clsAppointment(AppointmentID, clsPatient.Find(PatientID), clsDoctor.Find(DoctorID), (enAppointmentStatus)AppointmentStatus,
                    AppointmentDateTime, MedicalRecordID, clsPayment.Find(PaymentID));
            }
            return null;
        }

        private bool _AddNewAppointment()
        {
            this.AppointmentID = clsAppointmentDataAccess.AddNewAppointment(Patient.PatientID, Doctor.DoctorID,
                AppointmentDateTime, (byte)AppointmentStatus, MedicalRecord, Payment.PaymentID);
            return (this.AppointmentID != -1);
        }

        private bool _UpdateAppointment()
        {

            return clsAppointmentDataAccess.UpdateAppointment(AppointmentID, Patient.PatientID, Doctor.DoctorID,
                AppointmentDateTime, (byte)AppointmentStatus, MedicalRecord, Payment.PaymentID);

        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewAppointment())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateAppointment();

            }
            
            return false;
        }

        static public bool DeleteAppointment(int AppointmentID)
        {
            
            return clsAppointmentDataAccess.DeleteAppointment(AppointmentID);
        }

        static public DataTable GetAllAppointments()
        {
            return clsAppointmentDataAccess.GetAllAppointments();
        }

        static public bool IsAppointmentExist(int AppointmentID)
        {
            return clsAppointmentDataAccess.isAppointmentExists(AppointmentID);
        }
    }
}
