using System.Text;
using System.Text.RegularExpressions;

namespace pdfGenerator.Helpers
{
    public static class FileHelper
    {
        public static IFormFile? GetIndexFile(IEnumerable<IFormFile> files)
        {
            if (files.Count() == 0)
                return null!;

            return files.FirstOrDefault(f => f.FileName.ToLower().Contains("index.html"));
        }
        
        public static string GetStyleFrom(IEnumerable<IFormFile> files, string fileName)
        {
            var file = files.FirstOrDefault(f => f.FileName.ToLower().Contains(fileName.ToLower()));
            if (file == default)
                return string.Empty;

            using (var ms = new MemoryStream())
            {
                using (var rs = file.OpenReadStream())
                {
                    rs.CopyTo(ms);
                    var css = Encoding.ASCII.GetString(ms.ToArray());
                    var result = RemoveCssComments(RemoveNewlineSymbols(css));

                    return result;
                }
            }
        }

        private static string RemoveCssComments(string input)
        {
            return Regex.Replace(
                        input,
                        @"\/\*((.|\n)[^*])\*\/",
                        string.Empty);
        }

        private static string RemoveNewlineSymbols(string input)
        {
            return input.Replace("\r\n", string.Empty);
        }
    }
}