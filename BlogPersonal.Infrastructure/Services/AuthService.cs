using AutoMapper;
using BlogPersonal.Application.DTOs.Auth;
using BlogPersonal.Application.Interfaces;
using BlogPersonal.Domain.Entities;
using BlogPersonal.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BlogPersonal.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly BlogDbContext _context;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IMapper _mapper;

        public AuthService(BlogDbContext context, IJwtTokenGenerator jwtTokenGenerator, IMapper mapper)
        {
            _context = context;
            _jwtTokenGenerator = jwtTokenGenerator;
            _mapper = mapper;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginUserDto loginDto)
        {
            var user = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                throw new Exception("Credenciales inválidas");
            }

            var token = _jwtTokenGenerator.GenerateToken(user);

            return new AuthResponseDto
            {
                Id = user.Id,
                Email = user.Email,
                NombreUsuario = user.NombreUsuario,
                Token = token,
                Rol = user.Rol.Nombre
            };
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterUserDto registerDto)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Email == registerDto.Email))
            {
                throw new Exception("El email ya está registrado");
            }

            if (await _context.Usuarios.AnyAsync(u => u.NombreUsuario == registerDto.NombreUsuario))
            {
                throw new Exception("El nombre de usuario ya está en uso");
            }

            // Default role: Usuario (Readers who can comment)
            var defaultRole = await _context.Roles.FirstOrDefaultAsync(r => r.Nombre == "Usuario");
            if (defaultRole == null)
            {
                // Fallback to Autor if Usuario doesn't exist (should not happen with seed)
                defaultRole = await _context.Roles.FirstOrDefaultAsync(r => r.Nombre == "Autor");
                if (defaultRole == null) throw new Exception("Rol por defecto no encontrado");
            }

            var user = new Usuario
            {
                Email = registerDto.Email,
                NombreUsuario = registerDto.NombreUsuario,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                RolId = defaultRole.Id,
                Activo = true
            };

            _context.Usuarios.Add(user);
            await _context.SaveChangesAsync();

            // Reload user with Role to generate token
            await _context.Entry(user).Reference(u => u.Rol).LoadAsync();

            var token = _jwtTokenGenerator.GenerateToken(user);

            return new AuthResponseDto
            {
                Id = user.Id,
                Email = user.Email,
                NombreUsuario = user.NombreUsuario,
                Token = token,
                Rol = user.Rol.Nombre
            };
        }
    }
}
