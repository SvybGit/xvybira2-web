namespace Services;
using Blazored.LocalStorage;
using Models;

public class TodoService(ILocalStorageService localStorage)
{
    private const string StorageKey = "todoItemDataList";
    private List<TodoItemData> _todoItems = [];
    public event Action? OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();
    public async Task InitializeAsync()
    {
        _todoItems = await localStorage.GetItemAsync<List<TodoItemData>>(StorageKey) ?? [];
        NotifyStateChanged();
    }

    public List<TodoItemData> GetAll() => _todoItems;

    private int GetMaxiId()
    {
        TodoItemData? item = GetAll().MaxBy(i => i.Id);
        return item?.Id ?? 0;
    }

    public async Task CreateNewItem(string title, string description)
    {
        TodoItemData newItem = new TodoItemData(GetMaxiId() + 1, title, description);
        await AddTodoAsync(newItem);
    }

    public async Task AddTodoAsync(TodoItemData item)
    { 
        _todoItems.Add(item); 
        await SaveChangesAsync();
    }
    public async Task EditTodoAsync(TodoItemData item, string newTitle, string newDescription)
    {
        var index = _todoItems.FindIndex(t => t.Id == item.Id);
        if (index == -1) throw new KeyNotFoundException("Todo item not found.");

        _todoItems[index].Title = newTitle;
        _todoItems[index].Description = newDescription;
        await SaveChangesAsync();
    }
    public async Task DeleteTodoAsync(TodoItemData item)
    {
        _todoItems.RemoveAll(t => t.Id == item.Id);
        await SaveChangesAsync();
    }

    private async Task SaveChangesAsync()
    {
        await localStorage.SetItemAsync(StorageKey, _todoItems);
        NotifyStateChanged();
    }
}