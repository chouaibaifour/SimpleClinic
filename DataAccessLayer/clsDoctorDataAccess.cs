using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataAccessLayer.clsPersonDataAccess;

namespace DataAccessLayer
{
    static public class clsDoctorDataAccess
    {
        static public int Person(int DoctorID)
        {
            int PersonID = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"SELECT PersonID FROM Doctors WHERE  DoctorID = @DoctorID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DoctorID", DoctorID);

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

        static public bool GetDoctorByID(ref int DoctorID,ref int PersonID,  ref string Specialization)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"SELECT  PatientID, PersonID, Specialization  FROM Docotrs                                         
                                        WHERE DoctorID = @DoctorID ";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DoctorID", DoctorID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();


                if (reader.Read())
                {
                    // the Person Was Successfully Found

                    isFound = true;

                    DoctorID = (int)reader["DoctorID"];

                    PersonID = (int)reader["PersonID"];

                    Specialization = (string)reader["Specialization"];
                    

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

        static public int AddNewDoctor(int PersonID, string Specialization)
        {
            // the function will returns PatientID or -1 if not 

            int DoctorID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"
                                INSERT INTO Doctors(PersonID,Specializition)
                                    VALUES
                                  (@PersonID,@Specialization);
                                         SELECT SCOPE_IDENTITY(); ";

            SqlCommand command = new SqlCommand(query, connection);


            command.Parameters.AddWithValue("@Specialization", Specialization);
            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != DBNull.Value && int.TryParse(result.ToString(), out int InsertedID))
                {
                    DoctorID = InsertedID;
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

            return DoctorID;

        }

        static public bool UpdateDoctor(int DoctorID, int PersonID, string Specialization)
        {

            int RowsAffected = -1;
            // this function returns true if Rows affected > 0 or false if no RowsAffected

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"UPDATE Doctors
                            SET Specialization = @Specialization,
                            PersonID = @PersonID
                            WHERE DoctorID = @DoctorID;";

            SqlCommand command = new SqlCommand(query, connection);


            command.Parameters.AddWithValue("@PersonID", PersonID);

            command.Parameters.AddWithValue("@DoctorID", DoctorID);

            command.Parameters.AddWithValue("@Specialization", Specialization);

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

        static public bool DeleteDoctor(int DoctorID)
        {

            int RowsAffected = -1;
            // this function returns true if Rows affected > 0 or false if no RowsAffected

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"DELETE Doctors
                            WHERE DoctorID = @DoctorID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@DoctorID", DoctorID);

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

        public static DataTable GetAllDoctors()
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"SELECT   Doctors.DoctorID, Persons.FirstName, Persons.LastName, Persons.DateOfBirth,
                                        CASE
                                            WHEN Gender = 0 THEN Female
                                            WHEN Gender = 1 THEN Male
                                            ELSE result
                                        END, Persons.PhoneNumber, Persons.Email, Persons.Password, Persons.Address,Doctors.Specialization
                                            FROM  Doctors INNER JOIN Persons 
                                                ON Doctors.PersonID = Persons.PersonID";

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

        public static bool IsDoctorExist(int DoctorID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            // this fuction will returns true when RowsAffected > 0 and flase if not 

            bool isFound = false;

            string query = @"SELECT IsFound=1 FROM Doctors
                             WHERE DoctorID=@DoctorID";
            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@DoctorID", DoctorID);

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
