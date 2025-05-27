# Almostengr Common Utilities

This class library was created for common methods that I use in the applications that I build. 
These include, but are not limited to, methods and classes to make changes to entities, generic 
repository methods, and generic service classes.

## Example Injections

Below are some code examples to inject services into your application.


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register IDbContext as ApplicationDbContext
builder.Services.AddScoped<IDbContext, ApplicationDbContext>();

// Register repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
