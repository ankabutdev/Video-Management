namespace VideoManagement.API.Exceptions;

public class VideoNotFoundException : NotFoundException
{
    public VideoNotFoundException()
    {
        TitleMessage = "Video not found!";
    }
}
