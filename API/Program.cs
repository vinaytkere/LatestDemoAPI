using Application.Common.Behaviors;
using Application.Features.Address;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// 1. CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

// 2. Database context (SQL Server)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions =>
        {
            sqlOptions.MigrationsAssembly("Persistence");
        }));

// 3. JWT Authentication
var jwtSection = builder.Configuration.GetSection("Jwt");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
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

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidAudience = jwtSection["Audience"],
            IssuerSigningKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSection["Key"]!))
        };
    });

builder.Services.AddAuthorization();

// 4. Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by your JWT token"
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
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<AddressProfile>());
builder.Services.AddValidatorsFromAssemblyContaining<CreateAddressCommandValidator>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateAddressCommand>());
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// 6. Scoped services
builder.Services.AddScoped<Seed>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// 7. Middleware pipeline
app.UseCors("AllowAll");

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// 8. Database Migration & Seeding
using (var scope = app.Services.CreateScope())
{
    var servicesProvider = scope.ServiceProvider;
    var db = servicesProvider.GetRequiredService<AppDbContext>();
    var hasher = servicesProvider.GetRequiredService<IPasswordHasher<User>>();

    try
    {
        db.Database.Migrate();

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
