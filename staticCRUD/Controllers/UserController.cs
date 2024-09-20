using Microsoft.AspNetCore.Mvc;
using staticCRUD.Models;
using System.Data.SqlClient;
using System.Data;

namespace staticCRUD.Controllers
{
    public class UserController : Controller
    {
        #region Configurtions
        private IConfiguration configuration;
        
        public UserController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        #endregion
        #region SeleteAll
        public IActionResult User()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_User_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }
        #endregion
        #region Delete
        public IActionResult DeleteUser(int UserID)
        {
            try 
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_User_DeleteByPk";
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = UserID;
                command.ExecuteNonQuery();
                return RedirectToAction("User");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                Console.WriteLine(ex.ToString());
                return RedirectToAction("User");
            }
        }
        #endregion
        #region Add or Edit
        public IActionResult AddUser(int? UserID)
        {

            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection1 = new SqlConnection(connectionString);
            connection1.Open();
            SqlCommand command1 = connection1.CreateCommand();
            command1.CommandType = System.Data.CommandType.StoredProcedure;
           
          

            if (UserID > 0)
            {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_User_SelectByPk";
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = UserID;
                SqlDataReader reader = command.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);
                UserModel userModel = new UserModel();
                foreach (DataRow dr in table.Rows)
                {
                    userModel.UserID = Convert.ToInt32(dr["UserID"]);
                    userModel.UserName = dr["UserName"].ToString();
                    userModel.Email = dr["Email"].ToString();
                    userModel.Password = dr["Password"].ToString();
                    userModel.MobileNo = dr["MobileNo"].ToString();
                    userModel.Address = dr["Address"].ToString();
                    userModel.IsActive = Convert.ToBoolean(dr["IsActive"]);
                }
                return View(userModel);
            }
            else
            {
                return View();
            }
        }
        #endregion
        #region Save
        [HttpPost]
        public IActionResult Save(UserModel userModel)
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_User_Insert";

            if (userModel.UserID > 0)
            {
                command.CommandText = "PR_User_UpdateByPk";
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = userModel.UserID;
            }

            command.Parameters.Add("@UserName", SqlDbType.VarChar).Value = userModel.UserName;
            command.Parameters.Add("@Email", SqlDbType.VarChar).Value = userModel.Email;
            command.Parameters.Add("@Password", SqlDbType.VarChar).Value = userModel.Password;
            command.Parameters.Add("@MobileNo", SqlDbType.VarChar).Value = userModel.MobileNo;
            command.Parameters.Add("@Address", SqlDbType.VarChar).Value = userModel.Address;
            command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = userModel.IsActive;
            if (command.ExecuteNonQuery() > 0)
            {
                TempData["UserInsertMsg"] = userModel.UserID == null ? "Record Inserted Successfully" : "Record Updated Successfully";
            }
            connection.Close();
            return RedirectToAction("User");
        }
        #endregion
        #region Login
        public IActionResult UserLogin(UserLoginModel userLoginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string connectionString = this.configuration.GetConnectionString("ConnectionString");
                    SqlConnection sqlConnection = new SqlConnection(connectionString);
                    sqlConnection.Open();
                    SqlCommand sqlCommand = sqlConnection.CreateCommand();
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.CommandText = "PR_User_Login";
                    sqlCommand.Parameters.Add("@UserName", SqlDbType.VarChar).Value = userLoginModel.UserName;
                    sqlCommand.Parameters.Add("@Password", SqlDbType.VarChar).Value = userLoginModel.Password;
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    DataTable dataTable = new DataTable();
                    dataTable.Load(sqlDataReader);
                    if (dataTable.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dataTable.Rows)
                        {
                            HttpContext.Session.SetString("UserID", dr["UserID"].ToString());
                            HttpContext.Session.SetString("UserName", dr["UserName"].ToString());
                        }

                        return RedirectToAction("Index", "Home");

                    }
                    else
                    {
                        return RedirectToAction("Login", "User");
                    }

                }
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
            }

            return RedirectToAction("Login");
        }
        public IActionResult Login()
        {
            return View();
        }
        #endregion
        #region Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            Response.Cookies.Delete(".AspNetCore.Session");
            return RedirectToAction("Login", "User");
        }
        #endregion
        #region Register
        public IActionResult UserRegister(UserRegisterModel userRegisterModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string connectionString = this.configuration.GetConnectionString("ConnectionString");
                    SqlConnection sqlConnection = new SqlConnection(connectionString);
                    sqlConnection.Open();
                    SqlCommand sqlCommand = sqlConnection.CreateCommand();
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.CommandText = "PR_User_Insert";
                    sqlCommand.Parameters.Add("@UserName", SqlDbType.VarChar).Value = userRegisterModel.UserName;
                    sqlCommand.Parameters.Add("@Password", SqlDbType.VarChar).Value = userRegisterModel.Password;
                    sqlCommand.Parameters.Add("@Email", SqlDbType.VarChar).Value = userRegisterModel.Email;
                    sqlCommand.Parameters.Add("@MobileNo", SqlDbType.VarChar).Value = userRegisterModel.MobileNo;
                    sqlCommand.Parameters.Add("@Address", SqlDbType.VarChar).Value = userRegisterModel.Address;
                    sqlCommand.Parameters.Add("@IsActive", SqlDbType.Bit).Value = true;
                    sqlCommand.ExecuteNonQuery();
                    return RedirectToAction("Login", "User");
                }
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
                return RedirectToAction("Register");
            }
            return RedirectToAction("Register");
        }

        public IActionResult Register()
        {
            return View();
        }
        #endregion

    }
}
