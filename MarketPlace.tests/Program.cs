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

// Point d'entr�e principal de l'application Marketplace (.NET 8, C# 12)
var builder = WebApplication.CreateBuilder(args);

// ====================
// Configuration des services
// ====================

// Ajoute le support des contr�leurs MVC
builder.Services.AddControllers();

// Configuration de Swagger pour la documentation interactive de l'API et la gestion de la s�curit� JWT.
// Swagger permet de visualiser, tester et documenter les endpoints de l'API directement via une interface web.
builder.Services.AddSwaggerGen(swag =>
{
    // Ajoute la d�finition de s�curit� "Bearer" pour permettre l'authentification via JWT dans Swagger UI.
    swag.AddSecurityDefinition("Bearer",
        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            // Description affich�e dans l'interface Swagger pour expliquer comment fournir le token JWT.
            Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
            // Nom de l'en-t�te HTTP utilis� pour transmettre le token.
            Name = "Authorization",
            // Indique que le token doit �tre transmis dans l'en-t�te HTTP.
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            // Type de sch�ma de s�curit� utilis� (ici HTTP avec le sch�ma 'bearer').
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
            // Format attendu du token (JWT).
            BearerFormat = "JWT",
            // Sch�ma utilis� pour l'authentification.
            Scheme = "bearer"
        });

    // Ajoute une exigence de s�curit� pour que tous les endpoints n�cessitent le sch�ma "Bearer".
    swag.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
         {
            // R�f�rence � la d�finition de s�curit� "Bearer" ajout�e ci-dessus.
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            // Tableau vide : aucun scope sp�cifique requis, mais le token doit �tre pr�sent.
            new string[] { }
        }
    });
});

// Configuration de l'authentification JWT pour s�curiser l'API.
// Ici, on d�finit le sch�ma d'authentification par d�faut comme �tant JWT Bearer.
// Cela permet � l'application d'accepter et de valider les tokens JWT dans les requ�tes HTTP.
builder.Services.AddAuthentication(options =>
{
    // D�finit le sch�ma utilis� pour authentifier les utilisateurs
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    // D�finit le sch�ma utilis� pour les d�fis d'authentification (ex : acc�s refus�)
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    // D�finit le sch�ma par d�faut pour toutes les op�rations d'authentification
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    // Ajoute la configuration sp�cifique au sch�ma JWT Bearer
    .AddJwtBearer("Bearer", options =>
    {
        // Permet de sauvegarder le token dans le contexte d'authentification
        options.SaveToken = true;
        // D�sactive l'exigence de HTTPS pour les m�tadonn�es du token (utile en dev)
        options.RequireHttpsMetadata = false;
        // Param�tres de validation du token JWT
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            // Valide la cl� de signature du token
            ValidateIssuerSigningKey = true,
            // Cl� secr�te utilis�e pour signer et valider le token
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JwtSettings")["Key"])),
            // D�sactive la validation de l'�metteur du token
            ValidateIssuer = false,
            // D�sactive la validation du public cible du token
            ValidateAudience = false,
            // Active la validation de la dur�e de vie du token
            ValidateLifetime = true
        };
        // Autorit� (serveur d'identit�) attendue pour le token (optionnel)
        options.Authority = "https://localhost:5001";
        // Audience attendue dans le token (optionnel)
        options.Audience = "api1";
    });

// Configuration de l'identit� et de l'authentification JWT
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<MarketplaceDbContext>()
    .AddDefaultTokenProviders();

// Ajoute l'explorateur d'endpoints pour Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();

// D�finition des politiques d'autorisation
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "api1");
    });
});

// Politique suppl�mentaire pour les administrateurs
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

// Configuration de la redirection HTTPS
builder.Services.AddHttpsRedirection(options =>
{
    options.RedirectStatusCode = Microsoft.AspNetCore.Http.StatusCodes.Status308PermanentRedirect;
    options.HttpsPort = 5001; // Port HTTPS utilis�
});

// Ajout du logging
builder.Services.AddLogging();

// Configuration du contexte de base de donn�es avec SQL Server
builder.Services.AddDbContext<MarketplaceDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Injection de la configuration
builder.Services.Configure<IConfiguration>(builder.Configuration);

// Enregistrement des services m�tiers et des repositories
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

// Activation de Swagger en environnement de d�veloppement
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redirection HTTPS
app.UseHttpsRedirection();

// Activation de l'autorisation
app.UseAuthorization();

// Mapping des contr�leurs (API REST)
app.MapControllers();

// D�marrage de l'application
app.Run();