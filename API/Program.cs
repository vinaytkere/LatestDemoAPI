using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Define CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure()
    ));

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
                return context.Response.WriteAsync("You do not have access to this resource");
            }
        };
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
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddValidatorsFromAssemblyContaining<CreateAddressCommandValidator>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateAddressCommand>());

// Add services to the container.



builder.Services.AddScoped<Seed>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

var jwtSection = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
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

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 👇 Add this before UseAuthorization()
app.UseCors("AllowAll");
var scopeFactory = app.Services.GetService<IServiceScopeFactory>();
var scope = scopeFactory.CreateScope();
var services = app.Services;
var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();


var hasher = new PasswordHasher<User>();
var admin = new User { UserName = "admin", Role = "Admin" };
admin.PasswordHash = hasher.HashPassword(admin, "admin");
db.Users.Add(admin);

try
{

    // Apply pending migrations automatically (creates DB if not exists)
    db.Database.Migrate();

    // Optional: Seed data if DB is empty
    if (!db.Addresses.Any())
    {
        //var seed = services.GetRequiredService<Seed>();
        //var fakeAddresses = seed.GenerateFakeAddresses(10);
        //db.AddRange(fakeAddresses);
        //db.SaveChanges();
        var seed = services.GetRequiredService<Seed>();
        db.Addresses.RemoveRange(db.Addresses);
        db.AddRange(seed.GenerateFakeAddresses(100));
        db.SaveChanges();
    }

    if (!db.Users.Any())
    {
        var hasher1 = services.GetRequiredService<IPasswordHasher<User>>();
        var user = new User { UserName = "testuser" };
        user.PasswordHash = hasher1.HashPassword(user, "Pa$$w0rd");
        db.Users.Add(user);
        db.SaveChanges();
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Database migration or seeding failed: {ex.Message}");
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();