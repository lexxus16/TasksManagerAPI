using TasksManagerAPI.Models;

namespace TasksManagerAPI.Services
{
    public interface ITasksServices
    {
        IEnumerable<TasksModel> GetAllTasks();
        TasksModel GetTaskById(int id);
        TasksModel AddTask(TasksModel task);
        TasksModel UpdateTask(int id, TasksModel task);
        void DeleteTask(int id);


    }
}
