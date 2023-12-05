using EasyWord.Library.Services.Impl;
using EasyWord.Library.Services;

namespace EasyWord.Services;

public class PhotoService : IPhotoService
{
    private readonly IAlertService _alertService;

    public PhotoService(IAlertService alertService)
    {
        _alertService = alertService;
    }

    public async Task<byte[]> CaptureAsync()
    {
        if (!MediaPicker.Default.IsCaptureSupported)
        {
            await _alertService.AlertAsync(ErrorMessages.PhotoErrorTitle,
                "设备不支持拍照。", ErrorMessages.PhotoErrorButton);
            return null;
        }

        var photo = await MediaPicker.Default.CapturePhotoAsync();

        if (photo is null)
        {
            await _alertService.AlertAsync(ErrorMessages.PhotoErrorTitle,
                "未能拍摄照片。", ErrorMessages.PhotoErrorButton);
            return null;
        }

        await using var photoStream = await photo.OpenReadAsync();
        using var memoryStream = new MemoryStream();
        await photoStream.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }

    public async Task<byte[]> PickAsync()
    {
        var photo = await MediaPicker.Default.PickPhotoAsync();

        if (photo is null)
        {
            await _alertService.AlertAsync(ErrorMessages.PhotoErrorTitle,
                "未能选择照片。", ErrorMessages.PhotoErrorButton);
            return null;
        }

        await using var photoStream = await photo.OpenReadAsync();
        using var memoryStream = new MemoryStream();
        await photoStream.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }
}