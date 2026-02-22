using AutoMapper;
using FluentValidation;
using Model.Entities;

namespace WebUI.Models.Auth;

public class SignUpRequest
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public bool RememberMe { get; set; } = false;
}

public class SignUpRequestValidator : AbstractValidator<SignUpRequest>
{
    public SignUpRequestValidator()
    {
        RuleFor(b => b.Email).NotNull().EmailAddress().NotEmpty().EmailAddress();
        RuleFor(b => b.Password).NotNull().MinimumLength(6).NotEmpty();

        RuleFor(b => b.FirstName).NotNull().MinimumLength(2).NotEmpty();
        RuleFor(b => b.LastName).NotNull().MinimumLength(2).NotEmpty().NotEqual(s => s.FirstName);
    }
}

public class SignUpRequestMappingProfile : Profile
{
    public SignUpRequestMappingProfile()
    {
        CreateMap<SignUpRequest, User>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember, destMember) => !Equals(srcMember, destMember)));
    }
}