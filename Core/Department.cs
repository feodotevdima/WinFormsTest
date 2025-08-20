﻿using System.ComponentModel.DataAnnotations;

namespace Core
{
    public class Department
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
