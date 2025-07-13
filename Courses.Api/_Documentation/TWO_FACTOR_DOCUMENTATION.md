# Two-Factor Authentication (2FA) Documentation

## 🔐 **Two-Factor Authentication System**

The platform now supports Two-Factor Authentication using email verification codes for enhanced security.

---

## 📧 **Email Configuration**

### SMTP Settings in `appsettings.json`:

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

### Gmail Setup Instructions:

1. **Enable 2-Step Verification** in your Google Account
2. **Generate App Password**:
    - Go to Google Account Settings
    - Security → 2-Step Verification → App passwords
    - Generate password for "Mail"
3. **Use the App Password** in `SmtpPassword`

---

## 🔑 **Two-Factor Authentication Endpoints**

### Base URL: `/api/auth/2fa`

#### 1. Send Verification Code

**POST** `/api/auth/2fa/send-code`

Request:

```json
{
    "email": "user@example.com"
}
```

Response:

```json
{
    "success": true,
    "message": "Verification code sent to your email"
}
```

#### 2. Verify Code

**POST** `/api/auth/2fa/verify-code`

Request:

```json
{
    "email": "user@example.com",
    "code": "123456"
}
```

Response:

```json
{
    "success": true,
    "message": "Code verified successfully",
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
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

#### 3. Two-Factor Login

**POST** `/api/auth/2fa/login`

Request:

```json
{
    "email": "user@example.com",
    "password": "password123",
    "verificationCode": "123456"
}
```

Response:

```json
{
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
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

#### 4. Resend Verification Code

**POST** `/api/auth/2fa/resend-code`

Request:

```json
{
    "email": "user@example.com"
}
```

Response:

```json
{
    "success": true,
    "message": "New verification code sent to your email"
}
```

---

## 🔒 **Security Features**

### ✅ **Verification Code Properties**

-   **6-digit numeric code**
-   **10-minute expiration**
-   **Single-use only** (removed after validation)
-   **Cryptographically secure** generation

### ✅ **Email Security**

-   **HTML formatted emails** with professional design
-   **Clear instructions** and expiration warnings
-   **Branded templates** for Courses Platform

### ✅ **Rate Limiting**

-   **Prevents spam** by checking code expiration
-   **Cooldown period** between resend requests
-   **User validation** before sending codes

### ✅ **Error Handling**

-   **Clear error messages** for all scenarios
-   **Account status validation** (active/inactive)
-   **Email delivery confirmation**

---

## 📱 **Frontend Implementation**

### Step 1: Send Verification Code

```javascript
const sendVerificationCode = async (email) => {
    const response = await fetch("/api/auth/2fa/send-code", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email }),
    });
    return response.json();
};
```

### Step 2: Verify Code

```javascript
const verifyCode = async (email, code) => {
    const response = await fetch("/api/auth/2fa/verify-code", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email, code }),
    });
    return response.json();
};
```

### Step 3: Two-Factor Login

```javascript
const twoFactorLogin = async (email, password, code) => {
    const response = await fetch("/api/auth/2fa/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
            email,
            password,
            verificationCode: code,
        }),
    });
    return response.json();
};
```

### Step 4: Resend Code

```javascript
const resendCode = async (email) => {
    const response = await fetch("/api/auth/2fa/resend-code", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email }),
    });
    return response.json();
};
```

---

## 🎯 **User Flow**

### **Registration Flow:**

1. User registers normally
2. System sends welcome email
3. User can optionally enable 2FA

### **Login Flow with 2FA:**

1. User enters email/password
2. System validates credentials
3. System sends verification code to email
4. User enters 6-digit code
5. System validates code and issues JWT token

### **Alternative Flow:**

1. User requests verification code
2. System sends code to email
3. User enters code
4. System validates and issues JWT token

---

## 📧 **Email Templates**

### Verification Code Email:

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

### Welcome Email:

```html
<div style="font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;">
    <h2 style="color: #333;">Welcome to Courses Platform!</h2>
    <p>Hello John,</p>
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
</div>
```

---

## 🔧 **Testing with Swagger**

1. **Configure Email Settings** in `appsettings.json`
2. **Start the application**
3. **Navigate to `/swagger`**
4. **Test 2FA endpoints:**
    - `POST /api/auth/2fa/send-code`
    - `POST /api/auth/2fa/verify-code`
    - `POST /api/auth/2fa/login`
    - `POST /api/auth/2fa/resend-code`

### Testing Steps:

1. Send verification code to your email
2. Check email for 6-digit code
3. Verify the code using the API
4. Test login with email, password, and code

---

## 🚨 **Error Scenarios**

### Invalid Email:

```json
{
    "success": false,
    "message": "User not found"
}
```

### Invalid Code:

```json
{
    "success": false,
    "message": "Invalid or expired verification code"
}
```

### Expired Code:

```json
{
    "success": false,
    "message": "Invalid or expired verification code"
}
```

### Account Deactivated:

```json
{
    "success": false,
    "message": "Account is deactivated"
}
```

### Email Send Failure:

```json
{
    "success": false,
    "message": "Failed to send verification code. Please try again."
}
```

---

## 📊 **Configuration Options**

### Code Expiration (in `TwoFactorService.cs`):

```csharp
private const int CodeExpirationMinutes = 10;
```

### Code Length (6 digits):

```csharp
return number.ToString("D6"); // Ensure 6 digits with leading zeros
```

### Cache Settings:

```csharp
var cacheOptions = new MemoryCacheEntryOptions
{
    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(CodeExpirationMinutes)
};
```

---

## 🔐 **Security Best Practices**

### ✅ **Implemented:**

-   Cryptographically secure code generation
-   Time-limited codes (10 minutes)
-   Single-use codes
-   Email validation
-   Rate limiting protection
-   Clear error messages

### 🔄 **Future Enhancements:**

-   SMS verification option
-   Authenticator app integration (TOTP)
-   Backup codes generation
-   Device remember option
-   Admin 2FA bypass option

---

## 📞 **Support**

For issues with Two-Factor Authentication:

1. Check email configuration in `appsettings.json`
2. Verify Gmail App Password setup
3. Test email delivery manually
4. Check application logs for errors
5. Ensure user account is active

The 2FA system is now fully operational and ready for production use! 🎉
