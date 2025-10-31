using Ecommerce.viwemodel;
using Mapster;

namespace Ecommerce.Config
{
    public static class MapsterConfig
    {
        public static void RegisterMapsterConfig(this IServiceCollection services)
        {
            TypeAdapterConfig<Appliccationusr, ApplicationuserVM>
                    .NewConfig()
                    .Map(d => d.FullName, s => $"{s.firstname} {s.lastname}")
                    .TwoWays();
        }
    }
}
