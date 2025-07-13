# Email Verification System Guide

## 🔐 **Email Verification with Registration**

The platform now includes automatic email verification during user registration for enhanced security and user experience.

---

## 📧 **Registration Flow with Email Verification**

### 🎯 **New Registration Process:**

#### 1. **User Registration**

```
POST /api/auth/student/register
POST /api/auth/instructor/register
POST /api/auth/admin/register
```

**Request:**

```json
{
    "email": "user@example.com",
    "password": "password123",
    "firstName": "John",
    "lastName": "Doe"
}
```

**Response:**

```json
{
    "message": "Registration successful. Please check your email for verification code.",
    "user": {
        "id": "user-id",
        "email": "user@example.com",
        "userName": "user@example.com",
        "firstName": "John",
        "lastName": "Doe",
        "userType": 1,
        "fullName": "John Doe"
    }
}
```

#### 2. **Email Verification**

```
POST /api/auth/student/verify-email
POST /api/auth/instructor/verify-email
POST /api/auth/admin/verify-email
```

**Request:**

```json
{
    "email": "user@example.com",
    "code": "123456"
}
```

**Response:**

```json
{
    "message": "Email verified successfully!",
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "user": {
        "id": "user-id",
        "email": "user@example.com",
        "userName": "user@example.com",
        "firstName": "John",
        "lastName": "Doe",
        "userType": 1,
        "fullName": "John Doe",
        "emailConfirmed": true
    }
}
```

#### 3. **Resend Verification Code**

```
POST /api/auth/student/resend-verification
POST /api/auth/instructor/resend-verification
POST /api/auth/admin/resend-verification
```

**Request:**

```json
{
    "email": "user@example.com"
}
```

**Response:**

```json
{
    "message": "Verification code sent to your email"
}
```

---

## 📋 **Complete API Endpoints**

### 📚 **Student Authentication**

#### Registration & Verification:

-   `POST /api/auth/student/register` - Register new student
-   `POST /api/auth/student/verify-email` - Verify email with code
-   `POST /api/auth/student/resend-verification` - Resend verification code
-   `POST /api/auth/student/login` - Login (after verification)
-   `GET /api/auth/student/me` - Get current student info

### 👨‍🏫 **Instructor Authentication**

#### Registration & Verification:

-   `POST /api/auth/instructor/register` - Register new instructor
-   `POST /api/auth/instructor/verify-email` - Verify email with code
-   `POST /api/auth/instructor/resend-verification` - Resend verification code
-   `POST /api/auth/instructor/login` - Login (after verification)
-   `GET /api/auth/instructor/me` - Get current instructor info

### 👨‍💼 **Admin Authentication**

#### Registration & Verification:

-   `POST /api/auth/admin/register` - Register new admin
-   `POST /api/auth/admin/verify-email` - Verify email with code
-   `POST /api/auth/admin/resend-verification` - Resend verification code
-   `POST /api/auth/admin/login` - Login (after verification)
-   `GET /api/auth/admin/me` - Get current admin info

---

## 🔄 **User Journey**

### ✅ **Complete Registration Flow:**

#### **Step 1: Registration**

1. User fills registration form
2. System validates input
3. System creates user account
4. System sends welcome email
5. System sends verification code
6. User receives success message

#### **Step 2: Email Verification**

1. User checks email for verification code
2. User enters 6-digit code
3. System validates code
4. System marks email as confirmed
5. System issues JWT token
6. User is logged in automatically

#### **Step 3: Login (After Verification)**

1. User enters email/password
2. System validates credentials
3. System checks email confirmation
4. System issues JWT token
5. User accesses protected resources

---

## 📧 **Email Templates**

### ✅ **Welcome Email:**

```html
<div style="font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;">
    <h2 style="color: #333;">Welcome to Courses Platform!</h2>
    <p>Hello John Doe,</p>
    <p>
        Thank you for joining Courses Platform. We're excited to have you on
        board!
    </p>
    <p>You can now:</p>
    <ul>
        <li>Browse our courses</li>
        <li>Enroll in your favorite courses</li>
        <li>Track your learning progress</li>
        <li>Connect with instructors</li>
    </ul>
    <p>
        Please check your email for a verification code to complete your
        registration.
    </p>
</div>
```

### ✅ **Verification Code Email:**

```html
<div style="font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;">
    <h2 style="color: #333;">Email Verification</h2>
    <p>Your verification code is:</p>
    <div
        style="background-color: #f4f4f4; padding: 20px; text-align: center; margin: 20px 0;"
    >
        <h1 style="color: #007bff; font-size: 32px; margin: 0;">123456</h1>
    </div>
    <p>This code will expire in 10 minutes.</p>
    <p>If you didn't request this code, please ignore this email.</p>
</div>
```

---

## 🔒 **Security Features**

### ✅ **Email Verification Benefits:**

-   **Prevents fake emails** - Users must verify their email
-   **Reduces spam accounts** - Only real users can register
-   **Better user experience** - Clear verification process
-   **Enhanced security** - Two-step registration process

### ✅ **Code Security:**

-   **6-digit numeric codes** - Easy to remember
-   **10-minute expiration** - Prevents abuse
-   **Single-use only** - Cannot be reused
-   **Cryptographically secure** - Random generation

### ✅ **User Type Validation:**

-   **Student verification** - Only students can verify through student endpoints
-   **Instructor verification** - Only instructors can verify through instructor endpoints
-   **Admin verification** - Only admins can verify through admin endpoints

---

## 🚀 **Frontend Implementation**

### ✅ **Registration Flow:**

```javascript
// Step 1: Register user
const registerUser = async (userData) => {
    const response = await fetch("/api/auth/student/register", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(userData),
    });
    return response.json();
};

// Step 2: Verify email
const verifyEmail = async (email, code) => {
    const response = await fetch("/api/auth/student/verify-email", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email, code }),
    });
    return response.json();
};

// Step 3: Resend verification
const resendVerification = async (email) => {
    const response = await fetch("/api/auth/student/resend-verification", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email }),
    });
    return response.json();
};
```

### ✅ **UI Flow:**

1. **Registration Form** → User enters details
2. **Success Message** → "Check your email for verification code"
3. **Verification Form** → User enters 6-digit code
4. **Success & Login** → User is automatically logged in
5. **Dashboard** → User accesses protected content

---

## 🧪 **Testing Scenarios**

### ✅ **Happy Path:**

1. Register new user
2. Check email for verification code
3. Enter code in verification form
4. Receive JWT token
5. Access protected endpoints

### ✅ **Error Scenarios:**

1. **Invalid Code** → "Invalid or expired verification code"
2. **Expired Code** → "Invalid or expired verification code"
3. **Wrong User Type** → "This verification is for students only"
4. **Already Verified** → "Email is already verified"
5. **User Not Found** → "User not found"

### ✅ **Resend Scenarios:**

1. **Code Expired** → Can resend new code
2. **Code Still Valid** → "Please wait before requesting a new code"
3. **Email Already Verified** → "Email is already verified"

---

## 📊 **Database Changes**

### ✅ **User Properties:**

```csharp
public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public UserType UserType { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool EmailConfirmed { get; set; } // Identity property
}
```

### ✅ **Email Confirmation Status:**

-   **EmailConfirmed = false** → User registered but not verified
-   **EmailConfirmed = true** → User verified and can login
-   **IsActive = false** → Account deactivated (cannot login)

---

## 🔧 **Configuration**

### ✅ **Email Settings:**

```json
{
    "EmailSettings": {
        "SmtpServer": "smtp.gmail.com",
        "SmtpPort": 587,
        "SmtpUsername": "your-email@gmail.com",
        "SmtpPassword": "your-app-password",
        "FromEmail": "your-email@gmail.com",
        "FromName": "Courses Platform"
    }
}
```

### ✅ **Verification Settings:**

```csharp
// In TwoFactorService.cs
private const int CodeExpirationMinutes = 10;
private const string CodePrefix = "2FA_";
```

---

## 🎯 **Benefits Achieved**

### ✅ **Security:**

-   Email verification prevents fake accounts
-   Two-step registration process
-   Type-specific verification endpoints

### ✅ **User Experience:**

-   Clear registration flow
-   Automatic welcome emails
-   Easy verification process
-   Automatic login after verification

### ✅ **Developer Experience:**

-   Clean API structure
-   Type-safe endpoints
-   Comprehensive error handling
-   Easy to test and maintain

The email verification system is now fully integrated with registration! 🎉
