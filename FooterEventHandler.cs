using iText.Kernel.Events;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;


namespace ITextSharpPOC
{
    public class FooterEventHandler : IEventHandler
    {
        public void HandleEvent(Event @event)
        {
            PdfDocumentEvent docEvent = (PdfDocumentEvent)@event;
            PdfDocument pdfDoc = docEvent.GetDocument();
            PdfPage page = docEvent.GetPage();
            int pageNumber = pdfDoc.GetPageNumber(page);
            Rectangle pageSize = page.GetPageSize();
            PdfCanvas pdfCanvas = new PdfCanvas(page.NewContentStreamBefore(), page.GetResources(), pdfDoc);
            pdfCanvas
                .BeginText()
                .SetFontAndSize(pdfDoc.GetDefaultFont(), 12)
                .MoveText(pageSize.GetWidth() / 2 - 20, 20)
                .ShowText("Page " + pageNumber)
                .EndText();
            pdfCanvas.Release();
        }
    }
}
