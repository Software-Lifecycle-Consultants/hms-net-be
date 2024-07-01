using AutoMapper;
using HMS.Controllers.Admin;
using HMS.Models;
using HMS.Models.Admin;
using HMS.Services.Repository_Service;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Controllers
{

    public class HMSControllerBase<TController, TEntity> : ControllerBase where TEntity : class
    {
        protected readonly ILogger<TController> _logger;
       // protected readonly TService _repositoryService;
        protected readonly IMapper _mapper;
        protected readonly IRepositoryService<TEntity> _repositoryService;
     

        public HMSControllerBase(ILogger<TController> logger, IRepositoryService<TEntity> repositoryService, IMapper mapper)
        {
            _logger = logger;
            _repositoryService = repositoryService;
            _mapper = mapper;
        }

      
    }
}
