using MyCompany.Domain.Repositories.Abstract;

namespace MyCompany.Domain
{
    public class DataManager(IServiceCategoriesRepository serviceCategoriesRepository,
        IServicesRepository servicesRepository)
    {
        public IServiceCategoriesRepository ServiceCategories { get; set; } = serviceCategoriesRepository;
        public IServicesRepository Services { get; set; } = servicesRepository;


    }
}
