using Azure.Storage;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using VideoManagement.API.Dtos;
using VideoManagement.API.Entities;
using VideoManagement.API.Exceptions;
using VideoManagement.API.Helpers;
using VideoManagement.API.Repository;

namespace VideoManagement.API.Services;

public class ProductService : BlobAccessService, IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
        var credential = new StorageSharedKeyCredential(_storageAccount, _key);
        var blobUri = $"https://{_storageAccount}.blob.core.windows.net";
        var blobServiceClient = new BlobServiceClient(new Uri(blobUri), credential);
        _filesContainer = blobServiceClient.GetBlobContainerClient("task");

    }

    public async Task<bool> CreateAsync(ProductCreateDto dto)
    {
        Product product = new Product();
        product.Name = dto.Name;
        product.Description = dto.Description;

        // this field for video service
        product.VideoUrl = await this.UploadVideoAsync(dto.Video);

        var result = await _repository.CreateAsync(product);
        return result > 0;

    }

    public async Task<bool> DeleteAsync(int Id)
    {
        var product = await _repository.GetByIdAsync(Id);
        if (product is null) throw new VideoNotFoundException();

        // this field delete for video
        var deleteVideo = await this.DeleteVideoAsync(product
            .VideoUrl
            .Substring(product.VideoUrl
            .IndexOf("VIDEO")));

        if (!deleteVideo.Error is false)
            throw new VideoNotFoundException();

        var result = await _repository.DeleteAsync(product);
        return result > 0;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        var products = await _repository.GetAllAsync();
        if (products is null) throw new ProductNotFoundException();
        return products;
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        var exists = await _repository.GetByIdAsync(id);
        if (exists is null) throw new ProductNotFoundException();
        return exists;
    }

    public async Task<IQueryable<Product>> SearchAsync(string query)
    {
        return await _repository.SearchAsync(query.ToLower());
    }

    public async Task<bool> UpdateAsync(int Id, [FromForm] ProductUpdateDto dto)
    {
        var product = await _repository.GetByIdAsync(Id);
        if (product is null) throw new ProductNotFoundException();

        product.Name = dto.Name;
        product.Description = dto.Description;

        if (dto.Video is not null)
        {
            // this field for video
            var deleteVideo = await this.DeleteVideoAsync(product
                .VideoUrl
                .Substring(product.VideoUrl
                .IndexOf("VIDEO")));

            if (!deleteVideo.Error is false)
                throw new VideoNotFoundException();

            product.VideoUrl = await UploadVideoAsync(dto.Video);
        }

        var result = await _repository.UpdateAsync(product);
        return result > 0;
    }

    private async Task<string> UploadVideoAsync(IFormFile Video)
    {
        string videoName = MediaHelper.MakeVideoName(Video.FileName);

        //BlobUploadOptions videoUploadOptions = new BlobUploadOptions
        //{
        //    HttpHeaders = new BlobHttpHeaders
        //    {
        //        ContentType = Video.ContentType
        //    }
        //};

        BlobClient videoUpload = _filesContainer.GetBlobClient(videoName);
        await using (Stream streamVideo = Video.OpenReadStream())
        {
            await videoUpload.UploadAsync(streamVideo);
        }

        return videoUpload.Uri.AbsoluteUri;
    }

    private async Task<BlobResponseDto> DeleteVideoAsync(string blobFilename)
    {
        BlobClient file = _filesContainer.GetBlobClient(blobFilename);

        await file.DeleteAsync();

        return new BlobResponseDto
        {
            Error = false,
            Status = $"File: {blobFilename} has been successfully  deleted."
        };
    }

    private async Task<Stream?> DownloadVideoAsync(string blobfilename)
    {
        BlobClient file = _filesContainer.GetBlobClient(blobfilename);

        if (await file.ExistsAsync())
        {
            var data = await file.OpenReadAsync();
            Stream blobContent = data;

            var content = await file.DownloadContentAsync();

            return blobContent;
        }

        return null;
    }
}
