using Microsoft.AspNetCore.Mvc;
using staticCRUD.Models;
using System.Data.SqlClient;
using System.Data;

namespace staticCRUD.Controllers
{
    [CheckAccess]
    public class CustomerController : Controller
    {
        #region Configurtions
        private IConfiguration configuration;

        public CustomerController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        #endregion

        #region SelectAll
        public IActionResult Customer()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Customer_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }
        #endregion

        #region Delete
        public IActionResult DeleteCustomer(int CustomerID)
        {
            try
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_Customer_DeleteByPk";
                command.Parameters.Add("@CustomerID", SqlDbType.Int).Value = CustomerID;
                command.ExecuteNonQuery();
                return RedirectToAction("Customer");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                Console.WriteLine(ex.ToString());
                return RedirectToAction("Customer");
            }
        }
        #endregion

        #region Add or Edit
        public IActionResult AddCustomer(int? CustomerID)
        {

            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection1 = new SqlConnection(connectionString);
            connection1.Open();
            SqlCommand command1 = connection1.CreateCommand();
            command1.CommandType = System.Data.CommandType.StoredProcedure;
            command1.CommandText = "PR_User_DropDown";
            SqlDataReader reader1 = command1.ExecuteReader();
            DataTable dataTable1 = new DataTable();
            dataTable1.Load(reader1);
            List<UserDropDownModel> usertList = new List<UserDropDownModel>();
            foreach (DataRow data in dataTable1.Rows)
            {
                UserDropDownModel userDropDownModel = new UserDropDownModel();
                userDropDownModel.UserID = Convert.ToInt32(data["UserID"]);
                userDropDownModel.UserName = data["UserName"].ToString();
                usertList.Add(userDropDownModel);
            }
            ViewBag.UserList = usertList;

            if (CustomerID != null)
            {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_Customer_SelectByPk";
                command.Parameters.Add("@CustomerID", SqlDbType.Int).Value = CustomerID;
                SqlDataReader reader = command.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);
                CustomerModel data = new CustomerModel();
                foreach (DataRow dr in table.Rows)
                {
                    data.CustomerID = Convert.ToInt32(@dr["CustomerID"]);
                    data.CustomerName = @dr["CustomerName"].ToString();
                    data.HomeAddress = @dr["HomeAddress"].ToString();
                    data.Email = @dr["Email"].ToString();
                    data.MobileNo = @dr["MobileNo"].ToString();
                    data.GSTNO = @dr["GSTNO"].ToString();
                    data.CityName = @dr["CityName"].ToString();
                    data.PinCode = @dr["PinCode"].ToString();
                    data.NetAmount = Convert.ToDecimal(@dr["NetAmount"]);
                    data.UserID = Convert.ToInt32(@dr["UserID"]);
                }
                return View(data);
            }
            else
            {
                return View();
            }
        }
        #endregion

        #region Save
        [HttpPost]
        public IActionResult Save(CustomerModel customerModel)
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Customer_Insert";

            if (customerModel.CustomerID > 0)
            {
                command.CommandText = "PR_Customer_UpdateByPk";
                command.Parameters.Add("@CustomerID", SqlDbType.Int).Value = customerModel.CustomerID;
            }

            command.Parameters.Add("@CustomerName", SqlDbType.VarChar).Value = customerModel.CustomerName;
            command.Parameters.Add("@HomeAddress", SqlDbType.VarChar).Value = customerModel.HomeAddress;
            command.Parameters.Add("@Email", SqlDbType.VarChar).Value = customerModel.Email;
            command.Parameters.Add("@MobileNo", SqlDbType.VarChar).Value = customerModel.MobileNo;
            command.Parameters.Add("@GSTNO", SqlDbType.VarChar).Value = customerModel.GSTNO;
            command.Parameters.Add("@CityName", SqlDbType.VarChar).Value = customerModel.CityName;
            command.Parameters.Add("@PinCode", SqlDbType.VarChar).Value = customerModel.PinCode;
            command.Parameters.Add("@NetAmount", SqlDbType.Decimal).Value = customerModel.NetAmount;
            command.Parameters.Add("@UserID", SqlDbType.Int).Value = customerModel.UserID;
            if (command.ExecuteNonQuery() > 0)
            {
                TempData["customerInsertMsg"] = customerModel.CustomerID == null ? "Record Inserted Successfully" : "Record Updated Successfully";
            }
            connection.Close();
            return RedirectToAction("Customer");
        }
        #endregion
    }
}