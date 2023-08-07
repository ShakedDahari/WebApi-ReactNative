using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace UsersDalProj
{
    public static class UsersDB
    {
        static string conStr = ConfigurationManager.ConnectionStrings["serverDB"].ConnectionString;
        static SqlConnection con = new SqlConnection(conStr);

        public static User[] GetAllUsers()
        {
            string query = $"SELECT * FROM users";
            SqlCommand cmd = new SqlCommand(query, con);

            try
            {
                cmd.Connection.Open();
                    
                List<User> users = new List<User>();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    users.Add(new User()
                    {
                        _id = (int)reader["_id"],
                        email = (string)reader["email"],
                        phone = (string)reader["phone"],
                        username = (string)reader["username"],
                        image = (string)reader["image"],
                        password = (string)reader["password"],
                        verify = (string)reader["verify"],
                    });
                }

                return users.ToArray();
            }
            catch (Exception)
            {
                return null;
            }
            finally { cmd.Connection.Close(); }
        }

        public static User GetUser(int id)
        {
            string query = $"SELECT * FROM users WHERE _id=" + id;
            SqlCommand cmd = new SqlCommand(query, con);

            try
            {
                cmd.Connection.Open();

                User user = new User();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    user = new User()
                    {
                        _id = (int)reader["_id"],
                        email = (string)reader["email"],
                        phone = (string)reader["phone"],
                        username = (string)reader["username"],
                        image = (string)reader["image"],
                        password = (string)reader["password"],
                        verify = (string)reader["verify"],
                    };
                }

                return user;
            }
            catch (Exception)
            {
                return null;
            }
            finally { cmd.Connection.Close(); }
        }

        public static User InsertUser(User user)
        {
            SqlCommand comm = new SqlCommand(
               " INSERT INTO users(email, phone, username, image, password, verify)" +
               " VALUES(@EmailPar, @PhonePar, @UnPar, @ImagePar, @PassPar, @VerifyPar);" +
               " SELECT SCOPE_IDENTITY() as _id;", con);

            comm.Parameters.Add(new SqlParameter("EmailPar", user.email));
            comm.Parameters.Add(new SqlParameter("PhonePar", user.phone));
            comm.Parameters.Add(new SqlParameter("UnPar", user.username));
            comm.Parameters.Add(new SqlParameter("ImagePar", user.image));
            comm.Parameters.Add(new SqlParameter("PassPar", user.password));
            comm.Parameters.Add(new SqlParameter("VerifyPar", user.verify));

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
                    user._id = res;
                    return user;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
            finally { con.Close(); }
        }

        public static User CheckEmail(string email)
        {
            string query = $"SELECT * FROM users WHERE email = @Email";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Email", email);

            try
            {
                cmd.Connection.Open();

                User user = null;
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    user = new User()
                    {
                        _id = (int)reader["_id"],
                        email = (string)reader["email"],
                        phone = (string)reader["phone"],
                        username = (string)reader["username"],
                        image = (string)reader["image"],
                        password = (string)reader["password"],
                        verify = (string)reader["verify"],
                    };
                }

                return user;
            }
            catch (Exception)
            {
                return null;
            }
            finally { cmd.Connection.Close(); }

        }

        public static User Login(string email, string password)
        {
            string query = $"SELECT * FROM users WHERE email='{email}' AND password='{password}'";
            SqlCommand cmd = new SqlCommand(query, con);

            try
            {
                cmd.Connection.Open();

                User user = null;
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    user = (new User()
                    {
                        _id = (int)reader["_id"],
                        email = (string)reader["email"],
                        phone = (string)reader["phone"],
                        username = (string)reader["username"],
                        image = (string)reader["image"],
                        password = (string)reader["password"],
                        verify = (string)reader["verify"],
                    });
                }

                return user;
            }
            catch (Exception)
            {
                return null;
            }
            finally { cmd.Connection.Close(); }
        }

        public static bool DeleteUser(int id)
        {
            SqlCommand comm = new SqlCommand($"DELETE FROM users WHERE _id=" + id, con);

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
