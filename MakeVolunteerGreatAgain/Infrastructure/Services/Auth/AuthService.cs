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
using MakeVolunteerGreatAgain.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MakeVolunteerGreatAgain.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<CommonUser> _userManager;
        private readonly SignInManager<CommonUser> _signInManager;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _context;

        // Конструктор для инициализации зависимостей
        public AuthService(
            UserManager<CommonUser> userManager, 
            SignInManager<CommonUser> signInManager, 
            IJwtTokenService jwtTokenService, 
            IUnitOfWork unitOfWork,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
            _unitOfWork = unitOfWork;
            _context = context;
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
        
        public async Task<UpdateVolunteerDTO> UpdateVolunteerAsync(UpdateVolunteerDTO model,int volunteerCommonUserId)
        {
            // Найти волонтера по CommonUserId
            var volunteer = await _context.Volunteers
                .FirstOrDefaultAsync(v => v.CommonUserId == volunteerCommonUserId);
            if (volunteer == null)
            {
                throw new Exception("Volunteer not found");
            }

            // Обновить информацию о волонтере
            volunteer.FirstName = model.FirstName;
            volunteer.LastName = model.LastName;
            volunteer.MiddleName = model.MiddleName;
            volunteer.PhotoPath = model.PhotoPath;
            volunteer.BirthDate = model.BirthDate;
            volunteer.About = model.About;
            volunteer.ParticipationCount = model.ParticipationCount;
            // Сохранить изменения в базе данных
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<UpdateOrganizationDTO> UpdateOrganizationAsync(UpdateOrganizationDTO model, int organizationCommonUserId)
        {
            var organization = await _context.Organizations
                .FirstOrDefaultAsync(o => o.CommonUserId == organizationCommonUserId);
            if (organization == null)
            {
                throw new Exception("Organization not found");
            }
            
            // Обновить информацию об организации
            organization.Name = model.Name;
            organization.PhotoPath = model.PhotoPath;
            organization.LegalAddress = model.LegalAddress;
            organization.Website = model.Website;
            organization.WorkingHours = model.WorkingHours;
            // Сохранить изменения в базе данных
            await _context.SaveChangesAsync();
            return model;
        }
        
        public async Task<Volunteer> GetVolunteerProfileAsync(int volunteerCommonUserId)
        {
            var volunteer = await _context.Volunteers
                .Include(v => v.CommonUser) // Загрузка связанных данных из таблицы CommonUser
                .FirstOrDefaultAsync(v => v.CommonUserId == volunteerCommonUserId);
            if (volunteer == null)
            {
                throw new Exception("Volunteer not found");
            }

            // Возвращаем профиль волонтера с номером телефона
            return new Volunteer
            {
                Id = volunteer.Id,
                CommonUserId = volunteer.CommonUserId,
                FirstName = volunteer.FirstName,
                LastName = volunteer.LastName,
                MiddleName = volunteer.MiddleName,
                PhotoPath = volunteer.PhotoPath,
                BirthDate = volunteer.BirthDate,
                About = volunteer.About,
                ParticipationCount = volunteer.ParticipationCount,
                PhoneNumber = volunteer.CommonUser.PhoneNumber // Получение номера телефона из CommonUser
            };
        }
        
        public async Task<Organization> GetOrganizationProfileAsync(int organizationCommonUserId)
        {
            var organization = await _context.Organizations
                .Include(o => o.CommonUser)
                .FirstOrDefaultAsync(o => o.CommonUserId == organizationCommonUserId);
            if (organization == null)
            {
                throw new Exception("Organization not found");
            }
            
            return new Organization
            {
                Id = organization.Id,
                CommonUserId = organization.CommonUserId,
                Name = organization.Name,
                PhotoPath = organization.PhotoPath,
                LegalAddress = organization.LegalAddress,
                Website = organization.Website,
                WorkingHours = organization.WorkingHours,
                PhoneNumber = organization.CommonUser.PhoneNumber // Номер телефона теперь из сущности Organization
            };
        }
    }
}
