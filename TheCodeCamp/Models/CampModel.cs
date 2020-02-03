﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TheCodeCamp.Models
{
    public class CampModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Moniker { get; set; }
        [Required]
        public DateTime EventDate { get; set; } = DateTime.MinValue;
        [Required]
        [Range(1,30)]
        public int Length { get; set; } = 1;

        //Include data related to Talks
        public ICollection<TalkModel> Talks { get; set; }

        //Associate related data to the model
        public string VenueName { get; set; }
        public string LocationAddress1 { get; set; }
        public string LocationAddress2 { get; set; }
        public string LocationAddress3 { get; set; }
        public string LocationCityTown { get; set; }
        public string LocationStateProvince { get; set; }
        public string LocationPostalCode { get; set; }
        public string LocationCountry { get; set; }

        
    }
}