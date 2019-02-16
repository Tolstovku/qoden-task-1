using Qoden.Validation;

namespace WebApplication1.Validation
{
    public interface IValidate
    {
        void Validate(IValidator validator);
    }
}