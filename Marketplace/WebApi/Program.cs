using Marketplace;
using Marketplace.Application.Automapper;
using Marketplace.WebApi;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

// Point d'entrée principal de l'application Marketplace (.NET 8, C# 12)
var builder = WebApplication.CreateBuilder(args);

// ====================
// Configuration des services
// ====================

// Ajoute le support des contrôleurs MVC
builder.Services.AddControllers();

// Configuration de Swagger pour la documentation interactive de l'API et la gestion de la sécurité JWT.
// Swagger permet de visualiser, tester et documenter les endpoints de l'API directement via une interface web.
builder.Services.SwaggerDescriptors();

// Configuration de l'authentification JWT pour sécuriser l'API.
// Ici, on définit le schéma d'authentification par défaut comme étant JWT Bearer.
// Cela permet à l'application d'accepter et de valider les tokens JWT dans les requêtes HTTP.
builder.Services.AuthenticationDescriptors(builder.Configuration);

// Configuration de la redirection HTTPS
builder.Services.AddHttpsRedirection(options =>
{
    options.RedirectStatusCode = Microsoft.AspNetCore.Http.StatusCodes.Status308PermanentRedirect;
    options.HttpsPort = 5001; // Port HTTPS utilisé
});

// Ajout du logging
builder.Services.AddLogging();

// Configuration du contexte de base de données avec SQL Server
builder.Services.AddDbContext<MarketplaceDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Injection de la configuration
builder.Services.Configure<IConfiguration>(builder.Configuration);

builder.Services.IdentityDescriptors();

// Ajoute l'explorateur d'endpoints pour Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();

// Enregistrement des services métiers et des repositories
builder.Services.ServiceDescriptors();

// Configuration d'AutoMapper avec le profil produit
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<ProductProfile>());
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<CartProfile>());

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200") // ton front
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Construction de l'application
var app = builder.Build();

app.UseStaticFiles();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    // Rôles par défaut
    string[] roles = new[] { "Admin", "User" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // Création d’un admin par défaut si inexistant
    var adminEmail = "admin@marketplace.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        var user = new IdentityUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, "Admin123!");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, "Admin");
        }
    }
}

// ====================
// Configuration du pipeline HTTP
// ====================

// Activation de Swagger en environnement de développement
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngular");


// Redirection HTTPS
app.UseHttpsRedirection();

app.UseAuthentication();

// Activation de l'autorisation
app.UseAuthorization();

app.MapIdentityApi<IdentityUser>();

// Mapping des contrôleurs (API REST)
app.MapControllers();

// Démarrage de l'application
app.Run();