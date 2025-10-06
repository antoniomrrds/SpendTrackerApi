using SharedKernel;

namespace Domain.Categories;

public static class CategoryErrors
{
    public static  FailureReason NotFound(string name) =>new("Category.NotFound", $"The category with the Name = '{name}' was not found");
    public static readonly FailureReason CategoryNameAlreadyExists  = new(
        "Category.NameAlreadyExists", "A category with the provided name already exists.");
}
