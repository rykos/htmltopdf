using pdfGenerator.Helpers;
using pdfGenerator.Models;
using SelectPdf;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace pdfGenerator.Services
{
    public class PdfCreatorService : IPdfCreatorService
    {
        private HtmlToPdf converter = default!;

        public async Task<byte[]> CreatePdfFromRequest(CreatePdfRequest request)
        {
            converter = CreatePdfConverter(request.Options);
            if (request.HtmlFiles != default)
                return await PdfFromHtmlFile(request.HtmlFiles);
            else if (request.Url != default)
                return await PdfFromUrl(request.Url);
            else
                throw new NotImplementedException();
        }

        private async Task<byte[]> PdfFromHtmlFile(IEnumerable<IFormFile> files)
        {
            var htmlFile = FileHelper.GetIndexFile(files);
            if (htmlFile == default || htmlFile.ContentType != "text/html")
                return null!;

            using (var readStream = htmlFile.OpenReadStream())
            {
                byte[] buffer = new byte[readStream.Length];
                await readStream.ReadAsync(buffer, 0, (int)readStream.Length);

                var htmlLines = Encoding.UTF8.GetString(buffer).Split("\n").ToList();

                for (int i = 0; i < htmlLines.Count(); i++)
                {
                    var match = Regex.Match(htmlLines[i], @"(?<=href=)""([^""]*)""", RegexOptions.IgnoreCase);
                    if (match.Groups[1].Value != "")
                    {
                        var style = FileHelper.GetStyleFrom(files, match.Groups[1].Value);
                        if (style != string.Empty)
                            htmlLines[i] = $"<style>{FileHelper.GetStyleFrom(files, match.Groups[1].Value)}</style>";
                    }
                }
                string htmlRes = String.Concat(htmlLines);
                Console.WriteLine(htmlRes);

                var PdfDocument = converter.ConvertHtmlString(htmlRes);
                using (var memoryStream = new MemoryStream())
                {
                    PdfDocument.Save(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }

        private async Task<byte[]> PdfFromUrl(string url)
        {
            PdfDocument doc = default!;
            await Task.Run(() =>
            {
                try
                {
                    doc = converter.ConvertUrl(url);
                }
                catch { }
            });

            if (doc == default)
                return null!;

            using (var memoryStream = new MemoryStream())
            {
                doc.Save(memoryStream);
                doc.Close();

                return memoryStream.ToArray();
            }
        }

        private HtmlToPdf CreatePdfConverter(CreatePdfOptions? options)
        {
            HtmlToPdf converter = new HtmlToPdf(612, 792);

            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
            converter.Options.PdfPageSize = PdfPageSize.A4;

            converter.Options.MarginBottom = 0;
            converter.Options.MarginLeft = 0;
            converter.Options.MarginRight = 0;
            converter.Options.MarginTop = 0;

            converter.Options.AutoFitWidth = HtmlToPdfPageFitMode.AutoFit;
            converter.Options.AutoFitHeight = HtmlToPdfPageFitMode.NoAdjustment;

            converter.Options.AllowContentHeightResize = false;

            return converter;
        }
    }

    public interface IPdfCreatorService
    {
        Task<byte[]> CreatePdfFromRequest(CreatePdfRequest request);
    }
}