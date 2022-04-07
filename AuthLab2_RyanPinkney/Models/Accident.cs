using System;
using System.ComponentModel.DataAnnotations;

namespace AuthLab2_RyanPinkney.Models
{
    public class Accident
    {
        // class for each accident model
        [Key]
        [Required]
        public int crash_id { get; set; }


        public string datetime { get; set; }

        public string route { get; set; }

        public decimal milepoint { get; set; }

        public decimal lat_utm_y { get; set; }

        public decimal long_utm_x { get; set; }

        public string main_road_name { get; set; }

        public string city { get; set; }

        public string county_name { get; set; }

        public int crash_severity_id { get; set; }

        // length specified to help limit them from entering something other than 'true' or 'false'
    
        public bool work_zone_related_True { get; set; }


        public bool pedestrian_involved_True { get; set; }

    
        public bool bicyclist_involved_True { get; set; }

        public bool motorcycle_involved_True { get; set; }

 
        public bool improper_restraint_True { get; set; }

       
        public bool unrestrained_True { get; set; }

    
        public bool dui_true { get; set; }

   
        public bool intersection_related_True { get; set; }

  
        public bool wild_animal_related_True { get; set; }


        public bool domestic_animal_related_True { get; set; }


        public bool overturn_rollover_True { get; set; }

        public bool commercial_motor_veh_involved_True { get; set; }

      
        public bool teenage_driver_involved_True { get; set; }

    
        public bool older_driver_involved_True { get; set; }


        public bool night_dark_condition_True { get; set; }

        public bool single_vehicle_True { get; set; }

       
        public bool distracted_driving_True { get; set; }


        public bool drowsy_driving_True { get; set; }

        
        public bool roadway_departure_True { get; set; }

    }
}
