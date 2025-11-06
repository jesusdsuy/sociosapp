using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SociosApp.Services;
using SociosApp;
var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// HttpClient básico para llamadas (si lo necesitás)
builder.Services.AddScoped(sp => new HttpClient());

// 🔹 Leer la configuración Supabase del appsettings.json
var supabaseSettings = new SupabaseSettings();
builder.Configuration.GetSection("Supabase").Bind(supabaseSettings);


// 🔹 Registrar el objeto de configuración como Singleton
builder.Services.AddSingleton(supabaseSettings);
builder.Services.AddScoped<AuthService>();

// 🔹 Registrar el servicio Supabase
builder.Services.AddSingleton<SupabaseService>();
builder.Services.AddScoped<SessionService>();


//prueba***
//using var scope = builder.Services.BuildServiceProvider().CreateScope();
//var supabase = scope.ServiceProvider.GetRequiredService<SupabaseService>();
//var user = await supabase.GetUsuariosAsync();
//*****

await builder.Build().RunAsync();
