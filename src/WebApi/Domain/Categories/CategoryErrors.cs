namespace WebApi.Domain.Categories;

public static class CategoryErrors
{
    public static FailureReason NotFound(string id) =>
        new(CategoryErrorCodes.NotFound, $"A categoria com o id: '{id}' não foi encontrada.");

    public static readonly FailureReason NameAlreadyExists = new(
        CategoryErrorCodes.NameAlreadyExists,
        "Uma categoria com o nome fornecido já existe."
    );
}
