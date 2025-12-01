namespace SharedKernel.Abstractions;

public interface IUseCaseMarker;

public interface IUseCase<in TInput, out TOutput> : IUseCaseMarker
{
    TOutput Perform(TInput input, CancellationToken cancellationToken = default);
}

public interface IUseCaseWithoutInput<out TOutput> : IUseCaseMarker
{
    TOutput Perform(CancellationToken cancellationToken = default);
}
