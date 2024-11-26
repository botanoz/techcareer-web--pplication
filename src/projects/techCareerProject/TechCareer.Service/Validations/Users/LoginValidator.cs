using Core.Security.Dtos;
using FluentValidation;

namespace TechCareer.Service.Validations.Users;

public class LoginValidator : AbstractValidator<UserForLoginDto>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email).EmailAddress().WithMessage("Email Formatında Değil")
            .NotEmpty().WithMessage("Email Alanı boş olamaz.");
    }
}