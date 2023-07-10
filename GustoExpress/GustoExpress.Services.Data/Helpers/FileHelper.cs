namespace GustoExpress.Services.Data.Helpers
{
    public static class FileHelper
    {
        public static void DeleteImage(string imagePath)
        {
            FileInfo fileInfo = new FileInfo(imagePath);
            if (fileInfo != null)
            {
                System.IO.File.Delete(imagePath);
                fileInfo.Delete();
            }
        }
    }
}
