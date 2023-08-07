using FoodTypesBllProj;
using FoodTypesDalProj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace _3TL.Controllers
{
    [RoutePrefix("api/foodTypes")]
    public class FoodTypesController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            try
            {
                FoodType[] foodTypes = FoodTypesBll.GetAllFoodTypes();
                return Ok(foodTypes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}