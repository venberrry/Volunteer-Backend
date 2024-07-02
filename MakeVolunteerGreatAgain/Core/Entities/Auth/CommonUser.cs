namespace MakeVolunteerGreatAgain.Core.Entities
{
    using Microsoft.AspNetCore.Identity;
    
    //
    // Используется в качетстве базы под создаваемых пользователей (Волонтёров и Организаций)
    //
    // Содержит в себе поля:
    // Username
    // NormalizedUserName (UpperCase)
    // Email
    // NormalizedUserName
    // bool EmailConfirmed
    // PasswordHash
    // SecurityStamp - случайное значение, которое изменяется при изменении учетных данных пользователя
    // ConcurrencyStamp - защиты от конфликтов параллельных обновлений в бд
    // PhoneNumber
    // bool PhoneNumberConfirmed
    // bool TwoFactorEnabled
    // LockoutEnd
    // LockoutEnabled
    // AccessFailedCount
    // 
    public class CommonUser : IdentityUser<int>
    {

    }
}