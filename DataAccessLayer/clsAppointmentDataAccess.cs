using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataAccessLayer.clsAppointmentDataAccess;

namespace DataAccessLayer
{
    static public class clsAppointmentDataAccess
    {

        static public int Patient(int AppointmentID)
        {
            int PatientID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"SELECT AppointmentID FROM Appointments
		                    WHERE AppointmentID = @AppointmentID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@AppointmentID", AppointmentID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    
                    PatientID = (int)reader["AppointmentID"];
                }

                reader.Close();

            }catch (Exception ex)
            {
                clsDataAccessSettings.PrintExecptionErrorMessage(ex);
            }finally
            {
                connection.Close();
            }

            return PatientID;
        }

        static public int Doctor(int AppointmentID)
        {
            int DoctorID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"SELECT DoctorID FROM Appointments
		                    WHERE AppointmentID = @AppointmentID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@AppointmentID", AppointmentID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    DoctorID = (int)reader["DoctorID"];
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

            return DoctorID;
        }

        static public int Payment(int AppointmentID)
        {
            int PaymentID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"SELECT PaymentID FROM Appointments
		                    WHERE AppointmentID = @AppointmentID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@AppointmentID", AppointmentID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    if (reader["PaymentID"] != DBNull.Value)
                        PaymentID = (int)reader["PaymentID"];
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

            return PaymentID;
        }

        static public int MedicalRecord(int AppointmentID)
        {
            int MedicalRecordID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"SELECT MedicalRecordID FROM Appointments
		                    WHERE AppointmentID = @AppointmentID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@AppointmentID", AppointmentID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    if (reader["MedicalRecordID"] != DBNull.Value)
                        MedicalRecordID = (int)reader["MedicalRecordID"];
                    
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

            return MedicalRecordID;
        }

        static public bool GetAppointmentByID(ref int AppointmentID,ref int PatientID,ref int DoctorID,ref DateTime AppointmentDateTime
            ,ref byte AppointmentStatus,ref int MedicalRecordID, ref int PaymentID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"SELECT * FROM Appointments 
                                   WHERE AppointmentID = @AppointmentID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@AppointmentID", AppointmentID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;
                    
                    AppointmentID = (int)reader["AppointmentID"];

                    PatientID = (int)reader["AppointmentID"];
                    
                    PaymentID = (int)reader["PaymentID"];

                    DoctorID = (int)reader["DoctorID"];

                    AppointmentDateTime = (DateTime)reader["AppointmentDateTime"];

                    AppointmentStatus = (byte)reader["AppointmentStatus"];

                    MedicalRecordID = (int)reader["MedicalRecordID"];

                }

                reader.Close();

            }
            catch (Exception ex)
            {
                isFound = false;
                clsDataAccessSettings.PrintExecptionErrorMessage(ex);
            }
            finally
            {
                connection.Close();
            }
            return isFound;

        }

        static public int AddNewAppointment(int PatientID, int DoctorID, DateTime AppointmentDateTime, byte AppointmentStatus,
            int MedicalRecordID, int PaymentID)
        {

            int AppointmentID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"INSERT INTO Appointments (PatientID, DoctorID, AppointmentDateTime, AppointmentStatus, MedicalRecordID, PaymentID)
                                    VALUES
                                         (@PatientID, @DoctorID, @AppointmentDateTime, @AppointmentStatus, @MedicalRecordID, @PaymentID);
                                                 SELECT SCOPE_IDENTITY(); ";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PatientID", PatientID);

            command.Parameters.AddWithValue("@DoctorID", DoctorID);

            command.Parameters.AddWithValue("@AppointmentDateTime", AppointmentDateTime);

            command.Parameters.AddWithValue("@AppointmentStatus", AppointmentStatus);

            if (MedicalRecordID > 0)
            {
                command.Parameters.AddWithValue("@MedicalRecordID", MedicalRecordID);
            }
            else
            {
                command.Parameters.AddWithValue("@MedicalRecordID", DBNull.Value);
            }

            if(PaymentID > 0)
            {
                command.Parameters.AddWithValue("@PaymentID", PaymentID);
            }
            else
                command.Parameters.AddWithValue("@PaymentID", DBNull.Value);


            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != DBNull.Value && int.TryParse(result.ToString(), out int InsertedID))
                {
                    AppointmentID = InsertedID;
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

            return AppointmentID;

        }

        static public bool UpdateAppointment(int AppointmentID,int PatientID,int DoctorID,DateTime AppointmentDateTime,
            byte AppointmentStatus,int MedicalRecordID,int PaymentID)
        {
            int RowsAffected = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"UPDATE Appointments
                            SET DoctorID = @DoctorID,
                                PatientID = @PatientID,
                                AppointmentDateTime = @AppointmentDateTime,     
                                AppointmentStatus = @AppointmentStatus,     
                                MedicalRecordID = @MedicalRecordID,     
                                PaymentID = @PaymentID,     
                            WHERE AppointmentID = @AppointmentID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@AppointmentID", AppointmentID);

            command.Parameters.AddWithValue("@PatientID", PatientID);

            command.Parameters.AddWithValue("@DoctorID", DoctorID);

            command.Parameters.AddWithValue("@AppointmentDateTime", AppointmentDateTime);

            command.Parameters.AddWithValue("@AppointmentStatus", AppointmentStatus);

            command.Parameters.AddWithValue("@MedicalRecordID", MedicalRecordID);

            command.Parameters.AddWithValue("@PaymentID", PaymentID);            

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

        static public bool DeleteAppointment(int AppointmentID)
        {
            int RowsAffected = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"DELETE Appointments
                            WHERE AppointmentID = @AppointmentID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@AppointmentID", AppointmentID);


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

        static public DataTable GetAllAppointments()
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"SELECT * FROM Appointments";

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

        static public bool isAppointmentExists(int AppointmentID)
        {   
            // this fuction will returns true when RowsAffected > 0 and flase if not 
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"SELECT IsFound = 1 FROM Appointments
                             WHERE AppointmentID = @AppointmentID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@AppointmentID", AppointmentID);

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
