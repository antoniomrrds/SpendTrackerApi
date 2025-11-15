using NSubstitute;
using SharedKernel;
using WebApi.Domain.Categories;
using WebApi.Features.Categories.Common;
using WebApi.Features.Categories.Delete;

namespace WebApi.Tests.Features.Categories.Delete;

[Trait("Type", "Integration")]
public class DeleteCategoryUseCaseTests
{
    private readonly DeleteCategoryUseCase _sut;
    private readonly ICategoryWriterRepository _categoryWriterRepository;
    private readonly CancellationToken _ct = CancellationToken.None;
    private readonly DeleteCategoryInput _input = DeleteCategoryFixture.DeleteInput();

    public DeleteCategoryUseCaseTests()
    {
        _categoryWriterRepository = Substitute.For<ICategoryWriterRepository>();

        _sut = new DeleteCategoryUseCase(_categoryWriterRepository);
    }

    [Fact]
    public async Task Perform_WhenCategoryToDeleteIsNotFound_ShouldReturnFailure()
    {
        //Arrange
        MakeDeleteAsyncReturns(false);
        //Act
        Result<bool> result = await _sut.Perform(input: _input, cancellationToken: _ct);
        //Assert
        await _categoryWriterRepository
            .Received(1)
            .DeleteAsync(id: _input.Id, cancellationToken: _ct);
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(CategoryErrors.NotFound(_input.Id.ToString()));
    }

    [Fact]
    public async Task Perform_WhenCategoryExists_ShouldDeleteCategory()
    {
        MakeDeleteAsyncReturns();
        Result<bool> result = await _sut.Perform(input: _input, cancellationToken: _ct);
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBeTrue();
    }

    private void MakeDeleteAsyncReturns(bool returnValue = true)
    {
        _categoryWriterRepository
            .DeleteAsync(id: _input.Id, cancellationToken: _ct)
            .Returns(returnValue);
    }
}
