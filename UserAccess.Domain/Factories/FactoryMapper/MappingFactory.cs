using AutoMapper;
using Microsoft.Extensions.Logging;

namespace UserAccess.Domain.Factories.FactoryMapper
{
    public class MappingFactory<T> : IMappingFactory<T>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        public MappingFactory(IMapper mapper, ILogger<T> logger)
        {
            _mapper = mapper;
            _logger = logger;
        }

        public T CreateFrom<S>(S sourceObject)
        {
            T responseObject = default;
            try
            {
                responseObject = _mapper.Map<T>(sourceObject);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Exception Mapping from {typeof(S)} to {typeof(T)}");
            }
            return responseObject;
        }

        public IEnumerable<T> CreateMultipleFrom<S>(S sourceObject)
        {
            IEnumerable<T> responseObject = null;
            try
            {
                responseObject = _mapper.Map<IEnumerable<T>>(sourceObject);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Exception Mapping from {typeof(S)} to {typeof(T)}");
            }
            return responseObject;
        }
    }
}
