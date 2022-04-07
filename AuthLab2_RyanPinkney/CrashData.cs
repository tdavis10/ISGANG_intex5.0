using System;
using Microsoft.ML.OnnxRuntime.Tensors;

namespace AuthLab2_RyanPinkney
{
    public class CrashData
    {
        public float city_OUTSIDE_CITY_LIMITS { get; set; }
        public float route_15 { get; set; }
        public float county_name_DAVIS { get; set; }
        public float county_name_SALT_LAKE { get; set; }
        public float county_name_UTAH { get; set; }
        public float single_vehicle_True { get; set; }
        public float teenage_driver_involved_True { get; set; }
        public float older_driver_involved_True { get; set; }
        public float distracted_driving_True { get; set; }
        public float night_dark_condition_True { get; set; }
        public float roadway_departure_True { get; set; }
        public float intersection_related_True { get; set; }
        public float main_road_name_Other { get; set; }

        public Tensor<float> AsTensor()
        {
            float[] data = new float[]
            {
           city_OUTSIDE_CITY_LIMITS, route_15, county_name_DAVIS,  county_name_SALT_LAKE, county_name_UTAH, single_vehicle_True, teenage_driver_involved_True,
            older_driver_involved_True, distracted_driving_True, night_dark_condition_True, roadway_departure_True, intersection_related_True, main_road_name_Other
            };
            int[] dimensions = new int[] { 1, 13};
            return new DenseTensor<float>(data, dimensions);
        }



    }
}
