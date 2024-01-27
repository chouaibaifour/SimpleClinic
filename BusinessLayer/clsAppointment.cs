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

        public int PatientID { get; set; }

        public int DoctorID { get; set; }

        public enAppointmentStatus AppointmentStatus { get; set;}

        public DateTime AppointmentDateTime { get; set; } 
        
        public int MedicalRecord {  get; set; }

        public int PaymentID { get; set; }


        public clsAppointment()
        {
            AppointmentID = -1;

            PatientID = -1;

            DoctorID = -1;

            AppointmentStatus = enAppointmentStatus.NotSet;

            AppointmentDateTime = DateTime.Now;

            MedicalRecord = -1;

            PaymentID = -1;

            Mode = enMode.AddNew;

        }

        private clsAppointment( int AppointmentID, int PatientID, int DoctorID, enAppointmentStatus AppointmentStatus,
            DateTime AppointmentDateTime, int MedicalRecord, int PaymentID)
        {
           
            this.AppointmentID = AppointmentID;

            this.PatientID = PatientID;

            this.DoctorID = DoctorID;

            this.AppointmentStatus = AppointmentStatus;

            this.AppointmentDateTime = AppointmentDateTime;

            this.MedicalRecord = MedicalRecord;

            this.PaymentID = PaymentID;

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
                return new clsAppointment(AppointmentID, PatientID, DoctorID, (enAppointmentStatus)AppointmentStatus, AppointmentDateTime,
                    MedicalRecordID, PaymentID);
            }
            return null;
        }

        private bool _AddNewAppointment()
        {
            this.AppointmentID = clsAppointmentDataAccess.AddNewAppointment(this.PatientID, this.DoctorID,
                this.AppointmentDateTime, (byte)this.AppointmentStatus, this.MedicalRecord, this.PaymentID);
            return (this.AppointmentID != -1);
        }

        private bool _UpdateAppointment()
        {

            return clsAppointmentDataAccess.UpdateAppointment(this.AppointmentID, this.PatientID, this.DoctorID,
                this.AppointmentDateTime, (byte)this.AppointmentStatus, this.MedicalRecord, this.PaymentID);

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
