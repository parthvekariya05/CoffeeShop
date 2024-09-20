using Microsoft.AspNetCore.Mvc;
using staticCRUD.Models;
using System.Data.SqlClient;
using System.Data;

namespace staticCRUD.Controllers
{
    [CheckAccess]
    public class ProductController : Controller
    {
        #region Configurtions
        private IConfiguration configuration;

        public ProductController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        #endregion

        #region SeleteAll
        public IActionResult ProductList()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Product_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }
        #endregion

        #region Delete
        public IActionResult DeleteProduct(int ProductID)
        {
            try
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_Product_DeleteByPk";
                command.Parameters.Add("@ProductID", SqlDbType.Int).Value = ProductID;
                command.ExecuteNonQuery();
                return RedirectToAction("ProductList");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                Console.WriteLine(ex.ToString());
                return RedirectToAction("ProductList");
            }
        }
        #endregion

        #region Add or Edit
        public IActionResult AddProduct(int? ProductID)
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
            List<UserDropDownModel> userList = new List<UserDropDownModel>();
            foreach (DataRow data in dataTable1.Rows)
            {
                UserDropDownModel userDropDownModel = new UserDropDownModel();
                userDropDownModel.UserID = Convert.ToInt32(data["UserID"]);
                userDropDownModel.UserName = data["UserName"].ToString();
                userList.Add(userDropDownModel);
            }
            ViewBag.UserList = userList;
            if (ProductID != null)
            {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_Product_SelectByPk";
                command.Parameters.Add("@ProductID", SqlDbType.Int).Value = ProductID;
                SqlDataReader reader = command.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);
                ProductModel data = new ProductModel();
                foreach (DataRow dr in table.Rows)
                {
                    data.ProductID = Convert.ToInt32(@dr["ProductID"]);
                    data.ProductName = @dr["ProductName"].ToString();
                    data.ProductCode = @dr["ProductCode"].ToString();
                    data.ProductPrice = Convert.ToDecimal(@dr["ProductPrice"]);
                    data.Description = @dr["Description"].ToString();
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
        public IActionResult Save(ProductModel productModel)
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;

            if (productModel.ProductID == null)
            {
                command.CommandText = "PR_Product_Insert";
            }
            else
            {
                command.CommandText = "PR_Product_UpdateByPk";
                command.Parameters.Add("@ProductID", SqlDbType.Int).Value = productModel.ProductID;
            }
            command.Parameters.Add("@ProductName", SqlDbType.VarChar).Value = productModel.ProductName;
            command.Parameters.Add("@ProductPrice", SqlDbType.Decimal).Value = productModel.ProductPrice;
            command.Parameters.Add("@ProductCode", SqlDbType.VarChar).Value = productModel.ProductCode;
            command.Parameters.Add("@Description", SqlDbType.VarChar).Value = productModel.Description;
            command.Parameters.Add("@UserID", SqlDbType.Int).Value = productModel.UserID;
            if (command.ExecuteNonQuery() > 0)
            {
                TempData["ProductInsertMsg"] = productModel.ProductID == null ? "Record Inserted Successfully" : "Record Updated Successfully";
            }
            connection.Close();
            return RedirectToAction("ProductList");
        }
        #endregion
    }
}
