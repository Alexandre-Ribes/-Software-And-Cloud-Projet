using ScoreService.IServices;
using ScoreService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<INotificationService, NotificationService>();
builder.Services.AddHttpClient("FoodService", client =>
{
	client.BaseAddress = new Uri("http://foodservice/food/");
});

var app = builder.Build();

// Endpoint recevant les notifications depuis le microservice Food
app.MapPost("/notify", (FoodUpdatedEvent evt) =>
{
	Console.WriteLine($"Event re�u : GameId={evt.GameId}, FoodLocation={evt.FoodLocation}");
	return Results.Ok(new { message = "Notification re�ue", evt });
});

// Endpoint interrogeant FoodService
app.MapGet("/food/{gameId}", async (string gameId, IHttpClientFactory httpClientFactory) =>
{
	var client = httpClientFactory.CreateClient("FoodService");

	try
	{
		// Appel vers FoodService : GET /food/{gameId}
		var response = await client.GetAsync(gameId);

		if (!response.IsSuccessStatusCode)
		{
			return Results.NotFound(new { message = $"Food introuvable pour {gameId}" });
		}

		var foodValue = await response.Content.ReadFromJsonAsync<int>(); // Renvoi 1 si mang�, 0 sinon
		return Results.Ok(new { gameId, food = foodValue });
	}
	catch (Exception ex)
	{
		return Results.Problem($"Erreur lors de l'appel � FoodService : {ex.Message}");
	}
});

// Endpoint v�rifiant que le microservice tourne
app.MapGet("/", () => "Microservice ScoreService est en ligne");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


// -------------------------------
// Contrat de l��v�nement re�u
// -------------------------------
public class FoodUpdatedEvent
{
	public string Event { get; set; } = string.Empty;
	public string GameId { get; set; } = string.Empty;
	public Dictionary<string, int> FoodLocation { get; set; } = new Dictionary<string, int> { {"x", 0 }, {"y" , 0 } };
}