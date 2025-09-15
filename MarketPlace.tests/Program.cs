using Marketplace;
using Marketplace.Automapper;
using Marketplace.Controllers;
using Marketplace.Repositories;
using Marketplace.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// Point d'entrée principal de l'application Marketplace (.NET 8, C# 12)
var builder = WebApplication.CreateBuilder(args);

// ====================
// Configuration des services
// ====================

// Ajoute le support des contrôleurs MVC
builder.Services.AddControllers();

// Configuration de Swagger pour la documentation interactive de l'API et la gestion de la sécurité JWT.
// Swagger permet de visualiser, tester et documenter les endpoints de l'API directement via une interface web.
builder.Services.AddSwaggerGen(swag =>
{
    // Ajoute la définition de sécurité "Bearer" pour permettre l'authentification via JWT dans Swagger UI.
    swag.AddSecurityDefinition("Bearer",
        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            // Description affichée dans l'interface Swagger pour expliquer comment fournir le token JWT.
            Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
            // Nom de l'en-tête HTTP utilisé pour transmettre le token.
            Name = "Authorization",
            // Indique que le token doit être transmis dans l'en-tête HTTP.
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            // Type de schéma de sécurité utilisé (ici HTTP avec le schéma 'bearer').
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
            // Format attendu du token (JWT).
            BearerFormat = "JWT",
            // Schéma utilisé pour l'authentification.
            Scheme = "bearer"
        });

    // Ajoute une exigence de sécurité pour que tous les endpoints nécessitent le schéma "Bearer".
    swag.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
         {
            // Référence à la définition de sécurité "Bearer" ajoutée ci-dessus.
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            // Tableau vide : aucun scope spécifique requis, mais le token doit être présent.
            new string[] { }
        }
    });
});

// Configuration de l'authentification JWT pour sécuriser l'API.
// Ici, on définit le schéma d'authentification par défaut comme étant JWT Bearer.
// Cela permet à l'application d'accepter et de valider les tokens JWT dans les requêtes HTTP.
builder.Services.AddAuthentication(options =>
{
    // Définit le schéma utilisé pour authentifier les utilisateurs
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    // Définit le schéma utilisé pour les défis d'authentification (ex : accès refusé)
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    // Définit le schéma par défaut pour toutes les opérations d'authentification
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    // Ajoute la configuration spécifique au schéma JWT Bearer
    .AddJwtBearer("Bearer", options =>
    {
        // Permet de sauvegarder le token dans le contexte d'authentification
        options.SaveToken = true;
        // Désactive l'exigence de HTTPS pour les métadonnées du token (utile en dev)
        options.RequireHttpsMetadata = false;
        // Paramètres de validation du token JWT
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            // Valide la clé de signature du token
            ValidateIssuerSigningKey = true,
            // Clé secrète utilisée pour signer et valider le token
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JwtSettings")["Key"])),
            // Désactive la validation de l'émetteur du token
            ValidateIssuer = false,
            // Désactive la validation du public cible du token
            ValidateAudience = false,
            // Active la validation de la durée de vie du token
            ValidateLifetime = true
        };
        // Autorité (serveur d'identité) attendue pour le token (optionnel)
        options.Authority = "https://localhost:5001";
        // Audience attendue dans le token (optionnel)
        options.Audience = "api1";
    });

// Configuration de l'identité et de l'authentification JWT
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<MarketplaceDbContext>()
    .AddDefaultTokenProviders();

// Ajoute l'explorateur d'endpoints pour Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();

// Définition des politiques d'autorisation
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "api1");
    });
});

// Politique supplémentaire pour les administrateurs
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

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

// Enregistrement des services métiers et des repositories
builder.Services.AddScoped<BearerController>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Configuration d'AutoMapper avec le profil produit
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<ProductProfile>());

// Construction de l'application
var app = builder.Build();

// ====================
// Configuration du pipeline HTTP
// ====================

// Activation de Swagger en environnement de développement
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redirection HTTPS
app.UseHttpsRedirection();

// Activation de l'autorisation
app.UseAuthorization();

// Mapping des contrôleurs (API REST)
app.MapControllers();

// Démarrage de l'application
app.Run();