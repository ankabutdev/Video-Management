namespace VideoManagement.API.Helpers;

public static class MediaHelper
{
    public static string MakeVideoName(string filename)
    {
        FileInfo fileInfo = new FileInfo(filename);

        string extension = fileInfo.Extension;
        string name = "VIDEO_" + Guid.NewGuid() + extension;

        return name;
    }

    public static string[] GetVideoExtensions()
    {
        return new string[]
        {
            ".mp4",
            ".mkv",
            ".flv",
            ".mov",
            ".avi",
            ".wmv",
            ".MP4",
            ".MKV",
            ".FLV",
            ".MOV",
            ".AVI",
            ".WMV",
            // you can add more video extensions here
        };
    }
}
