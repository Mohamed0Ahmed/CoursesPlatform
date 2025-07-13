# Courses Platform API - Authentication Documentation

## 🔐 **Separated Authentication System**

Each user type now has its own dedicated authentication endpoints for better security and user experience.

---

## 📚 **Student Authentication**

### Base URL: `/api/auth/student`

#### 1. Student Login

**POST** `/api/auth/student/login`

Request:

```json
{
    "email": "student@example.com",
    "password": "password123"
}
```

Response:

```json
{
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "user": {
        "id": "student-id",
        "email": "student@example.com",
        "userName": "student@example.com",
        "firstName": "Ahmed",
        "lastName": "Ali",
        "userType": 1,
        "fullName": "Ahmed Ali"
    }
}
```

#### 2. Student Registration

**POST** `/api/auth/student/register`

Request:

```json
{
    "email": "newstudent@example.com",
    "password": "password123",
    "firstName": "Sara",
    "lastName": "Mohamed"
}
```

Response:

```json
{
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "user": {
        "id": "new-student-id",
        "email": "newstudent@example.com",
        "userName": "newstudent@example.com",
        "firstName": "Sara",
        "lastName": "Mohamed",
        "userType": 1,
        "fullName": "Sara Mohamed"
    }
}
```

#### 3. Get Current Student

**GET** `/api/auth/student/me`

Headers:

```
Authorization: Bearer <student-jwt-token>
```

Response:

```json
{
    "id": "student-id",
    "email": "student@example.com",
    "userName": "student@example.com",
    "firstName": "Ahmed",
    "lastName": "Ali",
    "userType": 1,
    "fullName": "Ahmed Ali",
    "isActive": true,
    "createdAt": "2024-01-01T00:00:00Z"
}
```

---

## 👨‍🏫 **Instructor Authentication**

### Base URL: `/api/auth/instructor`

#### 1. Instructor Login

**POST** `/api/auth/instructor/login`

Request:

```json
{
    "email": "instructor@example.com",
    "password": "password123"
}
```

Response:

```json
{
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "user": {
        "id": "instructor-id",
        "email": "instructor@example.com",
        "userName": "instructor@example.com",
        "firstName": "Dr. Omar",
        "lastName": "Hassan",
        "userType": 2,
        "fullName": "Dr. Omar Hassan"
    }
}
```

#### 2. Instructor Registration

**POST** `/api/auth/instructor/register`

Request:

```json
{
    "email": "newinstructor@example.com",
    "password": "password123",
    "firstName": "Dr. Fatima",
    "lastName": "Ahmed",
    "bio": "Expert in Computer Science with 10 years experience",
    "specialization": "Programming, Algorithms, Data Structures"
}
```

Response:

```json
{
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "user": {
        "id": "new-instructor-id",
        "email": "newinstructor@example.com",
        "userName": "newinstructor@example.com",
        "firstName": "Dr. Fatima",
        "lastName": "Ahmed",
        "userType": 2,
        "fullName": "Dr. Fatima Ahmed"
    }
}
```

#### 3. Get Current Instructor

**GET** `/api/auth/instructor/me`

Headers:

```
Authorization: Bearer <instructor-jwt-token>
```

Response:

```json
{
    "id": "instructor-id",
    "email": "instructor@example.com",
    "userName": "instructor@example.com",
    "firstName": "Dr. Omar",
    "lastName": "Hassan",
    "userType": 2,
    "fullName": "Dr. Omar Hassan",
    "isActive": true,
    "createdAt": "2024-01-01T00:00:00Z"
}
```

---

## 👨‍💼 **Admin Authentication**

### Base URL: `/api/auth/admin`

#### 1. Admin Login

**POST** `/api/auth/admin/login`

Request:

```json
{
    "email": "admin@example.com",
    "password": "password123"
}
```

Response:

```json
{
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "user": {
        "id": "admin-id",
        "email": "admin@example.com",
        "userName": "admin@example.com",
        "firstName": "Admin",
        "lastName": "User",
        "userType": 3,
        "fullName": "Admin User"
    }
}
```

#### 2. Admin Registration

**POST** `/api/auth/admin/register`

Request:

```json
{
    "email": "newadmin@example.com",
    "password": "password123",
    "firstName": "System",
    "lastName": "Administrator",
    "adminCode": "ADMIN2024" // Optional
}
```

Response:

```json
{
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "user": {
        "id": "new-admin-id",
        "email": "newadmin@example.com",
        "userName": "newadmin@example.com",
        "firstName": "System",
        "lastName": "Administrator",
        "userType": 3,
        "fullName": "System Administrator"
    }
}
```

#### 3. Get Current Admin

**GET** `/api/auth/admin/me`

Headers:

```
Authorization: Bearer <admin-jwt-token>
```

Response:

```json
{
    "id": "admin-id",
    "email": "admin@example.com",
    "userName": "admin@example.com",
    "firstName": "Admin",
    "lastName": "User",
    "userType": 3,
    "fullName": "Admin User",
    "isActive": true,
    "createdAt": "2024-01-01T00:00:00Z"
}
```

---

## 🔒 **Security Features**

### ✅ **Type-Specific Authentication**

-   Students can only login through `/api/auth/student/login`
-   Instructors can only login through `/api/auth/instructor/login`
-   Admins can only login through `/api/auth/admin/login`

### ✅ **Automatic User Type Assignment**

-   Student registration → `UserType.Student`
-   Instructor registration → `UserType.Instructor`
-   Admin registration → `UserType.Admin`

### ✅ **Endpoint Protection**

-   Each `/me` endpoint verifies the correct user type
-   Wrong user type → `403 Forbidden`

### ✅ **Error Messages**

-   Clear error messages for wrong authentication endpoints
-   "This login is for students only"
-   "This login is for instructors only"
-   "This login is for administrators only"

---

## 🚀 **Usage Examples**

### Frontend Implementation

```javascript
// Student Login
const studentLogin = async (email, password) => {
    const response = await fetch("/api/auth/student/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email, password }),
    });
    return response.json();
};

// Instructor Login
const instructorLogin = async (email, password) => {
    const response = await fetch("/api/auth/instructor/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email, password }),
    });
    return response.json();
};

// Admin Login
const adminLogin = async (email, password) => {
    const response = await fetch("/api/auth/admin/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email, password }),
    });
    return response.json();
};
```

### cURL Examples

```bash
# Student Login
curl -X POST "https://localhost:7001/api/auth/student/login" \
  -H "Content-Type: application/json" \
  -d '{"email": "student@example.com", "password": "password123"}'

# Instructor Login
curl -X POST "https://localhost:7001/api/auth/instructor/login" \
  -H "Content-Type: application/json" \
  -d '{"email": "instructor@example.com", "password": "password123"}'

# Admin Login
curl -X POST "https://localhost:7001/api/auth/admin/login" \
  -H "Content-Type: application/json" \
  -d '{"email": "admin@example.com", "password": "password123"}'
```

---

## 📊 **User Types**

| Type       | Value | Description               |
| ---------- | ----- | ------------------------- |
| Student    | 1     | Regular student user      |
| Instructor | 2     | Course instructor/teacher |
| Admin      | 3     | System administrator      |

---

## 🔧 **Testing with Swagger**

1. Start the application
2. Navigate to `/swagger`
3. Test each authentication endpoint:
    - `/api/auth/student/login`
    - `/api/auth/instructor/login`
    - `/api/auth/admin/login`
4. Use the "Authorize" button with the returned token
5. Test the `/me` endpoints for each user type
