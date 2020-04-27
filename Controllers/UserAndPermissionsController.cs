using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UserAccess.Models;
using System.Data.SqlClient;
using System.Text;
using System.Data;

namespace UserAccess.Controllers
{
    public class UserAndPermissionsController : ApiController
    {
        [ActionName("AddUser")]
        [HttpPost]
        public HttpResponseMessage AddUser([FromBody]User newUser)
        {
            try
            {
                if (string.IsNullOrEmpty(newUser.UserName))
                    throw new ArgumentException("UserName is missing.");

                else if(string.IsNullOrEmpty(newUser.FirstName))
                    throw new ArgumentException("User firstName is missing.");

                else if (string.IsNullOrEmpty(newUser.LastName))
                    throw new ArgumentException("User LastName is missing.");

                using (SqlConnection con = new SqlConnection(@"sql connection string"))
                {
                    using (SqlCommand sqlCommand = con.CreateCommand())
                    {
                        sqlCommand.CommandType = CommandType.Text;
                        sqlCommand.CommandText = ($"Insert into User(UserName, FirstName,LastName) VALUES (@UserName, @FirstName, @LastName)");
                        sqlCommand.Parameters.AddWithValue("@UserName", newUser.UserName);
                        sqlCommand.Parameters.AddWithValue("@FirstName", newUser.FirstName);
                        sqlCommand.Parameters.AddWithValue("@LastName", newUser.LastName);
                        con.Open();
                        sqlCommand.ExecuteNonQuery();
                        con.Close();
                    }
                }

                return this.Request.CreateResponse(HttpStatusCode.OK, "User is added successfully");
            }
            catch (ArgumentException exception)
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                   exception.Message);
            }
            catch (Exception ex)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [ActionName("AddPagePermissions")]
        [HttpPost]
        public HttpResponseMessage AddPagePermissions([FromBody]PagePermissions addAccess)
        {
            try
            {
                if (addAccess.UserId==0)
                    throw new ArgumentException("UserId is missing.");

                else if (string.IsNullOrEmpty(addAccess.PageName))
                    throw new ArgumentException("Page is not selected.");

              
                using (SqlConnection con = new SqlConnection(@"sql connection string"))
                {
                    using (SqlCommand sqlCommand = con.CreateCommand())
                    {
                        sqlCommand.CommandType = CommandType.Text;
                        sqlCommand.CommandText = ($"Insert into PagePermissions(PageName,UserId) VALUES (@PageName,@UserId)");
                        sqlCommand.Parameters.AddWithValue("@UserId", addAccess.UserId);
                        sqlCommand.Parameters.AddWithValue("@PageName", addAccess.PageName);
                        con.Open();
                        sqlCommand.ExecuteNonQuery();
                        con.Close();
                    }
                }
                return this.Request.CreateResponse(HttpStatusCode.OK, "Permission is set successfully");
            }
            catch (ArgumentException exception)
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                   exception.Message);
            }
            catch (Exception ex)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }


        // PUT: api/UserAndPermissions/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/UserAndPermissions/5
        public void Delete(int id)
        {
        }
    }
}
