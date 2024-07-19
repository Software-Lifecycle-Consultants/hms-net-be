using AutoMapper;
using HMS.Controllers.Admin;
using HMS.Models;
using HMS.Models.Admin;
using HMS.Services.Repository_Service;
using HMS.Services.RepositoryService;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Controllers
{
    public class HMSControllerBase<TController, TEntity> : ControllerBase where TEntity : class
    {
        protected readonly ILogger<TController> _logger;
        protected readonly IMapper _mapper;
        protected readonly IRepositoryService<TEntity> _repositoryService;
        protected readonly IAdminRepositoryService _adminRepository;

        // Constructor for general repository service
        public HMSControllerBase(ILogger<TController> logger, IRepositoryService<TEntity> repositoryService, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repositoryService = repositoryService ?? throw new ArgumentNullException(nameof(repositoryService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // Constructor for admin repository service
        public HMSControllerBase(ILogger<TController> logger, IAdminRepositoryService adminRepositoryService, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _adminRepository = adminRepositoryService ?? throw new ArgumentNullException(nameof(adminRepositoryService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
    }
}

