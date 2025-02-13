builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register IDbContext as ApplicationDbContext
builder.Services.AddScoped<IDbContext, ApplicationDbContext>();

// Register repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
