using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    static public class clsPaymentMethodDataAccess
    {

        static public bool GetPaymentMethodByID(ref int PaymentMethodID,ref string PaymentMethodName,ref bool isAvailable)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"SELECT * FROM PaymentMethods WHERE PaymentMethodID = @PaymentMethodID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PaymentMethodID", PaymentMethodID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();


                if (reader.Read())
                {
                    // the Person Was Successfully Found

                    isFound = true;

                    PaymentMethodID = (int)reader["PaymentMethodID"];

                    PaymentMethodName = (string)reader["PaymentMethodName"];

                    isAvailable = (bool)reader["isAvailable"];

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

        static public int AddNewPaymentMethod(string PaymentMethodName,bool isAvailable)
        {

            int PaymentMethodID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"INSERT INTO PaymentMethods (PaymentMethodName, isAvailable)
                                    VALUES
                                        (@PaymentMethodName, @isAvailable);
                                                     SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PaymentMethodName", PaymentMethodName);

            command.Parameters.AddWithValue("@isAvailable", isAvailable);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != DBNull.Value && int.TryParse(result.ToString(), out int InsertedID))
                {
                    PaymentMethodID = InsertedID;
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

            return PaymentMethodID;
        }

        static public bool UpdatePaymentMethod(int PaymentMethodID,string PaymentMethodName, bool isAvailable)
        {
            int RowsAffected = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"UPDATE PaymentMethods
                               SET  PaymentDate = @PaymentDate,                                    
                                    PaymentMethodName = @PaymentMethodName,
                                    isAvailable = @isAvailable
                                        WHERE PaymentMethodID = @PaymentMethodID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PaymentMethodID", PaymentMethodID);

            command.Parameters.AddWithValue("@PaymentMethodName", PaymentMethodName);

            command.Parameters.AddWithValue("@isAvailable", isAvailable);

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

        static public bool DeletePaymentMethod(int PaymentMethodID)
        {
            int RowsAffected = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"DELETE PaymentMethods                               
                                WHERE PaymentMethodID = @PaymentMethodID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PaymentMethodID", PaymentMethodID);

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

        static public DataTable GetAllPaymentMethods()
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"SELECT * FROM PaymentMethods";

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

        static public bool isPaymentMethodExists(int PaymentMethodID)
        {
            // this fuction will returns true when RowsAffected > 0 and flase if not 
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"SELECT IsFound = 1 FROM PaymentMethods
                             WHERE PaymentMethodID = @PaymentMethodID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PaymentMethodID", PaymentMethodID);

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
