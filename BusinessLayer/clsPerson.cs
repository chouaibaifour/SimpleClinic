using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;

namespace BusinessLayer
{
    public class clsPerson
    {
        private enum enMode { AddNew,Update};
        private enMode Mode = enMode.AddNew;
        public enum enGender { Female = 0, Male = 1, NotSet = 2 };
        public int PersonID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth {  get; set; }
        public enGender Gender { get; set; }
        public string PhoneNumber {  get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }

        public clsPerson()
        {
            Mode= enMode.AddNew;

            PersonID = -1;

            FirstName = "";

            LastName = "";

            DateOfBirth = DateTime.Now;

            Gender = enGender.NotSet;

            PhoneNumber = "";

            Email = "";

            Address = "";

        }

        protected clsPerson(int PersonID,string FistName,string LastName,DateTime DateOfBirth, enGender Gender,string PhoneNumber
            ,string Password,string Email,string Address)
        {

            this.PersonID = PersonID;

            this.FirstName = FirstName;

            this.LastName = LastName;

            this.DateOfBirth = DateOfBirth;

            this.Gender = Gender;

            this.PhoneNumber = PhoneNumber;

            this.Password = Password;

            this.Email = Email;

            this.Address = Address;

            Mode = enMode.Update;
        }
          
        private bool _AddNewPerson()
        {
            this.PersonID = clsPersonDataAccess.AddNewPerson(this.FirstName,this.LastName,this.DateOfBirth,(byte)this.Gender
                ,this.PhoneNumber,this.Password, this.Email, this.Address);

            return (this.PersonID!=-1);
        }

        private bool _UpdatePerson() 
        {
            return clsPersonDataAccess.UpdatePerson(this.PersonID, this.FirstName, this.LastName, this.DateOfBirth,(byte) this.Gender
                , this.PhoneNumber, this.Password, this.Email, this.Address);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewPerson())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdatePerson();

            }
            return false;
        }

        static public clsPerson Find(int PersonID)
        {
            string FirstName = "", LastName = "", PhoneNumber = "", Email = "", Password = "", Address = "";
            byte  Gender = (byte)enGender.NotSet;
            DateTime DateOfBirth = DateTime.Now;
            

            if(clsPersonDataAccess.GetPersonByID(ref PersonID,ref FirstName,ref LastName,ref DateOfBirth,ref Gender,
                ref PhoneNumber,ref  Password, ref Email,ref Address))
            {
                return new clsPerson(PersonID, FirstName, LastName, DateOfBirth, 
                    (enGender)Gender, PhoneNumber, Password, Email, Address);
            }
            else
            { 
                return null; 
            }
        }

        static public bool DeletePerson(int PersonID)
        {
            return clsPersonDataAccess.DeletePerson(PersonID);
        }

        static public DataTable GetAllPersons()
        {
            return clsPersonDataAccess.GetAllPersons();
        }
        
        static public bool IsPersonExists(int PersonID)
        {
            return clsPersonDataAccess.IsPersonExists(PersonID);
        }
    }
}
