using eshop.product.service.application.Dtos;
using eshop.product.service.domain.Events;
using eshop.product.service.domain.Products;
using eshop.product.service.persistence.Uow;
using Microsoft.Extensions.Logging;

namespace eshop.product.service.application.Products;

public class ProductService : IProductService
{
    private readonly ILogger _logger;
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(ILogger<ProductService> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateProductResponseDto> CreateProductAsync(CreateProductRequestDto createProductRequestDto, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ValidateCreateProductRequest(createProductRequestDto);

        Product product = new Product
        {
            Id = Guid.NewGuid().ToString(),
            Name = createProductRequestDto.Name,
            Description = createProductRequestDto.Description,
            Price = createProductRequestDto.Price,
            Inventory = 0,
            Stars = 0,
            NumberOfReviews = 0
        };

        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            await _unitOfWork.ProductRepository.CreateAsync(product, cancellationToken);
            _unitOfWork.Events.Add(new ProductCreatedEvent
            {
                Id = Guid.NewGuid().ToString(),
                ProductId = product.Id,
                Name = product.Name,
            });
            await _unitOfWork.CommitAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error occurred when creating a product");
            throw;
        }

        return new CreateProductResponseDto
        {
            Id = product.Id,
        };
    }

    public async Task<GetProductsResponseDto> GetAllProductsAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        List<Product> products;
        try
        {
            products = await _unitOfWork.ProductRepository.ReadAllAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error occurred when getting all products");
            throw;
        }

        GetProductsResponseDto getProductsResponseDto = new GetProductsResponseDto();
        foreach (Product product in products)
        {
            getProductsResponseDto.Products.Add(new GetProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                Inventory = product.Inventory,
                Stars = product.Stars,
                NumberOfReviews = product.NumberOfReviews,
            });
        }

        return getProductsResponseDto;
    }

    public async Task<GetProductsResponseDto> GetProductByIdAsync(string productId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Product foundProduct;
        try
        {
            foundProduct = await _unitOfWork.ProductRepository.ReadAsync(productId, cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error occurred while finding product with {id}", productId);
            throw;
        }

        if (foundProduct is null)
        {
            throw new NotFoundException($"Product with id {productId} not found!");
        }

        GetProductsResponseDto getProductsResponseDto = new GetProductsResponseDto();
        getProductsResponseDto.Products.Add(new GetProductResponseDto
        {
            Id = foundProduct.Id,
            Name = foundProduct.Name,
            Price = foundProduct.Price,
            Description = foundProduct.Description,
            Inventory = foundProduct.Inventory,
            Stars = foundProduct.Stars,
            NumberOfReviews = foundProduct.NumberOfReviews,
        });

        return getProductsResponseDto;
    }

    public async Task ModifyProductInventory(ModifyProductInventoryRequestDto modifyProductInventoryRequestDto, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Product foundProduct;
        try
        {
            foundProduct = await _unitOfWork.ProductRepository.ReadAsync(modifyProductInventoryRequestDto.ProductId, cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error occurred while finding product with {id}", modifyProductInventoryRequestDto.ProductId);
            throw;
        }

        if (foundProduct is null)
        {
            throw new NotFoundException($"Product with id {modifyProductInventoryRequestDto.ProductId} not found!");
        }

        foundProduct.Inventory = modifyProductInventoryRequestDto.NewInventory;

        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            await _unitOfWork.ProductRepository.UpdateAsync(foundProduct, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error occurred while updating product with {id} to modify inventory", modifyProductInventoryRequestDto.ProductId);
            throw;
        }
    }

    public async Task DecrementProductInventory(string productId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Product foundProduct;
        try
        {
            foundProduct = await _unitOfWork.ProductRepository.ReadAsync(productId, cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error occurred while finding product with {id}", productId);
            throw;
        }

        if (foundProduct is null)
        {
            throw new NotFoundException($"Product with id {productId} not found!");
        }

        foundProduct.Inventory -= 1;

        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            await _unitOfWork.ProductRepository.UpdateAsync(foundProduct, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error occurred while updating product with {id} to modify inventory", productId);
            throw;
        }
    }

    public async Task IncrementProductInventory(string productId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Product foundProduct;
        try
        {
            foundProduct = await _unitOfWork.ProductRepository.ReadAsync(productId, cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error occurred while finding product with {id}", productId);
            throw;
        }

        if (foundProduct is null)
        {
            throw new NotFoundException($"Product with id {productId} not found!");
        }

        foundProduct.Inventory += 1;

        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            await _unitOfWork.ProductRepository.UpdateAsync(foundProduct, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error occurred while updating product with {id} to modify inventory", productId);
            throw;
        }
    }

    private void ValidateCreateProductRequest(CreateProductRequestDto createProductRequestDto)
    {
        List<Exception> exceptions = new List<Exception>();

        if (string.IsNullOrWhiteSpace(createProductRequestDto.Name))
        {
            exceptions.Add(new BadRequestException("Product name cannot be empty"));
        }

        if (string.IsNullOrWhiteSpace(createProductRequestDto.Description))
        {
            exceptions.Add(new BadRequestException("Product description cannot be empty"));
        }

        if (createProductRequestDto.Price < 0.0)
        {
            exceptions.Add(new BadRequestException("Product price cannot be negative"));
        }

        if (exceptions.Any())
        {
            _logger.LogWarning("Invalid CreateProductRequestDto detected. Throwing...");
            throw new AggregateException(exceptions);
        }
    }
}