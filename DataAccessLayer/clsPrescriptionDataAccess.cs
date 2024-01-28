using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    static public class clsPrescriptionDataAccess
    {
        static public int MedicalRecord(int PrescriptionID)
        {
            int MedicalRecordID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"SELECT MedicalRecordID FROM Prescriptions
		                    WHERE PrescriptionID = @PrescriptionID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PrescriptionID", PrescriptionID);

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

        static public bool GetPrescriptionByID(ref int PrescriptionID, ref int MedicalRecordID, ref string MedicationName
           , ref string Dosage, ref string Frequency,ref DateTime StartDate, ref DateTime EndDate,ref string SpecialInstructions)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"SELECT * FROM Prescriptions WHERE PrescriptionID = @PrescriptionID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PrescriptionID", PrescriptionID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();


                if (reader.Read())
                {
                    // the Person Was Successfully Found

                    isFound = true;

                    PrescriptionID = (int)reader["PrescriptionID"];

                    MedicalRecordID = (int)reader["MedicalRecordID"];

                    MedicationName = (string)reader["MedicationName"];

                    Dosage = (string)reader["Dosage"];

                    Frequency = (string)reader["Frequency"];

                    StartDate = (DateTime)reader["StartDate"];

                    EndDate = (DateTime)reader["EndDate"];
                    

                    if (reader["SpecialInstructions"] != DBNull.Value)

                        SpecialInstructions = (string)reader["SpecialInstructions"];

                    else

                        SpecialInstructions = "";

                }
                else
                    isFound = false;
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

        static public int AddNewPrescription( int MedicalRecordID,  string MedicationName,  string Dosage,
             string Frequency,  DateTime StartDate,  DateTime EndDate,  string SpecialInstructions)
        {

            int PrescriptionID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"INSERT INTO Prescriptions (MedicalRecordID, MedicationName, Dosage, Frequency, StartDate, EndDate, SpecialInstructions)
                                VALUES
                                        (@MedicalRecordID, @MedicationName, @Dosage, @Frequency, @StartDate, @EndDate, @SpecialInstructions);
                                                     SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@MedicalRecordID", MedicalRecordID);

            command.Parameters.AddWithValue("@MedicationName", MedicationName);

            command.Parameters.AddWithValue("@Dosage", Dosage);

            command.Parameters.AddWithValue("@Frequency", Frequency);

            command.Parameters.AddWithValue("@StartDate", StartDate);

            command.Parameters.AddWithValue("@EndDate", EndDate);

            if (SpecialInstructions != "")

                command.Parameters.AddWithValue("@SpecialInstructions", SpecialInstructions);

            else

                command.Parameters.AddWithValue("@SpecialInstructions", DBNull.Value);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != DBNull.Value && int.TryParse(result.ToString(), out int InsertedID))
                {
                    PrescriptionID = InsertedID;
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

            return PrescriptionID;

        }

        static public bool UpdatePrescription(int PrescriptionID, int MedicalRecordID, string MedicationName, string Dosage,
             string Frequency, DateTime StartDate, DateTime EndDate, string SpecialInstructions)
        {
            int RowsAffected = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"UPDATE Payments
                               SET MedicalRecordID = @MedicalRecordID,
                                    MedicationName = @MedicationName,
                                    Dosage = @Dosage,
                                    Frequency = @Frequency,
                                    StartDate = @StartDate,
                                    EndDate = @EndDate,
                                    SpecialInstructions = @SpecialInstructions
                                        WHERE PaymentID = @PaymentID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@MedicalRecordID", MedicalRecordID);

            command.Parameters.AddWithValue("@MedicationName", MedicationName);

            command.Parameters.AddWithValue("@Dosage", Dosage);

            command.Parameters.AddWithValue("@Frequency", Frequency);

            command.Parameters.AddWithValue("@StartDate", StartDate);

            command.Parameters.AddWithValue("@EndDate", EndDate);

            if (SpecialInstructions != "")

                command.Parameters.AddWithValue("@SpecialInstructions", SpecialInstructions);

            else

                command.Parameters.AddWithValue("@SpecialInstructions", DBNull.Value);

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

        static public bool DeletePrescription(int PrescriptionID)
        {
            int RowsAffected = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"DELETE Prescriptions                               
                                WHERE PrescriptionID = @PrescriptionID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PrescriptionID", PrescriptionID);

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

        static public DataTable GetAllPrescriptions()
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"SELECT * FROM Prescriptions";

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

        static public bool isPrescriptionExists(int PrescriptionID)
        {
            // this fuction will returns true when RowsAffected > 0 and flase if not 
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"SELECT IsFound = 1 FROM Prescriptions
                             WHERE PrescriptionID = @PrescriptionID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PrescriptionID", PrescriptionID);

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
