namespace SharedKernel.Abstractions;

public interface IUseCase<in TInput, out TOutput>
{
    TOutput Perform(TInput input);
}

public interface IUseCaseWithoutInput<out TOutput>
{
    TOutput Perform();
}
