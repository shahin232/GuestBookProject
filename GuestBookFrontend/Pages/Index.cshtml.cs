using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Net.Http.Json;

namespace GuestBookFrontend.Pages;

public class IndexModel : PageModel
{
    private readonly HttpClient _httpClient;
    
    
    private string BackendUrl => Environment.GetEnvironmentVariable("BACKEND_URL") ?? "http://localhost:5000/api/messages";

    public List<MessageItem> Messages { get; set; } = new();

    [BindProperty]
    public string NewMessageText { get; set; } = string.Empty;

    public IndexModel(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    public async Task OnGetAsync()
    {
        
        var response = await _httpClient.GetAsync(BackendUrl);
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            Messages = JsonSerializer.Deserialize<List<MessageItem>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        
        if (!string.IsNullOrWhiteSpace(NewMessageText))
        {
            var newMessage = new MessageItem { Text = NewMessageText };
            await _httpClient.PostAsJsonAsync(BackendUrl, newMessage);
        }
        return RedirectToPage();
    }
}

public class MessageItem
{
    public string Text { get; set; } = string.Empty;
}