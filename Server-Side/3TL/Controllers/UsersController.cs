using _3TL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UsersBllProj;
using UsersDalProj;

namespace _3TL.Controllers
{
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    { 
        [HttpGet]
        public IHttpActionResult Get()
        {
			try
			{
				User[] users = UsersBll.GetUsers();
				return Ok(users);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
        }

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            try
            {
                User user = UsersBll.GetUser(id);
                if (user.username == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("add")]
        public IHttpActionResult Post([FromBody] User user)
        {
            try
            {
                User u = UsersBll.InsertUser(user);
                if (u != null)
                {
                    return Ok(u);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        [Route("email")]
        public IHttpActionResult GetEmail([FromUri] string email)
        {
            try
            {
                User user = UsersBll.CheckEmail(email);
                if (user != null) 
                    return Ok(user);
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Route("login")]
        public IHttpActionResult PostLogin([FromBody] UserLoginDTO loginData)
        {
            try
            {
                User user = UsersBll.Login(loginData.email, loginData.password);
                if (user == null)
                    return NotFound();
                return Ok(user);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.Conflict, ex.Message);
            }
        }


        [HttpDelete]
        [Route("delete/{id}")]
        public IHttpActionResult Delete([FromUri] int id)
        {
            try
            {
                if (UsersBll.DeleteUser(id))
                    return Ok(id);
                return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
