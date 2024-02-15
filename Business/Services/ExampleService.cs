using Application.Consts;
using Application.Generics;
using Application.Redis;
using Application.Services;
using Business.Generics;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ExampleService : GenericService<Example>, IExampleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRedisCacheService _redisCacheService;

        public ExampleService(IUnitOfWork unitOfWork, IRedisCacheService redisCacheService) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _redisCacheService = redisCacheService;
        }

        public async Task<IEnumerable<Example>> GetExamplesAsync()
        {
            return await _redisCacheService.GetAndSetDataAsync
            (
                async () => await _unitOfWork.ExampleRepository.GetAllAsync(),
                Redis.Keys.Example,
                TimeSpan.FromMinutes(2)
            );
        }
    }
}
