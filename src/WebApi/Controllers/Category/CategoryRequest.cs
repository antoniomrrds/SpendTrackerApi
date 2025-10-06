namespace WebApi.Controllers.Category;

public record CategoryRequest(
    string Name,
    string Description
);