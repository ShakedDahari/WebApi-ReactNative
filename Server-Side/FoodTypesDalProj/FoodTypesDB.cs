using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodTypesDalProj
{
    public static class FoodTypesDB
    {
        static string conStr = ConfigurationManager.ConnectionStrings["serverDB"].ConnectionString;
        static SqlConnection con = new SqlConnection(conStr);

        public static FoodType[] GetAllFoodTypes()
        {
            string query = $"SELECT * FROM foodTypes";
            SqlCommand cmd = new SqlCommand(query, con);

            try
            {
                cmd.Connection.Open();
                List<FoodType> foodTypes = new List<FoodType>();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    foodTypes.Add(new FoodType()
                    {
                        _id = (int)reader["_id"],
                        name = (string)reader["name"],
                        image = (string)reader["image"],
                    });
                }

                return foodTypes.ToArray();
            }
            catch (Exception)
            {
                return null;
            }
            finally { cmd.Connection.Close(); }
        }
    }
}
