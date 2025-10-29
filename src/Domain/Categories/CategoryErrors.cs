using SharedKernel;

namespace Domain.Categories;

public static class CategoryErrors
{
    public static FailureReason NotFound(string id) =>
        new("Category.NotFound", $"A categoria com o id: '{id}' não foi encontrada.");
    public static readonly FailureReason CategoryNameAlreadyExists  = new(
        "Category.NameAlreadyExists", "Uma categoria com o nome fornecido já existe.");
}
