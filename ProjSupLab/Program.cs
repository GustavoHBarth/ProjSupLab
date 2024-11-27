using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configura��es para arquivos grandes
builder.Services.Configure<FormOptions>(options =>
{
    // Define o limite m�ximo de tamanho para upload de arquivos
    options.MultipartBodyLengthLimit = 104857600; // 100MB (ajuste conforme necess�rio)
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();  // Habilita HTTP Strict Transport Security (HSTS) para produ��o
}

app.UseHttpsRedirection();
app.UseStaticFiles();  // Habilita a entrega de arquivos est�ticos (necess�rio para o upload)

app.UseRouting();
app.UseAuthorization();  // Autoriza usu�rios para controlar acesso

// Mapeia a rota do controlador e a a��o padr�o
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
