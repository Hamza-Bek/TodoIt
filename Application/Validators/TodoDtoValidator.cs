namespace DefaultNamespace;
using Application.Dtos.Todo;
using FluentValidation;
public class TodoDtoValidator : AbstractValidator<TodoDto>
{
    public TodoDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .NotNull().WithMessage("Title is required")
            .Length(1, 1000).WithMessage("Title must be between 1 and 1000 characters");
                
        RuleFor(x => x.Description)
            .MaximumLength(10000).WithMessage("Description must be less than 10000 characters");        
    }
}