namespace ToDoItem.Models
{
    public class Todo
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public bool IsDone { get; set; }
        public DateTime DueDate { get; set; } = DateTime.Today;
    }
}
