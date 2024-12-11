using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using staticCRUD.Models;
using staticCRUD.Helper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace staticCRUD.Controllers
{
    [CheckAccess]
    public class CityController : Controller
    {
        private readonly IConfiguration _configuration;

        #region configuration
        public CityController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region SelectAll
        public IActionResult SelectAll()
        {
            string connectionString = _configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_LOC_City_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            connection.Close();

            return View(table);
        }
        #endregion

        #region DELETE
        public IActionResult DeleteCity(string CityID)
        {
            try
            {
                // Decrypt the CityID
                int decryptedCityID = Convert.ToInt32(UrlEncryptor.Decrypt(CityID.ToString()));

                string connectionString = _configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_LOC_City_Delete";
                command.Parameters.Add("@CityID", SqlDbType.Int).Value = decryptedCityID; // Use decryptedCityID instead
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                Console.WriteLine(ex.ToString());
            }
            return RedirectToAction("SelectAll");
        }
        #endregion

        #region Add or Edit
        public IActionResult AddCity(string? CityID)
        {
            CityModel cityModel = new CityModel();
            LoadCountryList(); // Load country list

            // Decrypt the CityID if provided
            int? decryptedCityID = null;
            if (!string.IsNullOrEmpty(CityID))
            {
                string decryptedCityIDString = UrlEncryptor.Decrypt(CityID); // Decrypt the encrypted CityID
                decryptedCityID = int.Parse(decryptedCityIDString); // Convert decrypted string to integer
            }

            if (decryptedCityID.HasValue)
            {
                string connectionString = _configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_LOC_City_SelectByPK";
                command.Parameters.Add("@CityID", SqlDbType.Int).Value = decryptedCityID.Value;
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    cityModel.CityID = Convert.ToInt32(reader["CityID"]);
                    cityModel.StateID = Convert.ToInt32(reader["StateID"]);
                    cityModel.CountryID = Convert.ToInt32(reader["CountryID"]);
                    cityModel.CityName = reader["CityName"].ToString();
                    cityModel.CityCode = reader["CityCode"].ToString();
                    ViewBag.StateList = GetStateByCountryID(cityModel.CountryID);
                }
                connection.Close();
            }
            return View(cityModel);
        }
        #endregion

        #region GetStatesByCountry
        [HttpPost]
        public JsonResult GetStatesByCountry(int CountryID)
        {
            List<StateDropDownModel> stateList = GetStateByCountryID(CountryID);
            return Json(stateList);
        }
        #endregion

        #region GetStateByCountryID
        public List<StateDropDownModel> GetStateByCountryID(int CountryID)
        {
            string connectionString = _configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_state_DropDown";
            command.Parameters.AddWithValue("@CountryID", CountryID);
            SqlDataReader reader = command.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);
            List<StateDropDownModel> stateList = new List<StateDropDownModel>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                StateDropDownModel stateDropDownModel = new StateDropDownModel
                {
                    StateID = Convert.ToInt32(dataRow["StateID"]),
                    StateName = dataRow["StateName"].ToString()
                };
                stateList.Add(stateDropDownModel);
            }
            ViewBag.StateList = stateList;
            connection.Close();
            return stateList;
        }
        #endregion

        #region LoadCountryList
        private void LoadCountryList()
        {
            string connectionString = _configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Country_DropDown";
            SqlDataReader reader = command.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);
            List<CountryDropDownModel> countryList = new List<CountryDropDownModel>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                CountryDropDownModel countryDropDownModel = new CountryDropDownModel
                {
                    CountryID = Convert.ToInt32(dataRow["CountryID"]),
                    CountryName = dataRow["CountryName"].ToString()
                };
                countryList.Add(countryDropDownModel);
            }
            ViewBag.CountryList = countryList;
            connection.Close();
        }
        #endregion

        #region Save
        public IActionResult Save(CityModel cityModel)
        {
            try
            {
                Console.WriteLine($"Received CityID: {cityModel.CityID}");  // Log the received CityID for debugging

                // Decrypt the CityID if needed (only if you're passing an encrypted ID in the URL)
                if (cityModel.CityID != 0 && cityModel.CityID.ToString().StartsWith("EncryptedPrefix")) // Replace with actual condition
                {
                    int decryptedCityID = Convert.ToInt32(UrlEncryptor.Decrypt(cityModel.CityID.ToString()));
                    cityModel.CityID = decryptedCityID;
                }

                string connectionString = this._configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;

                if (cityModel.CityID == 0)  // Check if it's a new city
                {
                    command.CommandText = "PR_LOC_City_Insert";
                }
                else  // If it's an existing city, update it
                {
                    command.CommandText = "PR_LOC_City_Update";
                    command.Parameters.Add("@CityID", SqlDbType.Int).Value = cityModel.CityID;
                }

                command.Parameters.Add("@StateID", SqlDbType.Int).Value = cityModel.StateID;
                command.Parameters.Add("@CountryID", SqlDbType.Int).Value = cityModel.CountryID;
                command.Parameters.Add("@CityName", SqlDbType.VarChar).Value = cityModel.CityName;
                command.Parameters.Add("@CityCode", SqlDbType.VarChar).Value = cityModel.CityCode;
                command.ExecuteNonQuery();
                connection.Close();

                return RedirectToAction("SelectAll");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while saving the city: " + ex.Message;
                Console.WriteLine(ex);
                return View(cityModel);  // Return the model with error message
            }
        }

        #endregion
    }
}
