using OCRMultiline.Common;
using OCRMultiline.Services;
using Microsoft.AspNetCore.Mvc;

namespace OCRMultiline.Endpoints.OCRApi
{
    public class OCRController : BaseController
    {
        private readonly OCRService _ocrService;

        public OCRController(OCRService ocrService)
        {
            _ocrService = ocrService;
        }

        [HttpPost("ocr-2")]
        public async Task<IActionResult> PerformOCRAzure([FromBody] OCRRequestModel body)
        {
            if (body.url == null)
            {
                return BadRequest("No specific URL provided");
            }
            var extractedText = await _ocrService.ConvertImageToText(body.url);
            return Ok(new { ExtractedText = extractedText });
        }

        [HttpPost("ocr-rewrite")]
        public async Task<IActionResult> RewriteOCR([FromBody]FlowiseAIRequestModel data)
        {
            if (data.Question == null)
            {
                return BadRequest("No Data");
            }

            try
            {
                var result = await _ocrService.RewriteOCRAsync(data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("data-to-json")]
        public async Task<IActionResult> DatatoJSON([FromBody]FlowiseAIRequestModel data)
        {
            if (data.Question == null)
            {
                return BadRequest("No Data");
            }

            try
            {
                var result = await _ocrService.DatatoJSONAsync(data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("ocr-all")]
        public async Task<IActionResult> PerformOCRAll([FromBody] OCRRequestModel body)
        {
            if (body.url == null)
            {
                return BadRequest("No specific URL provided");
            }

            List<string> extractedText;
            try {
                extractedText = await _ocrService.ConvertImageToText(body.url);
            }catch{
                return BadRequest("Error during OCR image");
            }
            FlowiseAIRequestModel data = new FlowiseAIRequestModel();
            data.Question = extractedText.ToArray();
            if (data.Question == null)
            {
                return BadRequest("No Data");
            }

            try
            {
                var resultRewrite = await _ocrService.RewriteOCRAsync(data);
                FlowiseAIRequestModel dataRewrite = new FlowiseAIRequestModel();
                dataRewrite.Question = resultRewrite.ToArray();
                var resultJson = await _ocrService.DatatoJSONAsync(dataRewrite);
                return Ok(resultJson);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
            
        }
    }
}
