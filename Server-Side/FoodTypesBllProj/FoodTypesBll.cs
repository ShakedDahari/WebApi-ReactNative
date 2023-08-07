using FoodTypesDalProj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodTypesBllProj
{
    public static class FoodTypesBll
    {
        public static FoodType[] GetAllFoodTypes()
        {
            FoodType[] foodTypes = null;
            foodTypes = FoodTypesDB.GetAllFoodTypes();
            return foodTypes;
        }
    }
}
