using MicroService_SnakeState_NicolasThomas.Data.Repositories;

namespace MicroService_SnakeState_NicolasThomas
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddScoped<ISnakeStateRepository, APISnakeStateRepository>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
