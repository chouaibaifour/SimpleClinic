using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    static public class clsPersonDataAccess
    {
        public enum enGender { Female=0,Male=1,NotSet=2 };

        static public bool GetPersonByID(ref int PersonID, ref string FirstName, ref string LastName, ref DateTime DateOfBirth,
            ref enGender Gender, ref string PhoneNumber,ref string Password, ref string Email, ref string Address)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"SELECT PersonID,FirstName,LastName,DateOfBirth,Password
                                    Gender,PhoneNumber,Email,Address FROM Persons WHERE PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query,connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                

                if (reader.Read())
                {
                    // the Person Was Successfully Found

                    isFound = true;

                    PersonID = (int)reader["PersonID"];

                    FirstName = (string)reader["FirstName"];

                    LastName = (string)reader["LastName"];

                    DateOfBirth = (DateTime)reader["DateOfBirth"];

                    Gender = (enGender)reader["Gender"];

                    PhoneNumber = (string)reader["PhoneNumber"];

                    Password = (string)reader["Password"];

                    Email = (string)reader["Email"];

                    if (reader["Address"] != DBNull.Value)
                    {
                        Address = (string)reader["Address"];
                    }
                    else
                    {
                        Address = "";
                    }

                }else
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

        static public bool GetPersonByEmail(ref int PersonID, ref string FirstName, ref string LastName, ref DateTime DateOfBirth,
            ref enGender Gender, ref string PhoneNumber, ref string Email, ref string Address)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"SELECT PersonID,FirstName,LastName,DateOfBirth,
                                    Gender,PhoneNumber,Email,Address FROM Persons WHERE Email = @Email";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Email", Email);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();


                if (reader.Read())
                {
                    // the Person Was Successfully Found

                    isFound = true;

                    PersonID = (int)reader["PersonID"];

                    FirstName = (string)reader["FirstName"];

                    LastName = (string)reader["LastName"];

                    DateOfBirth = (DateTime)reader["DateOfBirth"];

                    Gender = (enGender)reader["Gender"];

                    PhoneNumber = (string)reader["PhoneNumber"];

                    Email = (string)reader["Email"];

                    if (reader["Address"] != DBNull.Value)
                    {
                        Address = (string)reader["Address"];
                    }
                    else
                    {
                        Address = "";
                    }

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

        static public int AddNewPerson( string FirstName, string LastName, DateTime DateOfBirth,
            enGender Gender, string PhoneNumber,string Password, string Email, string Address)
        {
            // the function will returns PersonID or -1 if not 
            int PersonID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"INSERT INTO Persons(FirstName,LastName, DateOfBirth, Gender, PhoneNumber, Email, Password, Address)
                            VALUES
                            (@FirstName,@LastName, @DateOfBirth, @Gender, @PhoneNumber, @Email, @Password, @Address);
                               SELECT SCOPE_IDENTITY(); ";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@FirstName", FirstName);

            command.Parameters.AddWithValue("@LastName", LastName);

            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);

            command.Parameters.AddWithValue("@Gender",(int) Gender);

            command.Parameters.AddWithValue("@PhoneNumber", PhoneNumber);

            command.Parameters.AddWithValue("@Email", Email);

            command.Parameters.AddWithValue("@Password", Password);

            if (Address != "")
            {

            command.Parameters.AddWithValue("@Address", Address);

            }
            else
            {

                command.Parameters.AddWithValue("@Address",DBNull.Value);

            }

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if(result != DBNull.Value&& int.TryParse(result.ToString(),out int InsertedID))
                {
                    PersonID = InsertedID;
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

            return PersonID;
            
        }

        static public bool UpdatePerson(int PersonID, string FirstName, string LastName, DateTime DateOfBirth,
            enGender Gender, string PhoneNumber, string Password, string Email, string Address)
        {
            int RowsAffected = -1;
            // this function returns true if Rows affected > 0 or false if no RowsAffected

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"UPDATE Persons
                            SET FirstName = @FirstName,
                            LastName = @LastName,
                            DateOfBirth = @DateOfBirth,
                            Gender = @Gender,
                            PhoneNumber = @PhoneNumber,
                            Email = @Email,
                            Password = @Password,
                            Address = @Address
                            WHERE PersonID = @PersonID;";

            SqlCommand command = new SqlCommand(query, connection);


            command.Parameters.AddWithValue("@PersonID",PersonID);

            command.Parameters.AddWithValue("@FirstName",FirstName);

            command.Parameters.AddWithValue("@LastName",LastName);

            command.Parameters.AddWithValue("@DateOfBirth",DateOfBirth);

            command.Parameters.AddWithValue("@Gender",(int)Gender);

            command.Parameters.AddWithValue("@PhoneNumber",PhoneNumber);

            command.Parameters.AddWithValue("@Email",Email);

            command.Parameters.AddWithValue("@Password",Password);

            if (Address != "")
            {
                command.Parameters.AddWithValue("@Address", Address);
            }
            else
                command.Parameters.AddWithValue("@Address",DBNull.Value);

            try
            {
                connection.Open();
                RowsAffected = command.ExecuteNonQuery();
                
            }catch (Exception ex)
            {
                clsDataAccessSettings.PrintExecptionErrorMessage(ex);  
            }
            finally
            {
                connection.Close();
            }

            return (RowsAffected >0);
        }

        static public bool DeletePerson(int PersonID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            // this fuction will returns true when RowsAffected > 0 and flase if not 

            int RowsAffected = 0;

            string query = @"DELETE Persons
                             WHERE PersonID=@PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

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

        public static DataTable GetAllPersons()
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"SELECT PersonID,FirstName,LastName,DateOfBirth,
                                       CASE
                                            WHEN Gender = 0 THEN Female
                                            WHEN Gender = 1 THEN Male
                                            ELSE result
                                        END,
                                   PhoneNumber,Email,Address FROM Persons ";

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

        public static bool IsPersonExist(int PersonID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            // this fuction will returns true when RowsAffected > 0 and flase if not 

            bool isFound = false;

            string query = @"SELECT IsFound=1 FROM Persons
                             WHERE PersonID=@PersonID";
            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

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

        public static bool IsPersonExist(string Email)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            // this fuction will returns true when RowsAffected > 0 and flase if not 

            bool isFound = false;

            string query = @"SELECT IsFound=1 FROM Persons
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
