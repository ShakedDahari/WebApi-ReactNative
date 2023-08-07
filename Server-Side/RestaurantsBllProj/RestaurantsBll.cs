using RestaurantsDalProj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantsBllProj
{
    public static class RestaurantsBll
    {
        public static Restaurant[] GetRestaurants()
        {
            Restaurant[] restaurants = null;
            restaurants = RestaurantsDB.GetAllRestaurants();
            return restaurants;
        }

        public static Restaurant CheckEmail(string email)
        {
            Restaurant restaurant = null;
            restaurant = RestaurantsDB.CheckEmail(email);
            return restaurant;
        }

        public static Restaurant Login(string email, string password) 
        {
            Restaurant restaurant = null;
            restaurant = RestaurantsDB.Login(email, password);
            return restaurant;
        }

        public static Restaurant[] GetRestaurantsByInputs(string city, string foodType)
        {
            Restaurant[] restaurants = null;
            restaurants = RestaurantsDB.GetRestaurantsByInputs(city, foodType);
            return restaurants;
        }

        public static Restaurant InsertRestaurant(Restaurant restaurant)
        {
            Restaurant newRes = RestaurantsDB.InsertRestaurant(restaurant);
            return newRes;
        }

        public static bool UpdateRestaurantApproved(int id)
        {
            return RestaurantsDB.UpdateRestaurantApproved(id);
        }

        public static Restaurant UploadMenuFile(int id, string file)
        {
            Restaurant restaurant = null;
            restaurant = RestaurantsDB.UploadFile(id, file);
            return restaurant;
        }

        public static bool SendEmail(string email) 
        { 
            return RestaurantsDB.SendEmail(email);
        }

        public static bool MakeReservation(string restEmail, string userEmail)
        {
            return RestaurantsDB.MakeReservation(restEmail, userEmail);
        }

        public static bool DeleteRestaurant(int id)
        {
            return RestaurantsDB.DeleteRestaurant(id);
        }
    }
}
