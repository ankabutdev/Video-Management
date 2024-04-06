using Azure.Storage.Blobs;

namespace VideoManagement.API.Services;

public class BlobAccessService
{
    protected readonly string _storageAccount = "cloudshell927410588";
    protected readonly string _key = "";

    protected BlobContainerClient _filesContainer;
}
