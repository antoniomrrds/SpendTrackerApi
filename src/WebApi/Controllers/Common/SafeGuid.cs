using WebApi.ModelBinders;

namespace WebApi.Controllers.Common;

[ModelBinder(BinderType = typeof(SafeGuidBinder))]
public class SafeGuid
{
    public required Guid Value { get; set; }

    public static implicit operator Guid(SafeGuid safeGuid) => safeGuid.Value;
}
