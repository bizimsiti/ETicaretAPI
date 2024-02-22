using ETicaretAPI.Application.Abstractions.Storage;
using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Application.RequestParameters;
using ETicaretAPI.Application.ViewModels.Products;
using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        readonly private IProductReadRepository _productReadRepository;
        readonly private IProductWriteRepository _productWriteRepository;
        readonly private IWebHostEnvironment _webHostEnvironment;
        //readonly private IFileService _fileService;
        readonly private IUploadFileReadRepository _uploadFileReadRepository;
        readonly private IUploadWriteRepository _uploadWriteRepository;
        readonly private IProductImageFileReadRepository _productImageFileReadRepository;
        readonly private IProductImageFileWriteRepository _productImageFileWriteRepository;
        readonly private IInvoiceFileWriteRepository _invoiceFileWriteRepository;
        readonly private IInvoiceFileReadRepository _invoiceFileReadRepository;
        readonly private IStorageService _storageService;

        public ProductsController(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository, IWebHostEnvironment webHostEnvironment, IUploadFileReadRepository uploadFileReadRepository, IUploadWriteRepository uploadWriteRepository, IProductImageFileReadRepository productImageFileReadRepository, IProductImageFileWriteRepository productImageFileWriteRepository, IInvoiceFileWriteRepository invoiceFileWriteRepository, IInvoiceFileReadRepository invoiceFileReadRepository, IStorageService storageService)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
            _webHostEnvironment = webHostEnvironment;
            //_fileService = fileService;
            _uploadFileReadRepository = uploadFileReadRepository;
            _uploadWriteRepository = uploadWriteRepository;
            _productImageFileReadRepository = productImageFileReadRepository;
            _productImageFileWriteRepository = productImageFileWriteRepository;
            _invoiceFileWriteRepository = invoiceFileWriteRepository;
            _invoiceFileReadRepository = invoiceFileReadRepository;
            _storageService = storageService;
        }


        // void yerine Task çünkü repository beklemediği için dispose ediliyor. (scoped)
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]Pagination pagination)
        {
            //await _productWriteRepository.AddRangeAsync(new()
            //{
            //    new(){Id = Guid.NewGuid(),Name = "Product1",Price=123, Stock =12, CreatedDate = DateTime.UtcNow},
            //    new(){Id = Guid.NewGuid(),Name = "Product2",Price=223, Stock =122, CreatedDate = DateTime.UtcNow},
            //    new(){Id = Guid.NewGuid(),Name = "Product3",Price=423, Stock =1122, CreatedDate = DateTime.UtcNow},
            //    new(){Id = Guid.NewGuid(),Name = "Product4",Price=15623, Stock =112, CreatedDate = DateTime.UtcNow},
            //});
            //var count = await _productWriteRepository.SaveAsync();

            //Product p = await _productReadRepository.GetByIdAsync("11da5128-2d37-4261-80e3-b3fca9ce8557", false);
            //p.Name = "ali";
            //var count = await _productWriteRepository.SaveAsync();

            //---------------------------------------------------- 
            //await _productWriteRepository.AddAsync(new()
            //{
            //    Name = "C Product",
            //    Id = Guid.NewGuid(),
            //    Price = 123.13F,
            //    CreatedDate = DateTime.UtcNow
            //});
            //await _productWriteRepository.SaveAsync();

            //------------------savechanges interceptors

            //var customerId = Guid.NewGuid();
            //await _customerWriteRepository.AddAsync(new() { Id = customerId, Name = "ali" });

            //await _orderWriteRepository.AddAsync(new() { Description = "falan filan", Adress = "istanbul gaziosmanpaşa", CustomerId = customerId });
            //await _orderWriteRepository.AddAsync(new() { Description = "falan filan 2", Adress = "istanbul beşiktaş", CustomerId = customerId });
            //await _orderWriteRepository.SaveAsync();

            //Order order = await _orderReadRepository.GetByIdAsync("342f6b5c-e814-4035-a1ad-1cbd12aa3a57");
            //order.Adress = "Ankara";
            //await _orderWriteRepository.SaveAsync();
            var totalCount = _productReadRepository.GetAll(false).Count();
            var products = _productReadRepository.GetAll(false).Skip(pagination.Page * pagination.Size).Take(pagination.Size).Select(p=>new
            {
                p.Id,
                p.Name,
                p.Price,
                p.Stock,
                p.CreatedDate,
                p.UpdatedDate
            });


            return Ok(new{ 
             totalCount,
             products
            });

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Product product = await _productReadRepository.GetByIdAsync(id, false);
            
            return Ok(product);

        }

        [HttpPost]
        public async Task<IActionResult> Post(VMCreateProduct model)
        {
            
            if (ModelState.IsValid)
            await _productWriteRepository.AddAsync(new()
            {
                Name = model.Name,
                Stock = model.Stock,
                Price = model.Price
            });
            await _productWriteRepository.SaveAsync();
            return Ok();

        }

        [HttpPut]
        public async Task<IActionResult> Put(VMUpdateProduct model)
        {
            Product product = await _productReadRepository.GetByIdAsync(model.Id);
            product.Name = model.Name;
            product.Stock = model.Stock;
            product.Price = model.Price;
            await _productWriteRepository.SaveAsync();

            return Ok();

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _productWriteRepository.RemoveAsync(id);
            await _productWriteRepository.SaveAsync();

            return Ok();
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Upload()
        {
            //string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "resources/product-images");
            //if(!Directory.Exists(uploadPath))
            //{
            //    Directory.CreateDirectory(uploadPath);
            //}
            //Random r = new();
            //foreach(IFormFile file in Request.Form.Files)
            //{
            //    string fullPath = Path.Combine(uploadPath, $"{r.Next()}{Path.GetExtension(file.FileName)}");
            //    using FileStream fileStream = new(fullPath, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: false);
            //    await file.CopyToAsync(fileStream);
            //    await fileStream.FlushAsync();  

            //}
            //var datas = await _fileService.UploadAsync("invoices", Request.Form.Files);
            //await _productImageFileWriteRepository.AddRangeAsync(datas.Select(d=> new ProductImageFile() {
            //    Name = d.fileName,
            //    Path = d.path
            //}).ToList());
            //await _productImageFileWriteRepository.SaveAsync();
            var datas = await _storageService.UploadAsync("recources/product-images", Request.Form.Files);
            await _invoiceFileWriteRepository.AddRangeAsync(datas.Select(d => new InvoiceFile()
            {
                Name = d.fileName,
                Path = d.path,
                Price = new Random().Next(),
                Storage = _storageService.StorageName
            }).ToList()) ;
            await _invoiceFileWriteRepository.SaveAsync();

            return Ok();
        }
    }
}
