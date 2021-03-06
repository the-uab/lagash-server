﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wolf.Lagash.Entities.helper.editorial
{
    public class Editorial : Base
    {
        [Key]
        [StringLength(36)]
        public string _id { get; set; }

        [StringLength(100)]
        public string name { get; set; }

        [StringLength(100)]
        public string city { get; set; }
        
        [StringLength(100)]
        public string country { get; set; }

        [StringLength(50)]
        public string image { get; set; }

        [NotMapped]
        public EditorialMap map { get; set; }
    }
}
