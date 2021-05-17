using System.IO;

namespace LaundryTime.Utilities.UtilityModels
{
    public interface IReport
    {
        public string Format { get; set; }

        public string FileName { get; set; }

        public MemoryStream Content { get; set; }
    }
}
