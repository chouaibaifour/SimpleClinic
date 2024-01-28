using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    static public class clsMedicalRecordDataAccess
    {
        static public bool GetMedicalRecordByID(ref int MedicalRecordID,ref string VisitDescription,ref string Diagnosis,
            ref string AdditionalNotes)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"SELECT * FROM MedicalRecords WHERE MedicalRecordID = @MedicalRecordID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@MedicalRecordID", MedicalRecordID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();


                if (reader.Read())
                {
                    // the Person Was Successfully Found

                    isFound = true;

                    MedicalRecordID = (int)reader["MedicalRecordID"];

                    VisitDescription = (string)reader["VisitDescription"];

                    Diagnosis = (string)reader["Diagnosis"];

                    if (reader["AdditionalNotes"]!=DBNull.Value)
                    
                       AdditionalNotes = (string)reader["AdditionalNotes"];

                    else
                        AdditionalNotes="";
                   
                    
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

        static public int AddNewMedicalRecord(string VisitDescription,string Diagnosis,string AdditionalNotes)
        {
            int MedicalRecordID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"INSERT INTO MedicalRecords (VisitDescription, Diagnosis, AdditionalNotes)
                                  VALUES
                                        (@VisitDescription, @Diagnosis, @AdditionalNotes);
                                                     SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@VisitDescription", VisitDescription);

            command.Parameters.AddWithValue("@Diagnosis", Diagnosis);

            if (AdditionalNotes != "")

                command.Parameters.AddWithValue("@AdditionalNotes", AdditionalNotes);

            else

                command.Parameters.AddWithValue("@AdditionalNotes", DBNull.Value);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != DBNull.Value && int.TryParse(result.ToString(), out int InsertedID))
                {
                    MedicalRecordID = InsertedID;
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

            return MedicalRecordID;

        }

        static public bool UpdateMedicalRecord(int MedicalRecordID,string VisitDescription,string Diagnosis,string AdditionalNotes)
        {
            int RowsAffected = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"UPDATE MedicalRecords
                               SET  VisitDescription = @VisitDescription,
                                    Diagnosis = @Diagnosis,                                    
                                    AdditionalNotes = @AdditionalNotes
                                        WHERE MedicalRecordID = @MedicalRecordID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@MedicalRecordID", MedicalRecordID);
             
            command.Parameters.AddWithValue("@VisitDescription", VisitDescription);

            command.Parameters.AddWithValue("@Diagnosis", Diagnosis);

            if (AdditionalNotes != "")

                command.Parameters.AddWithValue("@AdditionalNotes", AdditionalNotes);

            else

                command.Parameters.AddWithValue("@AdditionalNotes", DBNull.Value);

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

        static public bool DeleteMedicalRecord(int MedicalRecordID)
        {
            int RowsAffected = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"DELETE MedicalRecords                               
                                        WHERE MedicalRecordID = @MedicalRecordID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@MedicalRecordID", MedicalRecordID);          
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

        static public DataTable GetAllMedicalRecords()
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"SELECT * FROM MedicalRecords";

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

        static public bool isMedicalRecordExists(int MedicalRecordID)
        {
            // this fuction will returns true when RowsAffected > 0 and flase if not 
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"SELECT IsFound = 1 FROM MedicalRecords
                             WHERE MedicalRecordID = @MedicalRecordID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@MedicalRecordID", MedicalRecordID);

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
