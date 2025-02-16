﻿using Azure.Storage;
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
    private readonly string MEDIA = "media";
    private readonly string VIDEOS = "videos";
    private readonly string ROOTHPATH;

    public ProductService(IProductRepository repository, IWebHostEnvironment env)
    {
        _repository = repository;
        var credential = new StorageSharedKeyCredential(_storageAccount, _key);
        var blobUri = $"https://{_storageAccount}.blob.core.windows.net";
        var blobServiceClient = new BlobServiceClient(new Uri(blobUri), credential);
        _filesContainer = blobServiceClient.GetBlobContainerClient("task");
        ROOTHPATH = env.WebRootPath;

    }

    public async Task<bool> CreateAsync(ProductCreateDto dto)
    {
        Product product = new Product();
        product.Name = dto.Name;
        product.Description = dto.Description;

        string videoName = MediaHelper.MakeVideoName(dto.Video.FileName);

        // this field for video service
        product.VideoUrl = await Task.Run(async () =>
        {
            return await this.UploadVideoAsync(dto.Video, videoName);
        });

        FileInfo VideoInfo = new FileInfo(dto.Video.FileName);
        string subPathVideo = Path.Combine(MEDIA, VIDEOS, videoName);
        string path = Path.Combine(ROOTHPATH, subPathVideo);

        var stream = new FileStream(path, FileMode.Create);
        await dto.Video.CopyToAsync(stream);
        stream.Dispose();
        stream.Close();

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

            string videoName = MediaHelper.MakeVideoName(dto.Video.FileName);

            product.VideoUrl = await UploadVideoAsync(dto.Video, videoName);
        }

        var result = await _repository.UpdateAsync(product);
        return result > 0;
    }

    private async Task<string> UploadVideoAsync(IFormFile Video, string videoName)
    {

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

    public async Task<Product> GetVideoBySortNumberAsync(int sortNumber)
    {
        var result = await _repository.GetVideoUrlBySortNumberAsync(sortNumber);
        if (result == null) throw new VideoNotFoundException();
        return result;
    }
}
