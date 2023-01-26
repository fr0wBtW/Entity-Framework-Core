using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StudentSystem.Data.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        [Required]
        [MaxLength(80)]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public decimal Price { get; set; }
        public ICollection<Resource> Resources { get; set;}
        public ICollection<Homework> Homeworks { get; set; }
        public ICollection<StudentCourse> Students { get; set; }
    }
}
