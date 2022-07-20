using FluentValidation;
using RepotringService.BLL.Commands.Report;

namespace ReportingService.BLL.Commands.Validation.ReportValidation
{
    public class FileValidatior: AbstractValidator<AddFileCommand>
    {
        public FileValidatior()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.File).NotEmpty();
            RuleFor(x => x.File.Length).NotEmpty();
            RuleFor(x => x.File.FileName).Must(x => Path.GetExtension(x).Substring(1).Equals("csv")).WithMessage("Wrong File Format");
        }
    }
}
