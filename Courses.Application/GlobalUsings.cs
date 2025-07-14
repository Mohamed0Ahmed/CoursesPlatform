
// =======================
// Microsoft Extensions
// =======================
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.Logging;

// =======================
// ASP.NET Core Identity
// =======================
global using Microsoft.AspNetCore.Identity;

// =======================
// Domain Models & Enums
// =======================
global using Courses.Domain.Identity;
global using Courses.Shared.Enum;

// =======================
// Shared Base Classes
// =======================
global using Courses.Shared.BaseModels;

// =======================
// Application Interfaces & Repositories
// =======================
global using Courses.Application.IRepo;

// =======================
// Abstractions
// =======================
global using Courses.Application.Abstraction.Jwt;
global using Courses.Application.Abstraction.Email;
global using Courses.Application.Abstraction.TwoFactor;

// =======================
// DTOs
// =======================
global using Courses.Shared.DTOs.AuthDtos;

// =======================
// Mapping
// =======================
global using Mapster;

// =======================
// MediatR
// =======================
global using MediatR;

// =======================
// System
// =======================
global using System.Linq.Expressions;
