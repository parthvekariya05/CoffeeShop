using Microsoft.AspNetCore.Mvc;
using staticCRUD.Models;
using System.Data.SqlClient;
using System.Data;

namespace staticCRUD.Controllers
{
    [CheckAccess]
    public class OrderDetailController : Controller
    {
        #region Configurtions
        private IConfiguration configuration;

        public OrderDetailController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        #endregion

        #region SeleteAll
        public IActionResult OrderDetail()
        {

            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_OrderDetail_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }
        #endregion

        #region Delete
        public IActionResult DeleteOrderDetail(int OrderDetailID)
        {
            try
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_OrderDetail_DeleteByPk";
                command.Parameters.Add("@OrderDetailID", SqlDbType.Int).Value = OrderDetailID;
                command.ExecuteNonQuery();
                return RedirectToAction("OrderDetail");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                Console.WriteLine(ex.ToString());
                return RedirectToAction("OrderDetail");
            }
        }
        #endregion

        #region Add or Edit
        public IActionResult AddOrderDetail(int? OrderDetailID)
        {

            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection1 = new SqlConnection(connectionString);
            connection1.Open();
            SqlCommand command1 = connection1.CreateCommand();
            command1.CommandType = System.Data.CommandType.StoredProcedure;
            command1.CommandText = "PR_Order_DropDown";
            SqlDataReader reader1 = command1.ExecuteReader();
            DataTable dataTable1 = new DataTable();
            dataTable1.Load(reader1);
            #region Order
            List<OrderDropDownModel> orderList = new List<OrderDropDownModel>();
            foreach (DataRow data in dataTable1.Rows)
            {
                OrderDropDownModel orderDropDownModel = new OrderDropDownModel();
                orderDropDownModel.OrderID = Convert.ToInt32(data["OrderID"]);
                orderDropDownModel.OrderNumber = data["OrderNumber"].ToString();
                orderList.Add(orderDropDownModel);
            }
            ViewBag.OrderList = orderList;
            #endregion
            #region Product
            command1.CommandText = "PR_Product_DropDown";
            reader1 = command1.ExecuteReader();
            DataTable dataTable2 = new DataTable();
            dataTable2.Load(reader1);
            List<ProductDropDownModel> productList = new List<ProductDropDownModel>();
            foreach (DataRow data in dataTable2.Rows)
            {
                ProductDropDownModel productDropDownModel = new ProductDropDownModel();
                productDropDownModel.ProductID = Convert.ToInt32(data["ProductID"]);
                productDropDownModel.ProductName = data["ProductName"].ToString();
                productList.Add(productDropDownModel);
            }
            ViewBag.ProductList = productList;
            #endregion 
            #region User
            command1.CommandText = "PR_User_DropDown";
            reader1 = command1.ExecuteReader();
            DataTable dataTable3 = new DataTable();
            dataTable3.Load(reader1);
            List<UserDropDownModel> usertList = new List<UserDropDownModel>();
            foreach (DataRow data in dataTable3.Rows)
            {
                UserDropDownModel userDropDownModel = new UserDropDownModel();
                userDropDownModel.UserID = Convert.ToInt32(data["UserID"]);
                userDropDownModel.UserName = data["UserName"].ToString();
                usertList.Add(userDropDownModel);
            }
            ViewBag.UserList = usertList;
            #endregion 
            if (OrderDetailID != null)
            {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_OrderDetail_SelectByPk";
                command.Parameters.Add("@OrderDetailID", SqlDbType.Int).Value = OrderDetailID;
                SqlDataReader reader = command.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);
                OrderDetailModel data = new OrderDetailModel();
                foreach (DataRow dr in table.Rows)
                {
                    data.OrderDetailID = Convert.ToInt32(@dr["OrderDetailID"]);
                    data.Amount = Convert.ToDecimal(@dr["Amount"]);
                    data.TotalAmount = Convert.ToDecimal(@dr["TotalAmount"]);
                    data.OrderID = Convert.ToInt32(@dr["OrderID"]);
                    data.ProductID = Convert.ToInt32(@dr["ProductID"]);
                    data.Quantity = Convert.ToInt32(@dr["Quantity"]);
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
        public IActionResult Save(OrderDetailModel orderDetailModel)
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_OrderDetail_Insert";

            if (orderDetailModel.OrderDetailID > 0)
            {
                command.CommandText = "PR_OrderDetail_UpdateByPk";
                command.Parameters.Add("@OrderDetailID", SqlDbType.Int).Value = orderDetailModel.OrderDetailID;
            }

            command.Parameters.Add("@OrderID", SqlDbType.Int).Value = orderDetailModel.OrderID;
            command.Parameters.Add("@ProductID", SqlDbType.Int).Value = orderDetailModel.ProductID;
            command.Parameters.Add("@Quantity", SqlDbType.Int).Value = orderDetailModel.Quantity;
            command.Parameters.Add("@Amount", SqlDbType.Decimal).Value = orderDetailModel.Amount;
            command.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = orderDetailModel.TotalAmount;
            command.Parameters.Add("@UserID", SqlDbType.Int).Value = orderDetailModel.UserID;
            if (command.ExecuteNonQuery() > 0)
            {
                TempData["OrderDetailInsertMsg"] = orderDetailModel.OrderDetailID == null ? "Record Inserted Successfully" : "Record Updated Successfully";
            }
            connection.Close();
            return RedirectToAction("OrderDetail");
        }
        #endregion
    }
}
