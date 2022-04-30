namespace pdfGenerator.Models
{
    public record CreatePdfRequest(
        IEnumerable<IFormFile>? HtmlFiles,
        string? Url,
        CreatePdfOptions? Options
    );
}