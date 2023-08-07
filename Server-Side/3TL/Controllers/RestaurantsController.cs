using _3TL.Models;
using RestaurantsBllProj;
using RestaurantsDalProj;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace _3TL.Controllers
{
    [RoutePrefix("api/restaurants")]
    public class RestaurantsController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            try
            {
                Restaurant[] restaurant = RestaurantsBll.GetRestaurants();
                return Ok(restaurant);
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
                Restaurant restaurant = RestaurantsBll.CheckEmail(email);
                if (restaurant != null)
                    return Ok(restaurant); 
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("find")]
        public IHttpActionResult GetByInputs([FromBody] RestaurantFind rest)
        {
            try
            {
                Restaurant[] restaurant = RestaurantsBll.GetRestaurantsByInputs(rest.city, rest.foodType);
                if (restaurant.Length == 0)
                {
                    return NotFound();
                }
                return Ok(restaurant);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("login")]
        public IHttpActionResult PostLogin([FromBody] RestaurantLoginDTO loginData)
        {
            try
            {
                Restaurant restaurant = RestaurantsBll.Login(loginData.email, loginData.password);
                if (restaurant == null)
                    return NotFound();
                return Ok(restaurant);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.Conflict, ex.Message);
            }
        }


        [HttpPost]
        [Route("add")]
        public IHttpActionResult Post([FromBody] Restaurant restaurant)
        {
            try
            {
                Restaurant r = RestaurantsBll.InsertRestaurant(restaurant);
                if (r != null)
                {
                    return Ok(r);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut]
        [Route("approved/{id}")]
        public IHttpActionResult PutApproved([FromUri] int id)
        {
            try
            {
                if (RestaurantsBll.UpdateRestaurantApproved(id))
                    return Ok(id);
                return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("menu")]
        public IHttpActionResult PostFile([FromBody] MenuUpload request)
        {
            try
            {
                Restaurant restaurant = RestaurantsBll.UploadMenuFile(request.id, request.file);
                if (restaurant == null)
                    return NotFound();
                return Ok(restaurant);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.Conflict, ex.Message);
            }
        }

        [HttpPost]
        [Route("sendemail")]
        public IHttpActionResult SendEmail([FromBody] EmailModal emailModal) 
        {
            try
            {
                if (RestaurantsBll.SendEmail(emailModal.email))
                    return Ok(emailModal.email);
                return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("reservation")]
        public IHttpActionResult MakeReservation([FromBody] ReservationData reservation)
        {
            try
            {
                if (RestaurantsBll.MakeReservation(reservation.restEmail, reservation.userEmail))
                    return Ok(reservation);
                return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpDelete]
        [Route("delete/{id}")]
        public IHttpActionResult Delete([FromUri] int id)
        {
            try
            {
                if (RestaurantsBll.DeleteRestaurant(id))
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
