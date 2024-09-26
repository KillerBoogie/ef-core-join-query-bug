namespace VC.WebApi.Infrastructure.MediatR.Pipelines
{
    using global::MediatR;
    using Microsoft.EntityFrameworkCore.Storage;
    using Microsoft.Extensions.Logging;
    using VC.WebApi.Infrastructure.EFCore.Context;

    public class TransactionPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>, ITransactional
    {
        private readonly VCDbContext _context;
        private readonly ILogger<TransactionPipelineBehavior<TRequest, TResponse>> _logger;

        public TransactionPipelineBehavior(VCDbContext context, ILogger<TransactionPipelineBehavior<TRequest, TResponse>> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Beginning transaction for {RequestType}", typeof(TRequest).Name);

            TResponse response;

            try
            {
                IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

                response = await next();

                await _context.Database.CommitTransactionAsync(cancellationToken);

                _logger.LogInformation("Transaction committed for {RequestType}", typeof(TRequest).Name);
            }
            catch (Exception ex)
            {
                await _context.Database.RollbackTransactionAsync(cancellationToken);

                _logger.LogError(ex, "Transaction rolled back for {RequestType}", typeof(TRequest).Name);

                throw;
            }

            return response;
        }
    }

}
