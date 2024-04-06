using FluentValidation;
using VideoManagement.API.Dtos;
using VideoManagement.API.Helpers;

namespace VideoManagement.API.Validations;

public class VideoValidator : AbstractValidator<ProductCreateDto>
{
    public VideoValidator()
    {
        RuleFor(dto => dto.Name).NotEmpty().NotNull().WithMessage("Name is required!")
            .MinimumLength(3).WithMessage("Name must be more than 3 characters!")
            .MaximumLength(255).WithMessage("Name must be lass than 50 characters!");

        RuleFor(dto => dto.Video).NotEmpty().NotNull().WithMessage("Video field is required!");
        RuleFor(dto => dto.Video.FileName).Must(predicate =>
        {
            FileInfo fileInfo = new FileInfo(predicate);
            return MediaHelper.GetVideoExtensions().Contains(fileInfo.Extension);
        }).WithMessage("This file type is not VIDEO file!");
    }
}
