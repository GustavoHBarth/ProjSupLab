using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configurações para arquivos grandes
builder.Services.Configure<FormOptions>(options =>
{
    // Define o limite máximo de tamanho para upload de arquivos
    options.MultipartBodyLengthLimit = 104857600; // 100MB (ajuste conforme necessário)
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();  // Habilita HTTP Strict Transport Security (HSTS) para produção
}

app.UseHttpsRedirection();
app.UseStaticFiles();  // Habilita a entrega de arquivos estáticos (necessário para o upload)

app.UseRouting();
app.UseAuthorization();  // Autoriza usuários para controlar acesso

// Mapeia a rota do controlador e a ação padrão
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
