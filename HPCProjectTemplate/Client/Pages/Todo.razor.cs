using HPCProjectTemplate.Shared;

namespace HPCProjectTemplate.Client.Pages;

public partial class Todo
{
    private string? newTodo = String.Empty;
    public List<TodoItem> todos { get; set; } = new List<TodoItem>()
    {
        new TodoItem() { Title = "Learn Blazor", IsDone = true },
        new TodoItem() { Title = "Learn ASP.NET Core", IsDone = false },
        new TodoItem() { Title = "Build Awesome Apps", IsDone = false }
    };


    public void AddTodo()
    {
        todos.Add(new TodoItem { Title = newTodo });
        newTodo = String.Empty;
    }
}
