using System.Drawing;
using System.Drawing.Imaging;
namespace Grocery_Server.Services;


public class ImageStorageService
{
    // 5 * 1024 * 1024 bytes (5MB)
    const long MAX_FILESIZE = 0x500000;
    readonly string userFilesPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Groceries-Server", "User Files");

    public bool IsValidProfilePicture(IFormFile file)
    {
        long size = file.Length;
        if (size > MAX_FILESIZE)
            return false;

        try
        {
            Image? image = Image.FromStream(file.OpenReadStream());
            Size dimensions = image.Size;

            return dimensions.Width == dimensions.Height
                && dimensions.Width < 4000
                && dimensions.Width > 50;
        }
        catch
        {
            return false;
        }
    }

    public bool IsValidRecipePicture(IFormFile file)
    {
        long size = file.Length;
        if (size > MAX_FILESIZE)
            return false;

        try
        {
            Image? image = Image.FromStream(file.OpenReadStream());
            Size dimensions = image.Size;

            return dimensions.Height < 5000
                && dimensions.Height > 50
                && dimensions.Width < 5000
                && dimensions.Width > 50;
        }
        catch
        {
            return false;
        }
    }

    public string SaveImage(IFormFile file)
    {
        string fileName = Guid.NewGuid().ToString() + ".jpeg";
        string fullPath = Path.Combine(userFilesPath, fileName);

        Image? image = Image.FromStream(file.OpenReadStream());
        EncoderParameters parameters = new EncoderParameters(1);
        EncoderParameter parameter = new EncoderParameter(Encoder.Quality, 30L);
        parameters.Param = [parameter];

        ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg) ?? throw new Exception("Couldn't find JPEG decoder");

        if (!Directory.Exists(userFilesPath))
            Directory.CreateDirectory(userFilesPath);
        using (FileStream stream = new(fullPath, FileMode.OpenOrCreate))
        {
            image.Save(stream, jpgEncoder, parameters);
        }
        return fileName;
    }

    public FileStream GetImage(string fileName)
    {
        string path = Path.Combine(userFilesPath, fileName);
        if (!File.Exists(path))
            throw new FileNotFoundException("Requested a non-existent image");
        return File.OpenRead(path);
    }

    public void DeleteImage(string fileName)
    {
        string dir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string fullPath = Path.Combine(dir, "User Files", fileName);
        if (File.Exists(fullPath))
            File.Delete(fullPath);
        else
            throw new FileNotFoundException($"File not found: {fullPath}");
    }
    private ImageCodecInfo? GetEncoder(ImageFormat format)
    {
        ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
        return codecs.FirstOrDefault(codec => codec.FormatID == format.Guid);
    }
}
