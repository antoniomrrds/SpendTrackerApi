using SharedKernel;

namespace Domain.Errors;

public static class CommonErrors
{
    public static readonly FailureReason GuidInvalid = new("Invalid.GUID", "GUID Inválido.");
}
