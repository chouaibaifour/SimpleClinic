using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class clsPayment
    {
        private enum enMode { AddNew = 1, Update = 2 };
        enMode Mode = enMode.AddNew;
        public int PaymentID { get; set; }
        public DateTime PaymentDate { get; set; }
        public int PaymentMethodID { get; set; }
        public decimal AmoundPaid { get; set; }
        public string AdditionalNotes { get; set; }

        public clsPayment()
        {
            PaymentID = -1;

            PaymentDate = DateTime.Now;

            PaymentMethodID = -1;

            AmoundPaid = -1;

            AdditionalNotes = "";

            Mode = enMode.AddNew;
        }

        private clsPayment( int PaymentID,DateTime PaymentDate, int PaymentMethodID, decimal AmoundPaid, string AdditionalNotes)
        {
            this.Mode = enMode.Update;

            this.PaymentID = PaymentID;

            this.PaymentDate = PaymentDate;

            this.PaymentMethodID = PaymentMethodID;

            this.AmoundPaid = AmoundPaid;

            this.AdditionalNotes = AdditionalNotes;

        }

        static public clsPayment Find(int PaymentID)
        {
            int PaymentMethodID =-1;

            decimal AmoundPaid = -1;

            string AdditionalNotes = "";

            DateTime PaymentDate =DateTime.Now;

            if (clsPaymentDataAccess.GetPaymentByID(ref PaymentID, ref PaymentDate, ref PaymentMethodID, ref AmoundPaid, ref AdditionalNotes))

                return new clsPayment(PaymentID, PaymentDate, PaymentMethodID, AmoundPaid, AdditionalNotes);

            else

                return null;

        }

        private bool _AddNewPayment()
        {
            this.PaymentID = clsPaymentDataAccess.AddNewPayment(PaymentDate, PaymentMethodID,AmoundPaid,AdditionalNotes);

            return (this.PaymentID != -1);

        }

        private bool _UpdatePayment()
        {

            return clsPaymentDataAccess.UpdatePayment(PaymentID,PaymentDate,PaymentMethodID,AmoundPaid,AdditionalNotes);

        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewPayment())
                    {
                        Mode = enMode.Update;

                        return true;
                    }
                    else
                        return false;
                    
                case enMode.Update:
                    
                    return _UpdatePayment();

            }
            return false;
        }

        static public bool DeletePayment(int PaymentID)
        {

            return clsPaymentDataAccess.DeletePayment(PaymentID);

        }

        static public DataTable GetAllPayments()
        {

            return clsPaymentDataAccess.GetAllPayments();

        }

        static public bool isPaymentExists(int PaymentID)
        {

            return clsPaymentDataAccess.isPaymentExists(PaymentID);

        }


    }
}
