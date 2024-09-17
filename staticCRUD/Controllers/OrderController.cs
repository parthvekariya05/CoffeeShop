using Microsoft.AspNetCore.Mvc;
using staticCRUD.Models;
using System.Data.SqlClient;
using System.Data;

namespace staticCRUD.Controllers
{
    public class OrderController : Controller
    {
        #region Configurtions
        private IConfiguration configuration;
        public OrderController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        #endregion
        #region SelectAll
        public IActionResult Order()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Order_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }
        #endregion
        #region Delete
        public IActionResult DeleteOrder(int OrderID)
        {
            try
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_Order_DeleteByPk";
                command.Parameters.Add("@OrderID", SqlDbType.Int).Value = OrderID;
                command.ExecuteNonQuery();
                return RedirectToAction("Order");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                Console.WriteLine(ex.ToString());
                return RedirectToAction("Order");
            }
        }
        #endregion
        #region Add or Edit
        public IActionResult AddOrder(int? OrderID)
        {

            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection1 = new SqlConnection(connectionString);
            connection1.Open();
            SqlCommand command1 = connection1.CreateCommand();
            command1.CommandType = System.Data.CommandType.StoredProcedure;
            command1.CommandText = "PR_Customer_DropDown";
            SqlDataReader reader1 = command1.ExecuteReader();
            DataTable dataTable1 = new DataTable();
            dataTable1.Load(reader1);
            List<CustomerDropDownModel> customerList = new List<CustomerDropDownModel>();
            foreach (DataRow data in dataTable1.Rows)
            {
                CustomerDropDownModel customerDropDownModel = new CustomerDropDownModel();
                customerDropDownModel.CustomerID = Convert.ToInt32(data["CustomerID"]);
                customerDropDownModel.CustomerName = data["CustomerName"].ToString();
                customerList.Add(customerDropDownModel);
            }
            ViewBag.CustomerList = customerList;

            command1.CommandText = "PR_User_DropDown";
            reader1 = command1.ExecuteReader();
            DataTable dataTable2 = new DataTable();
            dataTable2.Load(reader1);
            List<UserDropDownModel> usertList = new List<UserDropDownModel>();
            foreach (DataRow data in dataTable2.Rows)
            {
                UserDropDownModel userDropDownModel = new UserDropDownModel();
                userDropDownModel.UserID = Convert.ToInt32(data["UserID"]);
                userDropDownModel.UserName = data["UserName"].ToString();
                usertList.Add(userDropDownModel);
            }
            ViewBag.UserList = usertList;

            if (OrderID != null)
            {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_Order_SelectByPk";
                command.Parameters.Add("@OrderID", SqlDbType.Int).Value = OrderID;
                SqlDataReader reader = command.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);
                OrderModel data = new OrderModel();
                foreach (DataRow dr in table.Rows)
                {
                    data.OrderID = Convert.ToInt32(@dr["OrderID"]);
                    data.CustomerID = Convert.ToInt32(@dr["CustomerID"]);
                    data.PaymentMode = @dr["PaymentMode"].ToString();
                    data.OrderNumber = @dr["OrderNumber"].ToString();
                    data.OrderDate = Convert.ToDateTime(@dr["OrderDate"]);
                    data.TotalAmount = Convert.ToDecimal(@dr["TotalAmount"]);
                    data.ShippingAddress = @dr["ShippingAddress"].ToString();
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
        public IActionResult Save(OrderModel orderModel)
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Order_Insert";

            if (orderModel.OrderID > 0)
            {
                command.CommandText = "PR_Order_UpdateByPk";
                command.Parameters.Add("@OrderID", SqlDbType.Int).Value = orderModel.OrderID;
            }
            command.Parameters.Add("@OrderNumber", SqlDbType.VarChar).Value = orderModel.OrderNumber;
            command.Parameters.Add("@OrderDate", SqlDbType.DateTime).Value = orderModel.OrderDate;
            command.Parameters.Add("@CustomerID", SqlDbType.Int).Value = orderModel.CustomerID;
            command.Parameters.Add("@PaymentMode", SqlDbType.VarChar).Value = orderModel.PaymentMode;
            command.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = orderModel.TotalAmount;
            command.Parameters.Add("@ShippingAddress", SqlDbType.VarChar).Value = orderModel.ShippingAddress;
            command.Parameters.Add("@UserID", SqlDbType.Int).Value = orderModel.UserID;
            
            if (command.ExecuteNonQuery() > 0)
            {
                TempData["OrderInsertMsg"] = orderModel.OrderID == null ? "Record Inserted Successfully" : "Record Updated Successfully";
            }
            connection.Close();
            return RedirectToAction("Order");
        }
        #endregion
    }
}