namespace SpendTracker.Api.Controllers.Category;

public record CategoryRequest(
    string Name,
    string Description
);