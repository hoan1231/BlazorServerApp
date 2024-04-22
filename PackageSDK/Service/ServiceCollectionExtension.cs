using ManagementPackage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageSDK.Service
{
    public static class ServiceCollectionExtension
    {
        public static void AddGrpcSdk(this IServiceCollection services)
        {
            services.AddGrpcClient<PackageProto.PackageProtoClient>(client =>
            {
                client.Address = new Uri("http://localhost:5184");
            });
            services.AddScoped<IPackageGrpcService, PackageGrpcService>();
        }

    }
}
