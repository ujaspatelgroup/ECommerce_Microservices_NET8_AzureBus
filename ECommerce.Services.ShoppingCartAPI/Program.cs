using AutoMapper;
using ECommerce.Services.ShoppingCartAPI;
using ECommerce.Services.ShoppingCartAPI.Data;
using ECommerce.Services.ShoppingCartAPI.Extensions;
using ECommerce.Services.ShoppingCartAPI.Services.Coupon;
using ECommerce.Services.ShoppingCartAPI.Services.Product;
using ECommerce.Services.ShoppingCartAPI.Services.ShoppingCart;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
IMapper mapper = AutoMapperProfile.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.ProductInterServiceCall();
builder.CouponInterServiceCall();
builder.AddJwtAuthentication();

builder.Services.AddTransient<IApplicationContextDapper, ApplicationContextDapper>();
builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICouponService, CouponService>();
var app = builder.Build();

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
