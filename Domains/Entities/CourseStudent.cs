namespace ToDoWeb.Domains.Entities
{
    public class CourseStudent
    {
        public int CourseId { get; set; }

        public virtual Course Course { get; set; }
        public int StudentId { get; set; }

        public virtual Student Student { get; set; }
        public float Assignment { get; set; }
        public float Practical { get; set; }
        public float Final { get; set; }
    }
}
