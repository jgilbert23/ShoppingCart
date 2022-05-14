using Polly;
using ShoppingCart;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient<IProductCatalogClient, ProductCatalogClient>()
    .AddTransientHttpErrorPolicy(p =>
        p.WaitAndRetryAsync(3, attempt => TimeSpan.FromMilliseconds(100*Math.Pow(2, attempt))));
builder.Services.AddControllers();
// builder.Services.Scan(selector =>
//     selector
//         .FromAssemblyOf<StartupBase>()
//         .AddClasses()
//         .AsImplementedInterfaces());

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
