using System.Net.Http.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

var FOOD_SERVICE_URL = "http://localhost:8004";
var GAME_ID = "partie_test";

var receivedEvents = new List<object>();

// --- Simuler Display Service ---
var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.MapPost("/display/on-food-eaten", async (HttpContext context) =>
{
    var evt = await context.Request.ReadFromJsonAsync<FoodEvent>();
    Console.WriteLine($"[Display] Événement reçu: {evt}");
    receivedEvents.Add(evt!);
    return Results.Json(new { status = "ok" });
});

var displayTask = app.RunAsync("http://0.0.0.0:8010");

// --- Test Food Service ---
using var client = new HttpClient();

var displayUrl = "http://localhost:8010/display/on-food-eaten";

// Subscribe
var subscribeResp = await client.PostAsJsonAsync($"{FOOD_SERVICE_URL}/food/{GAME_ID}/subscribe", new { route = displayUrl });
Console.WriteLine("[Test] Subscribe: " + await subscribeResp.Content.ReadAsStringAsync());

//  On-Move
var moveResp = await client.PostAsJsonAsync($"{FOOD_SERVICE_URL}/food/{GAME_ID}/on-move", new { x = 5, y = 5 });
Console.WriteLine("[Test] On-Move: " + await moveResp.Content.ReadAsStringAsync());

// Eat Food
var eatResp = await client.DeleteAsync($"{FOOD_SERVICE_URL}/food/{GAME_ID}");
Console.WriteLine("[Test] Eat Food: " + await eatResp.Content.ReadAsStringAsync());
Attendre notification
await Task.Delay(1000);
Console.WriteLine("[Test] Événements reçus par Display: " + System.Text.Json.JsonSerializer.Serialize(receivedEvents));

// Stop Display Service
await app.StopAsync();
Console.WriteLine("[Test] Display Service arrêté.");

// --- DTO ---
record FoodEvent(string eventName, string gameId, object food);
