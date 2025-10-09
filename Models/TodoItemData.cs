using BlazorBootstrap;

namespace Models
{
    public class TodoItemData(int id, string title, string description = "")
    {
        public string Description { get; set; } = description;
        public string Title { get; set; } = title;
        public int Id { get; set; } = id;
    }
}
