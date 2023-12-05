namespace EasyWord.Library.Services;

public interface IPhotoService
{
    Task<byte[]> CaptureAsync();

    Task<byte[]> PickAsync();
}