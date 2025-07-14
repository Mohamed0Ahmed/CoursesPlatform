// Extensions
global using Courses.Api.Extensions;


// Domain Models
global using Courses.Domain.Identity;
// Infrastructure
global using Courses.Infrastructure.Data;


global using Courses.Shared.BaseResponse;
global using Courses.Shared.Enum;
// Shared Base Classes

// DTOs
global using Courses.Shared.DTOs.AuthDtos;



global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.IdentityModel.Tokens;
global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;
global using System.Text;

//Middleware
global using Courses.Api.Middleware;

// DI
global using Courses.Infrastructure;
global using Courses.Application;
