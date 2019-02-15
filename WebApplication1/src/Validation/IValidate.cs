using Qoden.Validation;

namespace WebApplication1.Database.Entities
{
    public interface IValidate
    {
        void Validate(IValidator validator);
    }
}