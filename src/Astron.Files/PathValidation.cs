using System.IO;
using System.Text.RegularExpressions;

namespace Astron.Files
{
    public class PathValidation : IValidation<string>
    {
        private static readonly Regex _invalidPathCharsRegex;

        static PathValidation()
        {
            var invalidPathChars = new string(Path.GetInvalidPathChars());
            invalidPathChars += @"/?*";
            _invalidPathCharsRegex = new Regex($"[{Regex.Escape(invalidPathChars)}]", 
                RegexOptions.Compiled);
        }

        public bool IsValid(string path)
        {
            if (string.IsNullOrEmpty(path)) return false;

            return !_invalidPathCharsRegex.IsMatch(path);
        }
    }
}
