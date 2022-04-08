// Author: Ryan Pinkney, Tanner Davis, Kevin Gutierrez, Jacob Poor
// This is our startup file for configuring the middleware and services

using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.ML.OnnxRuntime.Tensors;

namespace AuthLab2_RyanPinkney
{
    public class CrashData
    {
        // All the required attribute that are the inputs into our model

        [Required]
        public float city_OUTSIDE_CITY_LIMITS { get; set; }
        [Required]
        public float route_15 { get; set; }
        [Required]
        public float county_name_DAVIS { get; set; }
        [Required]
        public float county_name_SALT_LAKE { get; set; }
        [Required]
        public float county_name_UTAH { get; set; }
        [Required]
        public float single_vehicle_True { get; set; }
        [Required]
        public float teenage_driver_involved_True { get; set; }
        [Required]
        public float older_driver_involved_True { get; set; }
        [Required]
        public float distracted_driving_True { get; set; }
        [Required]
        public float night_dark_condition_True { get; set; }
        [Required]
        public float roadway_departure_True { get; set; }
        [Required]
        public float intersection_related_True { get; set; }
        [Required]
        public float main_road_name_Other { get; set; }


        // Tensor function
        // Code used for the model implementation
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
