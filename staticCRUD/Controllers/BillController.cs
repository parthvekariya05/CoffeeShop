using Microsoft.AspNetCore.Mvc;
using staticCRUD.Models;
using System.Data.SqlClient;
using System.Data;

namespace staticCRUD.Controllers
{
    [CheckAccess]
    public class BillController : Controller
    {
        #region Configurtions
        private IConfiguration configuration;
        public BillController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        #endregion
        #region SelectAll
        public IActionResult Bill()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Bills_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }
        #endregion
        #region Delete
        public IActionResult DeleteBill(int BillID)
        {
            try
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_Bills_DeleteByPk";
                command.Parameters.Add("@BillID", SqlDbType.Int).Value = BillID;
                command.ExecuteNonQuery();
                return RedirectToAction("Bill");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                Console.WriteLine(ex.ToString());
                return RedirectToAction("Bill");
            }
        }
        #endregion
        #region Add or Edit
        public IActionResult AddBill(int? BillID)
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
            List<OrderDropDownModel> orderList = new List<OrderDropDownModel>();
            foreach (DataRow data in dataTable1.Rows)
            {
                OrderDropDownModel orderDropDownModel = new OrderDropDownModel();
                orderDropDownModel.OrderID = Convert.ToInt32(data["OrderID"]);
                orderDropDownModel.OrderNumber = data["OrderNumber"].ToString();
                orderList.Add(orderDropDownModel);
            }
            ViewBag.OrderList = orderList;

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
            if (BillID != null)
            {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_Bills_SelectByPk";
                command.Parameters.Add("@BillID", SqlDbType.Int).Value = BillID;
                SqlDataReader reader = command.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);
                BillModel data = new BillModel();
                foreach (DataRow dr in table.Rows)
                {
                    data.BillID = Convert.ToInt32(@dr["BillID"]);
                    data.BillNumber = @dr["BillNumber"].ToString();
                    data.BillDate = Convert.ToDateTime(@dr["BillDate"]);
                    data.OrderID = Convert.ToInt32(@dr["OrderID"]);
                    data.TotalAmount = Convert.ToInt32(@dr["TotalAmount"]);
                    data.Discount = Convert.ToInt32(@dr["Discount"]);
                    data.NetAmount = Convert.ToInt32(@dr["NetAmount"]);
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
        public IActionResult Save(BillModel billModel)
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Bills_Insert";

            if (billModel.BillID > 0)
            {
                command.CommandText = "PR_Bills_UpdateByPk";
                command.Parameters.Add("@BillID", SqlDbType.Int).Value = billModel.BillID;
            }
            command.Parameters.Add("@BillNumber", SqlDbType.VarChar).Value = billModel.BillNumber;
            command.Parameters.Add("@BillDate", SqlDbType.DateTime).Value = billModel.BillDate;
            command.Parameters.Add("@OrderID", SqlDbType.Int).Value = billModel.OrderID;
            command.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = billModel.TotalAmount;
            command.Parameters.Add("@Discount", SqlDbType.Decimal).Value = billModel.Discount;
            command.Parameters.Add("@NetAmount", SqlDbType.Decimal).Value = billModel.NetAmount;
            command.Parameters.Add("@UserID", SqlDbType.Int).Value = billModel.UserID;
            if (command.ExecuteNonQuery() > 0)
            {
                TempData["BillInsertMsg"] = billModel.BillID == null ? "Record Inserted Successfully" : "Record Updated Successfully";
            }
            connection.Close();
            return RedirectToAction("Bill");
        }
        #endregion
    }
}