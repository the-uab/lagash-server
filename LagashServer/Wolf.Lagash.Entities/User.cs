﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Wolf.Lagash.Entities
{
    public class User
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int _id { get; set; }

        [Required]
        [StringLength(100)]
        public string name { get; set; }

        [Required]
        [StringLength(50)]
        public string password { get; set; }

        [Required]
        [StringLength(15)]
        public string cel { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [StringLength(100)]
        //[Required(ErrorMessage = "Title cannot be empty")]
        public string email { get; set; }

        [Required]
        [StringLength(20)]
        public string role { get; set; }

        //[Required]
        //[StringLength(20)]
        //public string role2 { get; set; }

        [NotMapped]
        //[ScaffoldColumn(false)] TODO: Does not work
        public Token token { get; set; }
    }

    public class Token
    {
        public string session_id { get; set; }
    }

    public class Login
    {
        [Required(ErrorMessage = "El password no puede ser vacio")]
        public string password { get; set; }

        [Required(ErrorMessage = "El email no puede ser vacio")]
        public string email { get; set; }
    }

}