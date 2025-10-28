using Application.Categories.GetById;
using Domain.Errors;
using SharedKernel;
using TestUtilities.Common;

namespace Application.Tests.Categories.GetById;

[Trait("Type", "Unit")]
public class GetByIdUseCaseTests : TestCommon
{
    private readonly GetByIdUseCase _sut = new();

    [Fact]
    public void Perform_WhenGuidIsEmpty_ShouldReturnFailure()
    {
        //Arrange 
        Guid invalidGuid = Guid.Empty;
        GetByIdCommand command = new(invalidGuid);
        //Act
        Result<bool> result = _sut.Perform(command);
        //Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(CommonErrors.GuidInvalid);
    }
}