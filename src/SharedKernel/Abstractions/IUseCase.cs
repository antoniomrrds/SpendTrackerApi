namespace SharedKernel.Abstractions;

public interface IUseCase<in TInput, out TOutput>
{
    TOutput Perform(TInput input, CancellationToken cancellationToken = default);
}

public interface IUseCaseWithoutInput<out TOutput>
{
    TOutput Perform(CancellationToken cancellationToken = default);
}
