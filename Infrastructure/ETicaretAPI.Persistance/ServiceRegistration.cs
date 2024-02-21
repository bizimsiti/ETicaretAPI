using ETicaretAPI.Persistance.Contexts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ETicaretAPI.Persistance.Repositories;
using ETicaretAPI.Application.Repositories;

namespace ETicaretAPI.Persistance
{
    public static class ServiceRegistration
    {
        public static void AddPersistanceService(this IServiceCollection services)
        {
           
            services.AddDbContext<ETicaretAPIDbContext>(options => options.UseNpgsql(Configurations.ConnectionsString));
            
            services.AddScoped<ICustomerReadRepository, CustomerReadRepository>();
            services.AddScoped<ICustomerWriteRepository, CustomerWriteRepository>();
            
            services.AddScoped<IOrderReadRepository, OrderReadRepository>();
            services.AddScoped<IOrderWriteRepository, OrderWriteRepository>();

            services.AddScoped<IProductReadRepository, ProductReadRepository>();
            services.AddScoped<IProductWriteRepository, ProductWriteRepository>();

            services.AddScoped<IUploadFileReadRepository, UploadFileReadRepository>();
            services.AddScoped<IUploadWriteRepository,UploadFileWriteRepository>();

            services.AddScoped<IProductImageFileReadRepository, ProductImageReadRepository>();
            services.AddScoped<IProductImageFileWriteRepository, ProductImageWriteRepository>();

            services.AddScoped<IInvoiceFileReadRepository, InvoinceFileReadRepository>();
            services.AddScoped<IInvoiceFileWriteRepository, InvoinceFileWriteRepository>();





        }
    }
}
