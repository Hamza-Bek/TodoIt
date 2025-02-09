using Application.Dtos.Folder;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class FolderDtoValidator : AbstractValidator<FolderDto>
    {
        public FolderDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(150).WithMessage("Name can't be longer than 150 characters");            
        }
    }
}
