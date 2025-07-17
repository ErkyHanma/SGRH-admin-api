using SGRH.Application.Interfaces.Repositories.Hotel;
using SGRH.Application.Interfaces.UseCases;
using SGRH.Domain.Base;

public class RatesMustNotBeOverlapping : IMustNotBeOverlapping<int>
{
    private readonly IRatesRepository _ratesRepository;

    public RatesMustNotBeOverlapping(IRatesRepository ratesRepository)
    {
        _ratesRepository = ratesRepository;
    }

    public async Task<OperationResult<string>> Validate(int categoryId, int seasonId)
    {
        return await ValidateInternal(categoryId, seasonId, null);
    }

    public async Task<OperationResult<string>> ValidateWithExclusion(int categoryId, int seasonId, int excludeRateId)
    {
        return await ValidateInternal(categoryId, seasonId, excludeRateId);
    }

    private async Task<OperationResult<string>> ValidateInternal(int categoryId, int seasonId, int? excludeRateId)
    {
        var overlappingRates = await _ratesRepository.GetAllAsync(r =>
            r.CategoryId == categoryId &&
            r.SeasonId == seasonId &&
            !r.IsDeleted &&
            (!excludeRateId.HasValue || r.RateId != excludeRateId.Value));

        if (!overlappingRates.IsSuccess)
            return OperationResult<string>.Failure($"Error checking overlapping rates: {overlappingRates.Message}");

        if (overlappingRates.Data != null && overlappingRates.Data.Any())
            return OperationResult<string>.Failure("A rate already exists for this category and season.");

        return OperationResult<string>.Success("No overlapping rates found.");
    }
}
