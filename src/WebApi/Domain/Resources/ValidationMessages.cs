using System.Text;

namespace WebApi.Domain.Resources;

public static class ValidationMessages
{
    public static readonly CompositeFormat RequiredField = CompositeFormat.Parse(
        "O campo {0} é obrigatório."
    );

    public static readonly CompositeFormat InvalidValue = CompositeFormat.Parse(
        "O valor fornecido para {0} é inválido."
    );

    public static readonly CompositeFormat DateIsFuture = CompositeFormat.Parse(
        "A data fornecida '{0}' não pode estar no futuro."
    );

    public static readonly CompositeFormat MaxChars = CompositeFormat.Parse(
        "O campo {0} não pode exceder {1} caracteres."
    );

    public static readonly CompositeFormat GreaterThan = CompositeFormat.Parse(
        "O campo {0} deve ser maior que {1}."
    );

    public static readonly CompositeFormat WhiteSpaceOnly = CompositeFormat.Parse(
        "O campo {0} não pode conter apenas espaços em branco."
    );

    public static readonly CompositeFormat StringLengthRangeMessage = CompositeFormat.Parse(
        "O campo {0} deve ter entre {1} e {2} caracteres."
    );
}
