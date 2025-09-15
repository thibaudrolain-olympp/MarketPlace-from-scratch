using Marketplace.Controllers;
using Marketplace.Repositories;
using Marketplace.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using System.Runtime.CompilerServices;

namespace Marketplace
{
    public static class Middleware
    {

        public static IServiceCollection ServiceDescriptors(this IServiceCollection services)
        {
            services.AddScoped<BearerController>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IEmailSenderService, EmailSenderService>();

            return services;
        }

        public static IServiceCollection AuthenticationDescriptors(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
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
                            System.Text.Encoding.UTF8.GetBytes(configuration.GetSection("JwtSettings")["Key"])),
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

            // Définition des politiques d'autorisation
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "api1");
                });

                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
            });

            return services;
        }

        public static IServiceCollection IdentityDescriptors(this IServiceCollection services)
        {
            services.AddIdentityApiEndpoints<IdentityUser>(options =>
            {
                // Config éventuelle des mots de passe / verrous / etc.
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
            }).AddRoles<IdentityRole>().AddEntityFrameworkStores<MarketplaceDbContext>().AddDefaultTokenProviders();

            return services;

        }

        public static IServiceCollection SwaggerDescriptors(this IServiceCollection services)
        {
            services.AddSwaggerGen(swag =>
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

            return services;
        }
    }
}
