using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

// 2. Database context (SQLite)
// NOTE: specify the migrations assembly so EF knows where migrations live
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqliteOptions =>
        {
            sqliteOptions.MigrationsAssembly("Persistence");
        }));

// 3. JWT Authentication (combined events + validation)
var jwtSection = builder.Configuration.GetSection("Jwt");
Console.WriteLine("🔑 JWT Key: " + jwtSection["Key"]);
Console.WriteLine("🔑 JWT Key Length: " + jwtSection["Key"]?.Length);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // 3a. Event handlers for custom 401/403 messages
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                context.NoResult();
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return context.Response.WriteAsync("Invalid token");
            },
            OnChallenge = context =>
            {
                if (string.IsNullOrWhiteSpace(context.Request.Headers["Authorization"]))
                {
                    context.HandleResponse();
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return context.Response.WriteAsync("Token not found");
                }
                return Task.CompletedTask;
            },
            OnForbidden = context =>
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return context.Response.WriteAsync("You are not authorized to perform this action");
            }
        };

        // 3b. How to validate incoming JWTs
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidAudience = jwtSection["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]!))
        };
    });

builder.Services.AddAuthorization();

// 4. Swagger / OpenAPI (security definitions once)
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your token"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// 5. AutoMapper, FluentValidation, MediatR
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddValidatorsFromAssemblyContaining<CreateAddressCommandValidator>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateAddressCommand>());

// 6. Scoped services & Identity
builder.Services.AddScoped<Seed>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// 7. CORS & pipeline
app.UseCors("AllowAll");

//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// 8. Seeding & Migrations
using (var scope = app.Services.CreateScope())
{
    var servicesProvider = scope.ServiceProvider;
    var db = servicesProvider.GetRequiredService<AppDbContext>();
    var hasher = servicesProvider.GetRequiredService<IPasswordHasher<User>>();

    try
    {
        db.Database.Migrate();

        // Only seed if no users exist
        if (!db.Users.Any())
        {
            var admin = new User { UserName = "admin", Role = "Admin" };
            admin.PasswordHash = hasher.HashPassword(admin, "admin");
            db.Users.Add(admin);

            var testUser = new User { UserName = "testuser" };
            testUser.PasswordHash = hasher.HashPassword(testUser, "Pa$$w0rd");
            db.Users.Add(testUser);

            db.SaveChanges();
        }

        // Only seed addresses if none exist
        if (!db.Addresses.Any())
        {
            var seed = servicesProvider.GetRequiredService<Seed>();
            db.AddRange(seed.GenerateFakeAddresses(100));
            db.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database migration or seeding failed: {ex.Message}");
    }
}

app.Run();