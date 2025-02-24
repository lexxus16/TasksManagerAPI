namespace TasksManagerAPI.Models
{
    public class TasksModel
    {
        public int ID { get; set; }          // Primary Key
        public string Title { get; set; }    // Task title
        public string Description { get; set; } // Task description
        public DateTime DueDate { get; set; } // Task due date
        public string Priority { get; set; }  // Priority: low, medium, high, urgent
        public string Status { get; set; }    // Status: Pending, In Progress, Completed
        public int UserID { get; set; }       // User assigned to the task (Foreign Key)
    }
}
