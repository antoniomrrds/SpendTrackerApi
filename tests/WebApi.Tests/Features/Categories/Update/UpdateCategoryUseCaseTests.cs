using NSubstitute;
using SharedKernel;
using TestUtilities.Common;
using WebApi.Domain.Categories;
using WebApi.Features.Categories.Common;
using WebApi.Features.Categories.Update;
using WebApi.Tests.Domain.Categories;

namespace WebApi.Tests.Features.Categories.Update;

public class UpdateCategoryUseCaseTests : TestCommon
{
    private readonly ICategoryRepository _categoryRepositoryMock;
    private readonly UpdateCategoryUseCase _sut;
    private readonly UpdateCategoryInput _input = UpdateCategoryFixture.UpdateInput();
    private readonly Category _category = CategoryFixture.GetCategory();

    public UpdateCategoryUseCaseTests()
    {
        _categoryRepositoryMock = Substitute.For<ICategoryRepository>();
        _categoryRepositoryMock.HasCategoryWithNameAsync(_input.Name).Returns(false);
        _categoryRepositoryMock.UpdateAsync(_category).Returns(false);
        _sut = new UpdateCategoryUseCase(_categoryRepositoryMock);
    }

    [Fact]
    public async Task Perform_WhenNameIsTaken_ShouldReturnFailureWithProperError()
    {
        _categoryRepositoryMock
            .HasCategoryWithNameAsync(
                name: _input.Name,
                excludeId: _input.Id,
                cancellationToken: CancellationToken
            )
            .Returns(true);

        Result<bool> result = await _sut.Perform(_input);

        await _categoryRepositoryMock
            .Received(1)
            .HasCategoryWithNameAsync(
                Arg.Is<string>(n => n == _input.Name),
                Arg.Is<Guid?>(id => id == _input.Id),
                Arg.Any<CancellationToken>()
            );

        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(CategoryErrors.NameAlreadyExists);
    }
}
