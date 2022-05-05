using ShoppingCart;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient<IProductCatalogClient, ProductCatalogClient>();
builder.Services.AddControllers();
builder.Services.Scan(selector =>
    selector
        .FromAssemblyOf<StartupBase>()
        .AddClasses()
        .AsImplementedInterfaces());

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
