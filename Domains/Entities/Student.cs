using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoWeb.Domains.Entities
{
    [Table("Students")]
    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }


        [MaxLength(255)]
        public string? FirstName { get; set; }

        [Column("Surname")]
        [StringLength(255)]
        public string? LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        //[MaxLength(255)]
        //public byte[] Image { get; set; }

        [ConcurrencyCheck]
        public decimal Balance { get; set; }
        //[Timestamp]
        //public byte[] RowVersion { get; set; }

        public string Address1 { get; set; }
        //public string Address2 { get; set; }

        public int Age { get; set; }

        [ForeignKey("School")]
        public int SId { get; set; }

        public virtual School School { get; set; }

        public virtual ICollection<CourseStudent> CourseStudents { get; set; }
    }
}
