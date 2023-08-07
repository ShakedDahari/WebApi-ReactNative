using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace RestaurantsDalProj
{
    public static class RestaurantsDB
    {
        static string conStr = ConfigurationManager.ConnectionStrings["serverDB"].ConnectionString;
        static SqlConnection con = new SqlConnection(conStr);


        public static Restaurant[] GetAllRestaurants()
        {
            string query = $"SELECT * FROM restaurants";
            SqlCommand cmd = new SqlCommand(query, con);

            try
            {
                cmd.Connection.Open();

                List<Restaurant> restaurants = new List<Restaurant>();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    restaurants.Add(new Restaurant()
                    {
                        _id = (int)reader["_id"],
                        name = (string)reader["name"],
                        email = (string)reader["email"],
                        phone = (string)reader["phone"],
                        password = (string)reader["password"],
                        verify = (string)reader["verify"],
                        image = (string)reader["image"],
                        location = (string)reader["location"],
                        foodType = (string)reader["foodType"],
                        approved = (string)reader["approved"],
                        createdAt = (DateTime)reader["createdAt"],
                        menu = (string)reader["menu"],
                    });
                }

                return restaurants.ToArray();
            }
            catch (Exception)
            {
                return null;
            }
            finally { cmd.Connection.Close(); }
        }

        public static Restaurant CheckEmail(string email)
        {
            string query = $"SELECT * FROM restaurants WHERE email = @Email";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Email", email);

            try
            {
                cmd.Connection.Open();

                Restaurant restaurant = null;
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    restaurant = new Restaurant()
                    {
                        _id = (int)reader["_id"],
                        name = (string)reader["name"],
                        email = (string)reader["email"],
                        phone = (string)reader["phone"],
                        password = (string)reader["password"],
                        verify = (string)reader["verify"],
                        image = (string)reader["image"],
                        location = (string)reader["location"],
                        foodType = (string)reader["foodType"],
                        approved = (string)reader["approved"],
                        createdAt = (DateTime)reader["createdAt"],
                        menu = (string)reader["menu"],
                    };
                }

                return restaurant;
            }
            catch (Exception)
            {
                return null;
            }
            finally { cmd.Connection.Close(); }
        }

        public static Restaurant Login(string email, string password)
        {
            string query = $"SELECT * FROM restaurants WHERE email='{email}' AND password='{password}'";
            SqlCommand cmd = new SqlCommand(query, con);

            try
            {
                cmd.Connection.Open();

                Restaurant restaurant = null;
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    restaurant = new Restaurant()
                    {
                        _id = (int)reader["_id"],
                        name = (string)reader["name"],
                        email = (string)reader["email"],
                        phone = (string)reader["phone"],
                        password = (string)reader["password"],
                        verify = (string)reader["verify"],
                        image = (string)reader["image"],
                        location = (string)reader["location"],
                        foodType = (string)reader["foodType"],
                        approved = (string)reader["approved"],
                        createdAt = (DateTime)reader["createdAt"],
                        menu = (string)reader["menu"],
                    };
                }

                return restaurant;
            }
            catch (Exception)
            {
                return null;
            }
            finally { cmd.Connection.Close(); }
        }

        public static Restaurant[] GetRestaurantsByInputs(string city, string foodType)
        {
            string query = $"SELECT * FROM restaurants WHERE location='{city}' AND foodType='{foodType}'";
            SqlCommand cmd = new SqlCommand(query, con);

            try
            {
                cmd.Connection.Open();

                List<Restaurant> restaurants = new List<Restaurant>();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    restaurants.Add(new Restaurant()
                    {
                        _id = (int)reader["_id"],
                        name = (string)reader["name"],
                        email = (string)reader["email"],
                        phone = (string)reader["phone"],
                        password = (string)reader["password"],
                        verify = (string)reader["verify"],
                        image = (string)reader["image"],
                        location = (string)reader["location"],
                        foodType = (string)reader["foodType"],
                        approved = (string)reader["approved"],
                        createdAt = (DateTime)reader["createdAt"],
                        menu = (string)reader["menu"],
                    });
                }

                return restaurants.ToArray();
            }
            catch (Exception)
            {
                return null;
            }
            finally { cmd.Connection.Close(); }

        }

        public static Restaurant InsertRestaurant(Restaurant restaurant)
        {
            SqlCommand comm = new SqlCommand(
               " INSERT INTO restaurants(name, email, phone, location, password, verify, image, foodType, approved, createdAt, menu)" +
               " VALUES(@NamePar, @EmailPar, @PhonePar, @LoactionPar, @PassPar, @VerifyPar, @ImgPar, @CuisinePar, @ApprovedPar, @CreatedPar, @Menu);" +
               " SELECT SCOPE_IDENTITY() as _id;", con);

            DateTime dt = DateTime.Now;

            comm.Parameters.Add(new SqlParameter("NamePar", restaurant.name));
            comm.Parameters.Add(new SqlParameter("EmailPar", restaurant.email));
            comm.Parameters.Add(new SqlParameter("PhonePar", restaurant.phone));
            comm.Parameters.Add(new SqlParameter("LoactionPar", restaurant.location));
            comm.Parameters.Add(new SqlParameter("PassPar", restaurant.password));
            comm.Parameters.Add(new SqlParameter("VerifyPar", restaurant.verify));
            comm.Parameters.Add(new SqlParameter("ImgPar", restaurant.image));
            comm.Parameters.Add(new SqlParameter("CuisinePar", restaurant.foodType));
            comm.Parameters.Add(new SqlParameter("ApprovedPar", "false"));
            comm.Parameters.Add(new SqlParameter("CreatedPar", dt));
            comm.Parameters.Add(new SqlParameter("Menu", ""));

            try
            {
                con.Open();
                int res = -1;
                SqlDataReader reader = comm.ExecuteReader();
                if (reader.Read())
                {
                    res = int.Parse(reader["_id"].ToString());
                }
                con.Close();

                if (res != -1)
                {
                    restaurant._id = res;
                    return restaurant;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
            finally { con.Close(); }
        }

        public static bool UpdateRestaurantApproved(int id)
        {
            SqlCommand comm = new SqlCommand($" UPDATE restaurants SET approved='true'" +
                $" WHERE _id='{id}'", con);

            try
            {
                con.Open();
                int res = comm.ExecuteNonQuery();
                return res == 1;
            }
            catch (Exception)
            {
                return false;
            }
            finally { con.Close(); }
        }

        public static Restaurant UploadFile(int id, string file)
        {
            SqlCommand comm = new SqlCommand(
                $" UPDATE restaurants SET menu = '{file}' WHERE _id = '{id}' " +
                $" SELECT * FROM restaurants WHERE _id = '{id}'", con);

            try
            {
                con.Open();

                Restaurant restaurant = null;
                SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    restaurant = new Restaurant()
                    {
                        _id = (int)reader["_id"],
                        name = (string)reader["name"],
                        email = (string)reader["email"],
                        phone = (string)reader["phone"],
                        password = (string)reader["password"],
                        verify = (string)reader["verify"],
                        image = (string)reader["image"],
                        location = (string)reader["location"],
                        foodType = (string)reader["foodType"],
                        approved = (string)reader["approved"],
                        createdAt = (DateTime)reader["createdAt"],
                        menu = (string)reader["menu"],
                    };
                }
                return restaurant;
            }
            catch (Exception)
            {
                return null;
            }
            finally { con.Close(); }
        }

        public static bool SendEmail(string email)
        {
            SqlCommand comm = new SqlCommand($"SELECT COUNT(*) FROM restaurants WHERE email = '{email}'", con);

            try
            {
                con.Open();
                int count = (int)comm.ExecuteScalar();

                string emailSender = "dineintimeapp@gmail.com";
                string subject = "Restaurant Approval";
                string body = "Congratulations! Your restaurant has been approved.";

                if (count > 0)
                {
                    MailMessage message = new MailMessage(emailSender, email, subject, body);
                    SmtpClient client = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(emailSender, "aqkzocxkiecsicvz")
                    };
                    client.Send(message);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
            finally { con.Close(); }
        }

        public static bool MakeReservation(string restEmail, string userEmail)
        {
            SqlCommand comm = new SqlCommand($"SELECT COUNT(*) FROM restaurants WHERE email = '{restEmail}'", con);
            DateTime date = DateTime.Now;

            try
            {
                con.Open();
                int count = (int)comm.ExecuteScalar();

                string emailSender = "dineintimeapp@gmail.com";
                string subject = "New Reservation";
                string body = $"A new reservation has been made at your restaurant.\n" +
                     $"Reservation details:\n" +
                     $"User email: {userEmail}\n" +
                     $"Reservation date: {date}\n";

                if (count > 0)
                {
                    MailMessage message = new MailMessage(emailSender, restEmail, subject, body);
                    SmtpClient client = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(emailSender, "aqkzocxkiecsicvz")
                    };
                    client.Send(message);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
            finally { con.Close(); }
        }

        public static bool DeleteRestaurant(int id)
        {
            SqlCommand comm = new SqlCommand($"DELETE FROM restaurants WHERE _id=" + id, con);

            try
            {
                con.Open();
                int res = comm.ExecuteNonQuery();
                return res == 1;
            }
            catch (Exception)
            {
                return false;
            }
            finally { con.Close(); }
        }
    }
}
