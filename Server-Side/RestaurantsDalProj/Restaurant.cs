using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantsDalProj
{
    public class Restaurant
    {
        public int _id { get; set; }

        public string name { get; set; }

        public string email { get; set; }

        public string phone { get; set; }

        public string location { get; set; }

        public string password { get; set; }

        public string verify { get; set; }

        public string image { get; set; }

        public string foodType { get; set; }

        public string approved { get; set; }

        public DateTime createdAt { get; set; }

        public string menu { get; set; }
    }
}
