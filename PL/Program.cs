using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ML;
using Newtonsoft.Json;
using PL.Data;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>() //-----Se añadio para que el identity builder cree una nueva entidad.
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

#region To Habilitate Session in NETCore
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
#endregion


var app = builder.Build();
#region Session

app.UseSession();

#endregion
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

#region Session 1 Object
//public static class SessionHelper
//{
//    public static void SetObjectInSession(this ISession session, string key, object value)
//    {
//        session.SetString(key, JsonConvert.SerializeObject(value));
//    }

//    public static T GetCustomObjectFromSession<T>(this ISession session, string key)
//    {
//        var value = session.GetString(key);
//        return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
//    }
//}
#endregion

#region Session 2 Object(List)
//public static class SessionHelper
//{
//    public static void SetObjectInSession(this ISession session, string key, object value)
//    {
//        session.SetString(key, JsonConvert.SerializeObject(value));
//    }

//    public static T GetCustomObjectFromSession<T>(this ISession session, string key)
//    {
//        var value = session.GetString(key);
//        return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
//    }
//}
#endregion

#region Session 1 List_Objects
//public static class CartExtensions
//{
//    private const string CartSessionKey = "AnansiCart";

//    public static List<VentaProducto> GetCart(this ISession session)
//    {
//        return session.SetObjectInSession<List<VentaProducto>>(CartSessionKey) ?? new List<VentaProducto>();
//    }

//    public static void SaveCart(this ISession session, List<VentaProducto> cart)
//    {
//        session.SetObjectInSession(CartSessionKey, cart);
//    }
//}
#endregion

#region IntentStripe
//StripeConfiguration.ApiKey = "pk_test_51Nn3GbA6P4ZFiH4hyk0KqDlinJS8PQrVx5f0ZvDoELI81hiH7YveE9F7jvJfyUBa47obBGXjcPvyIlW848A7Tx1v007sOIfB1q";
//var options = new PaymentIntentCreateOptions
//{
//    Amount
// = 1099,
//    Currency
// = "mxn",
//    AutomaticPaymentMethods
// = new PaymentIntentAutomaticPaymentMethodsOptions
// {
//     Enabled
// = true,
// },
//};
//var service = new PaymentIntentService();
//service.Create(options); 
#endregion

#region Session 3 https://learningprogramming.net/net/asp-net-core-mvc-5/use-session-in-asp-net-core-mvc-5/
public static class SessionHelper
{
    public static void SetObjectAsJson(this ISession session, string key, object value)
    {
        session.SetString(key, JsonConvert.SerializeObject(value));
    }

    public static T GetObjectFromJson<T>(this ISession session, string key)
    {
        var value = session.GetString(key);
        return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
    }
}
#endregion




