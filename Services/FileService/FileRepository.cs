using HMS.Models;

namespace HMS.Services.FileService
{
    public class FileRepository : ImageRepository
    {
        private readonly HMSDBContext _context;
        public FileRepository(HMSDBContext context)
        {
            this._context = context;
        }
        public bool Add(ImageUpload ImageUpload)
        {
            try
            {
                _context.ImageUploads.Add(ImageUpload);
                _context.SaveChanges();
                return true;

            }
            catch (Exception ex)
            {
                return false;
                //handle this exception gracefully
            }
        }
    }
}
