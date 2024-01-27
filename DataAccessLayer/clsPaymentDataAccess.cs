using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    static public class clsPaymentDataAccess
    {
        static public int PaymentMethod(int PaymentID)
        {
            int PaymentMethodID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"SELECT PaymentMethodID FROM Payments
		                    WHERE PaymentID = @PaymentID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PaymentID", PaymentID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {

                    PaymentMethodID = (int)reader["PaymentMethodID"];
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

            return PaymentMethodID;
        }

        static public bool GetPaymentByID(ref int PaymentID,ref DateTime PaymentDate,ref int PaymentMethodID
            ,ref decimal AmountPaid,ref string AdditionalNotes)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"SELECT * FROM Payments WHERE PaymentID = @PaymentID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PaymentID", PaymentID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();


                if (reader.Read())
                {
                    // the Person Was Successfully Found

                    isFound = true;

                    PaymentID = (int)reader["PaymentID"];

                    PaymentDate = (DateTime)reader["PaymentDate"];

                    PaymentMethodID = (int)reader["PaymentMethodID"];

                    AmountPaid = (decimal)reader["AmountPaid"];

                    if (reader["AdditionalNotes"] != DBNull.Value)

                        AdditionalNotes = (string)reader["AdditionalNotes"];

                    else

                        AdditionalNotes = "";



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

        static public int AddNewPayment(DateTime PaymentDate, int PaymentMethodID, decimal AmountPaid, string AdditionalNotes)
        {

            int PaymentID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"INSERT INTO Payments (PaymentDate, PaymentMethodID, AmountPaid, AdditionalNotes)
                                VALUES
                                        (@PaymentDate, @PaymentMethodID, @AmountPaid, @AdditionalNotes);
                                                     SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PaymentDate",PaymentDate);

            command.Parameters.AddWithValue("@PaymentMethodID", PaymentMethodID);

            command.Parameters.AddWithValue("@AmountPaid", AmountPaid);

            if(AdditionalNotes !="")

            command.Parameters.AddWithValue("@AdditionalNotes", AdditionalNotes);

            else

            command.Parameters.AddWithValue("@AdditionalNotes",DBNull.Value);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != DBNull.Value && int.TryParse(result.ToString(), out int InsertedID))
                {
                    PaymentID = InsertedID;
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

            return PaymentID;

        }

        static public bool UpdatePayment(int PaymentID, DateTime PaymentDate, int PaymentMethodID, 
            decimal AmountPaid, string AdditionalNotes)
        {
            int RowsAffected = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"UPDATE Payments
                               SET PaymentDate = @PaymentDate,
                                    PaymentMethodID = @PaymentMethodID,
                                    AmountPaid = @AmountPaid,
                                    AdditionalNotes = @AdditionalNotes
                                        WHERE Payment = @PaymentID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PaymentID", PaymentID);

            command.Parameters.AddWithValue("@PaymentDate", PaymentDate);

            command.Parameters.AddWithValue("@PaymentMethodID", PaymentMethodID);

            command.Parameters.AddWithValue("@AmountPaid", AmountPaid);

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

        static public bool DeletePayment(int PaymentID)
        {
            int RowsAffected = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"DELETE Payments                               
                                WHERE Payment = @PaymentID;";

            SqlCommand command = new SqlCommand(query, connection);

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

        static public DataTable GetAllPayments()
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"SELECT * FROM Payments";

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

        static public bool isPaymentExists(int PaymentID)
        {
            // this fuction will returns true when RowsAffected > 0 and flase if not 
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"SELECT IsFound = 1 FROM Payments
                             WHERE PaymentID = @PaymentID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PaymentID", PaymentID);

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
