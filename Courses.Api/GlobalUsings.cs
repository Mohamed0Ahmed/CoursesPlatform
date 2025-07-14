// =======================
// Extensions
// =======================
global using Courses.Api.Extensions;

// =======================
// Domain Models
// =======================
global using Courses.Domain.Identity;

// =======================
// Infrastructure
// =======================
global using Courses.Infrastructure.Data;

// =======================
// Shared Base Response & Enums
// =======================
global using Courses.Shared.BaseResponse;
global using Courses.Shared.Enum;

// =======================
// DTOs
// =======================
global using Courses.Shared.DTOs.AuthDtos;

// =======================
// ASP.NET Core & Identity
// =======================
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.AspNetCore.Authorization;

// =======================
// System
// =======================
global using System.Security.Claims;
global using System.Text;

// =======================
// Middleware
// =======================
global using Courses.Api.Middleware;

// =======================
// Dependency Injection
// =======================
global using Courses.Infrastructure;
global using Courses.Application;

// =======================
// Application Features - Authentication Commands
// =======================
global using Courses.Application.Features.Authentication.Commands.Register.Admins;
global using Courses.Application.Features.Authentication.Commands.Register.Students;
global using Courses.Application.Features.Authentication.Commands.Register.Instructors;
global using Courses.Application.Features.Authentication.Commands.ResendVerificationCode;
global using Courses.Application.Features.Authentication.Commands.VerifyEmail;

// =======================
// Application Features - Authentication Queries
// =======================
global using Courses.Application.Features.Authentication.Queries.GetCurrentUser;
global using Courses.Application.Features.Authentication.Queries.Login;

// =======================
// MediatR
// =======================
global using MediatR;