using FastEndpoints;
using FluentValidation;
using TestingWebApi.Endpoints.Users;

namespace TestingWebApi.Features.Users
{
    public class CreateUserValidator : Validator<CreateUserRequest>
    {
        public CreateUserValidator() {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email can't be empty");    
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name can't be empty"); 
        }
    }
}
