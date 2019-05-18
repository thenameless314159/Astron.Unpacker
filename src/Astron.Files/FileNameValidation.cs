using System.IO;
using System.Text.RegularExpressions;

namespace Astron.Files
{
    public class FileNameValidation : IValidation<string>
    {
        private static readonly Regex _invalidFileNameCharsRegex;
        private readonly string _fileExtension;

        static FileNameValidation()
        {
            var invalidFileNameChars = Regex.Escape(new string(Path.GetInvalidFileNameChars()));
            _invalidFileNameCharsRegex = new Regex($"[{invalidFileNameChars}]", 
                RegexOptions.Compiled);
        }

        public FileNameValidation() => _fileExtension = string.Empty;
        public FileNameValidation(string fileExtension) => _fileExtension = fileExtension;

        public bool IsValid(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return false;
            if (string.IsNullOrEmpty(_fileExtension)) return !_invalidFileNameCharsRegex.IsMatch(fileName);

            return !_invalidFileNameCharsRegex.IsMatch(fileName) && fileName.Contains(_fileExtension);
        }
    }
}
