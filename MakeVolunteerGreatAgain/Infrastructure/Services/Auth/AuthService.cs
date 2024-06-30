using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MakeVolunteerGreatAgain.Core.Entities;
using MakeVolunteerGreatAgain.Core.Repositories.DTO;
using MakeVolunteerGreatAgain.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MakeVolunteerGreatAgain.Core.Repositories;

namespace MakeVolunteerGreatAgain.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<CommonUser> _userManager;
        private readonly SignInManager<CommonUser> _signInManager;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IUnitOfWork _unitOfWork;

        // Конструктор для инициализации зависимостей
        public AuthService(
            UserManager<CommonUser> userManager, 
            SignInManager<CommonUser> signInManager, 
            IJwtTokenService jwtTokenService, 
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
            _unitOfWork = unitOfWork;
        }

        // Метод для регистрации волонтера
        public async Task<AuthResultDTO> RegisterVolunteerAsync(RegisterVolunteerDTO model)
        {
            // Создаем объект пользователя CommonUser
            var user = new CommonUser
            {
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber
            };

            // Создаем пользователя в базе данных
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                // Возвращаем ошибки, если создание пользователя не удалось
                return new AuthResultDTO { Success = false, Errors = result.Errors.Select(e => e.Description).ToList() };
            }

            // Получаем ID созданного пользователя
            var userId = Convert.ToInt32(user.Id);

            // Добавляем пользователя в роль Volunteer
            await _userManager.AddToRoleAsync(user, "Volunteer");

            // Создаем запись в таблице Volunteer
            var volunteer = new Volunteer
            {
                CommonUserId = userId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                BirthDate = model.BirthDate,
            };

            // Сохраняем запись в таблице Volunteer через UnitOfWork
            await _unitOfWork.VolunteerRepository.AddAsync(volunteer);
            await _unitOfWork.CommitAsync();

            // Автоматический логин пользователя после регистрации
            return await LoginAsync(new LoginDTO { Email = model.Email, Password = model.Password });
        }

        // Метод для регистрации организации
        public async Task<AuthResultDTO> RegisterOrganizationAsync(RegisterOrganizationDTO model)
        {
            // Создаем объект пользователя CommonUser
            var user = new CommonUser
            {
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber
            };

            // Создаем пользователя в базе данных
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                // Возвращаем ошибки, если создание пользователя не удалось
                return new AuthResultDTO { Success = false, Errors = result.Errors.Select(e => e.Description).ToList() };
            }

            // Получаем ID созданного пользователя
            var userId = Convert.ToInt32(user.Id);

            // Добавляем пользователя в роль Organization
            await _userManager.AddToRoleAsync(user, "Organization");

            // Создаем запись в таблице Organization
            var organization = new Organization
            {
                CommonUserId = userId,
                Name = model.Name,
                LegalAddress = model.LegalAddress,
            };

            // Сохраняем запись в таблице Organization через UnitOfWork
            await _unitOfWork.OrganizationRepository.AddAsync(organization);
            await _unitOfWork.CommitAsync();

            // Автоматический логин пользователя после регистрации
            return await LoginAsync(new LoginDTO { Email = model.Email, Password = model.Password });
        }

        // Метод для логина
        public async Task<AuthResultDTO> LoginAsync(LoginDTO model)
        {
            // Поиск пользователя по email
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Возвращаем ошибку, если пользователь не найден
                return new AuthResultDTO { Success = false, Errors = new List<string> { "User does not exist." } };
            }

            // Проверка пароля и попытка входа
            var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, false, false);

            if (!result.Succeeded)
            {
                // Возвращаем ошибку, если логин не удался
                return new AuthResultDTO { Success = false, Errors = new List<string> { "Invalid login attempt." } };
            }

            // Генерация JWT токена
            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtTokenService.GenerateToken(user, roles);

            // Возвращаем результат успешного логина и токен
            return new AuthResultDTO { Success = true, Token = token };
        }
    }
}
