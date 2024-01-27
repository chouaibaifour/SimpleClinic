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

        static public int Person(int PatientID)
        {
            int PersonID = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"SELECT PersonID FROM Patients WHERE  PatientID = @PatientID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PatientID", PatientID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();


                if (reader.Read())
                {

                    PersonID = (int)reader["PersonID"];

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
            return PersonID;
        }

        static public bool GetPatientByID(ref int PatientID, ref int PersonID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"SELECT PatientID, PersonID FROM Patients WHERE PatientID = @PatientID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PatientID", PatientID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();


                if (reader.Read())
                {
                    // the Person Was Successfully Found

                    isFound = true;

                    PersonID = (int)reader["PersonID"];

                    PatientID = (int)reader["PatientID"];

                }
                else
                    isFound = false;
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

            return isFound;

        }

        static public int AddNewPatient(int PersonID)
        {
            // the function will returns PatientID or -1 if not 


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

        static public bool UpdatePatient(int PatientID,int PersonID)
        {
            int RowsAffected = -1;
            // this function returns true if Rows affected > 0 or false if no RowsAffected

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"UPDATE Patients
                            SET PersonID = @PersonID                            
                            WHERE PatientID = @PatientID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            command.Parameters.AddWithValue("@PatientID", PatientID);

            try
            {
                connection.Open();
                RowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                clsDataAccessSettings.PrintExecptionErrorMessage(ex);
            }
            finally
            {
                connection.Close();
            }

            return (RowsAffected > 0);
                     

        }

        static public bool DeletePatient(int PatientID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            // this fuction will returns true when RowsAffected > 0 and flase if not 

            int RowsAffected = 0;

            string query = @"DELETE Patients
                                WHERE PatientID=@PatientID;";

            SqlCommand command = new SqlCommand(query, connection);


            command.Parameters.AddWithValue("@PatientID", PatientID);

            try
            {
                connection.Open();

                RowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                clsDataAccessSettings.PrintExecptionErrorMessage(ex);
            }
            finally
            {
                connection.Close();
            }

            return (RowsAffected > 0);

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
            return isFound;
        }


    }
}
