# Clean Architecture Guide

## 🏗️ **Clean Architecture Structure**

### ✅ **الصحيح - Current Structure:**

```
CoursesPlatform/
├── Courses.Domain/           # Entities, Enums, Domain Logic
├── Courses.Application/       # Services, Interfaces, Business Logic
├── Courses.Infrastructure/   # Data Access, External Services
└── Courses.Api/             # Controllers, API Endpoints
```

### ❌ **الخطأ - Previous Structure:**

```
CoursesPlatform/
├── Courses.Domain/
├── Courses.Application/
├── Courses.Infrastructure/
└── Courses.Api/
    └── Services/            # ❌ Services في API Layer
```

---

## 📋 **Layer Responsibilities**

### 🎯 **Domain Layer (`Courses.Domain`)**

```csharp
// Entities
public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public UserType UserType { get; set; }
}

// Enums
public enum UserType
{
    Student = 1,
    Instructor = 2,
    Admin = 3
}
```

### 🔧 **Application Layer (`Courses.Application`)**

```csharp
// Services
public interface IEmailService
{
    Task<bool> SendEmailAsync(string to, string subject, string body);
}

public class EmailService : IEmailService
{
    // Implementation
}

// DTOs
public class LoginRequestDto
{
    public string Email { get; set; }
    public string Password { get; set; }
}
```

### 🗄️ **Infrastructure Layer (`Courses.Infrastructure`)**

```csharp
// Data Access
public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    // DbContext implementation
}

// External Services
public class ExternalEmailService : IEmailService
{
    // External email provider implementation
}
```

### 🌐 **API Layer (`Courses.Api`)**

```csharp
// Controllers
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IEmailService _emailService;

    public AuthController(IEmailService emailService)
    {
        _emailService = emailService;
    }
}
```

---

## 🔄 **Dependency Flow**

### ✅ **Correct Dependencies:**

```
API → Application → Domain
Infrastructure → Application → Domain
```

### ❌ **Wrong Dependencies:**

```
API → Infrastructure (Direct)
API → Domain (Direct)
```

---

## 🎯 **Why Services in Application Layer?**

### ✅ **Benefits:**

#### 1. **Separation of Concerns**

```csharp
// Application Layer - Business Logic
public class EmailService : IEmailService
{
    public async Task<bool> SendVerificationCodeAsync(string to, string code)
    {
        // Business logic for email verification
        var subject = "Email Verification";
        var body = CreateVerificationEmailTemplate(code);
        return await SendEmailAsync(to, subject, body);
    }
}
```

#### 2. **Testability**

```csharp
// Easy to mock in tests
public class AuthControllerTests
{
    [Fact]
    public async Task Login_WithValidCredentials_ReturnsToken()
    {
        // Arrange
        var mockEmailService = new Mock<IEmailService>();
        var controller = new AuthController(mockEmailService.Object);

        // Act & Assert
    }
}
```

#### 3. **Reusability**

```csharp
// Can be used by multiple layers
public class TwoFactorService
{
    private readonly IEmailService _emailService;

    public TwoFactorService(IEmailService emailService)
    {
        _emailService = emailService;
    }
}
```

#### 4. **Maintainability**

```csharp
// Easy to change implementation
public class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(IServiceCollection services)
    {
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ITwoFactorService, TwoFactorService>();
        return services;
    }
}
```

---

## 📁 **Current File Structure**

### ✅ **Application Layer (`Courses.Application/`)**

```
Services/
├── IEmailService.cs
├── EmailService.cs
├── ITwoFactorService.cs
└── TwoFactorService.cs

DependencyInjection.cs
```

### ✅ **API Layer (`Courses.Api/`)**

```
Controllers/
├── Auth/
│   ├── StudentAuthController.cs
│   ├── InstructorAuthController.cs
│   ├── AdminAuthController.cs
│   └── TwoFactorAuthController.cs
└── CoursesController.cs

Services/
└── JwtService.cs  # API-specific service
```

---

## 🔧 **Dependency Injection Setup**

### ✅ **Application Layer Registration:**

```csharp
// Courses.Application/DependencyInjection.cs
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ITwoFactorService, TwoFactorService>();
        services.AddMemoryCache();
        return services;
    }
}
```

### ✅ **API Layer Registration:**

```csharp
// Courses.Api/Program.cs
builder.Services.AddApplicationServices(); // Register Application services
builder.Services.AddScoped<IJwtService, JwtService>(); // API-specific service
```

---

## 🧪 **Testing Strategy**

### ✅ **Unit Testing Application Services:**

```csharp
[TestClass]
public class EmailServiceTests
{
    [TestMethod]
    public async Task SendVerificationCode_WithValidEmail_ReturnsTrue()
    {
        // Arrange
        var configuration = new Mock<IConfiguration>();
        var emailService = new EmailService(configuration.Object);

        // Act
        var result = await emailService.SendVerificationCodeAsync("test@example.com", "123456");

        // Assert
        Assert.IsTrue(result);
    }
}
```

### ✅ **Integration Testing API Controllers:**

```csharp
[TestClass]
public class AuthControllerTests
{
    [TestMethod]
    public async Task Login_WithValidCredentials_ReturnsToken()
    {
        // Arrange
        var mockEmailService = new Mock<IEmailService>();
        var mockUserManager = new Mock<UserManager<ApplicationUser>>();
        var controller = new AuthController(mockUserManager.Object, mockEmailService.Object);

        // Act & Assert
    }
}
```

---

## 🚀 **Migration Benefits**

### ✅ **Before (Wrong):**

-   Services mixed with API controllers
-   Hard to test business logic
-   Tight coupling between layers
-   Difficult to reuse services

### ✅ **After (Correct):**

-   Clear separation of concerns
-   Easy to test business logic
-   Loose coupling between layers
-   Highly reusable services
-   Follows Clean Architecture principles

---

## 📚 **Best Practices**

### ✅ **Do's:**

-   ✅ Put business logic in Application layer
-   ✅ Keep API layer thin (controllers only)
-   ✅ Use interfaces for dependency injection
-   ✅ Register services in appropriate layers
-   ✅ Follow dependency flow rules

### ❌ **Don'ts:**

-   ❌ Put business logic in API layer
-   ❌ Create tight coupling between layers
-   ❌ Skip interfaces for services
-   ❌ Mix concerns between layers

---

## 🎯 **Summary**

### ✅ **Current Structure is Correct:**

1. **Domain Layer** - Entities and domain logic
2. **Application Layer** - Services and business logic
3. **Infrastructure Layer** - Data access and external services
4. **API Layer** - Controllers and endpoints only

### ✅ **Benefits Achieved:**

-   🧪 **Better Testability**
-   🔄 **Higher Reusability**
-   🛠️ **Easier Maintenance**
-   📚 **Cleaner Code Structure**
-   🎯 **Clear Separation of Concerns**

The architecture now follows Clean Architecture principles correctly! 🎉
