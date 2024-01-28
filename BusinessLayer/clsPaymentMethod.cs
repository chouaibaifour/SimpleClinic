using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class clsPaymentMethod
    {
        private enum enMode { AddNew = 1, Update = 2 };
        enMode Mode = enMode.AddNew;

        public int PaymentMethodID { get; set; }
        public string PaymentMethodName { get; set; }
        public bool isAvailable { get; set; }
        
        public clsPaymentMethod()
        {
            PaymentMethodID = -1;

            PaymentMethodName = "";

            isAvailable = false;

            Mode = enMode.AddNew;

        }

        private clsPaymentMethod(int PaymentMethodID, string PaymentMethodName,bool isAvailable)
        {
            this.PaymentMethodID = PaymentMethodID;

            this.PaymentMethodName = PaymentMethodName;

            this.isAvailable = isAvailable;

            Mode = enMode.Update;

        }

        static public clsPaymentMethod Find(int PaymentMethodID)
        {
            string PaymentMethodName = "";

            bool isAvailable = false;

            if (clsPaymentMethodDataAccess.GetPaymentMethodByID(ref PaymentMethodID, ref PaymentMethodName, ref isAvailable))
            
                return new clsPaymentMethod(PaymentMethodID, PaymentMethodName, isAvailable);
            
            else

                return null;

        }

        private bool _AddNewPaymentMethod()
        {
            this.PaymentMethodID=clsPaymentMethodDataAccess.AddNewPaymentMethod(PaymentMethodName,isAvailable);

            return (this.PaymentMethodID!=-1);
        }

        private bool _UpdatePaymentMethod()
        {
            return clsPaymentMethodDataAccess.UpdatePaymentMethod( PaymentMethodID,PaymentMethodName,isAvailable);

        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewPaymentMethod())
                    {
                        Mode = enMode.Update;

                        return true;
                    }
                    else
                        return false;

                case enMode.Update:

                    return _UpdatePaymentMethod();

            }
            return false;
        }

        static public bool DeletePaymentMethod(int PaymentMethodID)
        {

            return clsPaymentMethodDataAccess.DeletePaymentMethod(PaymentMethodID);

        }

        static public DataTable GetAllPaymentMethods()
        {

            return clsPaymentMethodDataAccess.GetAllPaymentMethods();

        }

        static public bool isPaymentMethodExists(int PaymentMethodID)
        {

            return clsPaymentMethodDataAccess.isPaymentMethodExists(PaymentMethodID);

        }

    }
}
