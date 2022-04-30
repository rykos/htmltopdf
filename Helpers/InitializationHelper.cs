namespace pdfGenerator.Services
{
    public static class InitializationHelper
    {
        public static void AddCustomServices(IServiceCollection services)
        {
            services.AddScoped<IPdfCreatorService, PdfCreatorService>();
        }
    }
}