using Microsoft.AspNetCore.Mvc;
using ReadingListApi.Services.ReadingList;
using ReadingListApi.Services.ReadingList.DTO;

namespace ReadingListApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReadingListController : ControllerBase
    {
        private readonly ILogger<ReadingListController> _logger;
        private readonly IReadingListService _readingListService;

        public ReadingListController(ILogger<ReadingListController> logger, IReadingListService readingListService)
        {
            _logger = logger;
            _readingListService = readingListService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReadingItemAsync(ReadingItemCreateDTO readingItem)
        {
            try
            {
                return Ok(await _readingListService.CreateReadingItemAsync(readingItem));
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while creating a reading item: {Exception}", new { Exception = ex });
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating a reading item");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ListReadingItemsAsync()
        {
            try
            {
                return Ok(await _readingListService.ListReadingItemsAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while listing all the reading items: {Exception}", new { Exception = ex });
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while listing all the reading items");
            }
        }

        [HttpGet("{id}/screenshot")]
        public async Task<IActionResult> GetScreenshotAsync(Guid id)
        {
            try
            {
                var file = await _readingListService.GetReadingItemScreenshotAsync(id);

                if (file == null)
                    return NotFound();

                return File(file, "image/png");
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while retrieving the screenshot file: {Exception}", new { Exception = ex });
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the screenshot file");
            }
        }
    }
}