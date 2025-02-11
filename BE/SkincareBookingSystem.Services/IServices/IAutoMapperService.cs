using AutoMapper;

namespace SkincareBookingSystem.Services.IServices
{
    public interface IAutoMapperService
    {
        TDestination Map<TSource, TDestination>(TSource source);
        IEnumerable<TDestination> MapCollection<TSource, TDestination>(IEnumerable<TSource> source);
    }
}
