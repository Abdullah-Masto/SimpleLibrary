using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimpleLibrary.Services;
using System.Threading.Tasks;

namespace SimpleLibrary.Controllers
{
    public class StackOverflowController : Controller
    {
        private readonly StackOverflowService _stackOverflowService;
        private readonly ILogger<StackOverflowController> _logger;

        public StackOverflowController(StackOverflowService stackOverflowService, ILogger<StackOverflowController> logger)
        {
            _stackOverflowService = stackOverflowService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var questions = await _stackOverflowService.GetRecentQuestionsAsync();
            return View(questions);
        }

        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning($"Invalid question ID: {id}");
                return BadRequest("Invalid question ID.");
            }

            _logger.LogInformation($"Fetching details for question ID: {id}");
            var question = await _stackOverflowService.GetQuestionDetailsAsync(id);

            if (question == null)
            {
                _logger.LogWarning($"No details found for question ID: {id}");
                return NotFound();
            }

            _logger.LogInformation($"Question Title: {question.Title}");
            return View(question);
        }
    }
}
