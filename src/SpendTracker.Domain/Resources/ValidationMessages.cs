using System.Text;

namespace SpendTracker.Domain.Resources;

public static class ValidationMessages
{
    public static readonly CompositeFormat RequiredField =
        CompositeFormat.Parse("O campo {0} é obrigatório.");

    public static readonly CompositeFormat InvalidValue =
        CompositeFormat.Parse("O valor informado para {0} é inválido.");
    
    public static readonly CompositeFormat MaxChars =
        CompositeFormat.Parse("O campo {0} não pode exceder {1} caracteres.");
}
