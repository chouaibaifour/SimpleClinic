using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataAccessLayer.clsPersonDataAccess;

namespace DataAccessLayer
{
    static public class clsDoctorDataAccess
    {
        static public int DoctorToPerson(string Condition)
        {
            int PersonID = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"SELECT PersonID FROM Doctors WHERE  @Condition";

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

        static public bool GetDoctorByID(ref int DoctorID, ref string FirstName, ref string LastName, ref DateTime DateOfBirth,
            ref clsPersonDataAccess.enGender Gender, ref string PhoneNumber, ref string Email, ref string Address , ref string Specialization)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"SELECT  Persons.FirstName, Persons.LastName, Persons.DateOfBirth, Persons.Gender , Persons.PhoneNumber,
                                         Persons.Email, Persons.Password, Persons.Address, Doctors.Specialization
                                              FROM  Doctors INNER JOIN
                                                     Persons ON Doctors.PersonID = Persons.PersonID
                                                         where DoctorID = @DoctorID ";

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

        static public bool GetPatientByEmail(ref int DoctorID, ref string FirstName, ref string LastName, ref DateTime DateOfBirth,
            ref clsPersonDataAccess.enGender Gender, ref string PhoneNumber, ref string Email, ref string Address, ref string Specialization)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"SELECT  Persons.FirstName, Persons.LastName, Persons.DateOfBirth, Persons.Gender , Persons.PhoneNumber,
                                         Persons.Email, Persons.Password, Persons.Address, Doctors.Specialization
                                              FROM  Doctors INNER JOIN Persons ON Doctors.PersonID = Persons.PersonID
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

        static public int AddNewDoctor(string FirstName, string LastName, DateTime DateOfBirth,
            clsPersonDataAccess.enGender Gender, string PhoneNumber, string Password, string Email, string Address, string Specialization)
        {
            // the function will returns PatientID or -1 if not 
            int PersonID = clsPersonDataAccess.AddNewPerson(FirstName, LastName, DateOfBirth, Gender,
                PhoneNumber, Password, Email, Address);

            if (PersonID < 1)
                return -1;

            int DoctorID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"INSERT INTO Patients(PersonID,Specializition)
                            VALUES
                            (@PersonID,Specialization);
                               SELECT SCOPE_IDENTITY(); ";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@Specialization", Specialization);

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

        static public bool UpdateDoctor(int DoctorID, string FirstName, string LastName, DateTime DateOfBirth,
           clsPersonDataAccess.enGender Gender, string PhoneNumber, string Password, string Email, string Address, string Specialization)
        {
            int PersonID = DoctorToPerson(@"DoctorID = " + DoctorID.ToString());


                int RowsAffected = -1;
                // this function returns true if Rows affected > 0 or false if no RowsAffected
            if (PersonID > 0)
            {
                if (clsPersonDataAccess.UpdatePerson(PersonID, FirstName, LastName, DateOfBirth,
                    Gender, PhoneNumber, Password, Email, Address)) { 

                

                SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

                string query = @"UPDATE Persons
                            SET Specialization = @Specialization,
                            WHERE PersonID = @PersonID;";

                SqlCommand command = new SqlCommand(query, connection);


                command.Parameters.AddWithValue("@PersonID", PersonID);

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
            }
            return false;
        }


    }
}
