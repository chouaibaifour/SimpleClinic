using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace DataAccessLayer
{
    public class clsPatientDataAccess
    {

        static public int PatientToPerson(string Condition)
        {
            int PersonID = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"SELECT PersonID FROM Patients WHERE  @Condition";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Condition", Condition);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();


                if (reader.Read())
                {

                     PersonID = (int)reader["PersonID"];

                }
                else

                   

                reader.Close();

            }
            catch (Exception ex)
            {
                clsDataAccessSettings.PrintExecptionErrorMessage(ex);
            }
            finally
            {
                connection.Close();
            }
            return PersonID;
        }

        static public bool GetPatientByID(ref int PatientID, ref string FirstName, ref string LastName, ref DateTime DateOfBirth,
            ref  clsPersonDataAccess.enGender Gender, ref string PhoneNumber, ref string Email, ref string Address)
        {
            int PersonID = PatientToPerson(@"PatientID = "+PatientID.ToString());

                if (PersonID>0)
                {
                    if (clsPersonDataAccess.GetPersonByID(ref PersonID, ref FirstName, ref LastName, ref DateOfBirth,
                        ref Gender, ref PhoneNumber, ref Email, ref Address)) 

                       return true; // the Patient Was Successfully Found

                    else 

                        return false;

                }
            return false;

        }

        static public bool GetPatientByEmail(ref int PatientID, ref string FirstName, ref string LastName, ref DateTime DateOfBirth,
            ref clsPersonDataAccess.enGender Gender, ref string PhoneNumber, ref string Email, ref string Address)
        {
            int PersonID = PatientToPerson(@"PatientID = " + Email.ToString());

            if (PersonID > 0)
            {
                if (clsPersonDataAccess.GetPersonByID(ref PersonID, ref FirstName, ref LastName, ref DateOfBirth,
                    ref Gender, ref PhoneNumber, ref Email, ref Address))

                    return true; // the Patient Was Successfully Found

                else

                    return false;

            }
            return false;


        }

        static public int AddNewPatient(string FirstName, string LastName, DateTime DateOfBirth,
            clsPersonDataAccess.enGender Gender, string PhoneNumber, string Password, string Email, string Address)
        {
            // the function will returns PatientID or -1 if not 
            int PersonID = clsPersonDataAccess.AddNewPerson(FirstName, LastName, DateOfBirth, Gender,
                PhoneNumber, Password, Email, Address);

            if (PersonID < 1)
                return -1;

            int PatientID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"INSERT INTO Patients(PersonID)
                            VALUES
                            (@PersonID);
                               SELECT SCOPE_IDENTITY(); ";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != DBNull.Value && int.TryParse(result.ToString(), out int InsertedID))
                {
                    PatientID = InsertedID;
                }

            }
            catch (Exception ex)
            {
                clsDataAccessSettings.PrintExecptionErrorMessage(ex);
            }
            finally
            {
                connection.Close();
            }

            return PatientID;

        }

        static public bool UpdatePatient(int PatientID, string FirstName, string LastName, DateTime DateOfBirth,
            clsPersonDataAccess.enGender Gender, string PhoneNumber, string Password, string Email, string Address)
        {
            int PersonID = PatientToPerson(@"PatientID = " + PatientID.ToString());

            if (PersonID > 0)
            {
                if (clsPersonDataAccess.UpdatePerson( PersonID,  FirstName,  LastName,  DateOfBirth,
                     Gender,  PhoneNumber,Password,  Email,  Address))

                    return true; // the Patient Was Successfully Found

                else

                    return false;

            }
            return false;
        }

        static public bool DeletePatient(int PatientID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            // this fuction will returns true when RowsAffected > 0 and flase if not 

            bool IsDeleted = false;
            int PersonID = PatientToPerson(@"PatientID = " + PatientID.ToString());

            string query = @"DELETE Patients
                                WHERE PatientID=@PatientID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PatientID", PatientID);

            try
            {
                connection.Open();

                if(command.ExecuteNonQuery()>0)
                {
                IsDeleted = clsPersonDataAccess.DeletePerson(PersonID);
                }
            }
            catch (Exception ex)
            {
                clsDataAccessSettings.PrintExecptionErrorMessage(ex);
            }
            finally
            {
                connection.Close();
            }

            return IsDeleted;

        }

        public static DataTable GetAllPatients()
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"SELECT   Patients.PatientID, Persons.FirstName, Persons.LastName, Persons.DateOfBirth,
                                        CASE
                                            WHEN Gender = 0 THEN Female
                                            WHEN Gender = 1 THEN Male
                                            ELSE result
                                        END, Persons.PhoneNumber, Persons.Email, Persons.Password, Persons.Address
                                            FROM  Patients INNER JOIN Persons 
                                                ON Patients.PersonID = Persons.PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dt.Load(reader);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                clsDataAccessSettings.PrintExecptionErrorMessage(ex);
            }
            finally
            {
                connection.Close();
            }
            return dt;

        }

        public static bool IsPatientExist(int PatientID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            // this fuction will returns true when RowsAffected > 0 and flase if not 

            bool isFound = false;

            string query = @"SELECT IsFound=1 FROM Patients
                             WHERE PatientID=@PatientID";
            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PatientID", PatientID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                isFound = reader.HasRows;
            }
            catch (Exception ex)
            {
                clsDataAccessSettings.PrintExecptionErrorMessage(ex);
            }
            finally
            {
                connection.Close();
            }
            return isFound;
        }

        public static bool IsPatientExist(string Email)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            // this fuction will returns true when RowsAffected > 0 and flase if not 

            bool isFound = false;

            string query = @"SELECT IsFound=1 FROM Patients
                             WHERE Email=@Email";
            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Email", Email);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                isFound = reader.HasRows;
            }
            catch (Exception ex)
            {
                clsDataAccessSettings.PrintExecptionErrorMessage(ex);
            }
            finally
            {
                connection.Close();
            }
            return isFound;
        }


    }
}
