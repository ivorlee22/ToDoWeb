namespace ToDoWeb.Domains.Interfaces
{
    public interface IDelete
    {
        public DateTime DeleteAt { get; set; }
        public int DeleteBy { get; set; }
    }
}
