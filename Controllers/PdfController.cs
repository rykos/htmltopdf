using Microsoft.AspNetCore.Mvc;
using pdfGenerator.Models;
using pdfGenerator.Services;

namespace pdfGenerator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PdfController : ControllerBase
    {
        private readonly IPdfCreatorService pdfCreatorService;

        public PdfController(IPdfCreatorService pdfCreatorService)
        {
            this.pdfCreatorService = pdfCreatorService;
        }

        [HttpPost]
        public async Task<IActionResult> GetPdfFromUrl([FromForm] CreatePdfRequest request)
        {
            byte[] content = await pdfCreatorService.CreatePdfFromRequest(request);

            if(content == null)
                return BadRequest();
                
            return File(content,
                        "application/pdf",
                        "generated.pdf");
        }
    }
}