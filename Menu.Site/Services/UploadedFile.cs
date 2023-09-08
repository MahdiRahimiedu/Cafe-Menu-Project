namespace Menu.Site.Services
{
    public static class UploadedFile
    {
        public static bool CreateFileImage(IFormFile model, string nameFile ,IWebHostEnvironment webHostEnvironment)
        {
            if (model != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                string uniqueFileName = nameFile;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.CopyTo(fileStream);
                }

                return true;
            }
            return false;
        }
        public static string CreateNameFileImage(IFormFile model)
        {
            return Guid.NewGuid().ToString() + "_" + model.FileName;
        }
        public static bool DeleteFileImage(string nameFile, IWebHostEnvironment webHostEnvironment)
        {
            var imagePath = webHostEnvironment.WebRootPath;
            imagePath += "\\images\\" + nameFile;
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
                return true;
            }
            return false;

        }
        
    }
}
