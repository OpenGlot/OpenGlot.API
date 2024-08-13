using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PolyglotAPI.Data.Models;
using PolyglotAPI.Data.Repos;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace PolyglotAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly IFilesRepository _filesRepository;
        private readonly ILogger<FilesController> _logger;

        public FilesController(IFilesRepository filesRepository, ILogger<FilesController> logger)
        {
            _filesRepository = filesRepository;
            _logger = logger;
        }

        [HttpPost("upload-images")]
        public async Task<IActionResult> UploadImages(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File not selected");

            CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture)
            { HasHeaderRecord = true };

            var images = new List<Image>();

            using (var stream = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(stream, config))
            {
                var records = csv.GetRecords<dynamic>();
                foreach (var record in records)
                {
                    images.Add(new Image
                    {
                        Id = int.Parse(record.image_id),
                        Context = record.context,
                        Description = record.original_description,
                        EnhancedDescription = record.enhanced_description,
                        UrlKey = record.file_name
                    });
                }
            }

            await _filesRepository.UploadImagesAsync(images);
            return Ok("Images uploaded successfully");
        }

        [HttpPost("upload-audio")]
        public async Task<IActionResult> UploadAudio(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File not selected");

            CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture)
            { HasHeaderRecord = true };

            var audios = new List<Audio>();

            using (var stream = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(stream, config))
            {
                var records = csv.GetRecords<dynamic>();
                foreach (var record in records)
                {
                    audios.Add(new Audio
                    {
                        //Id = int.Parse(record.sentence_id),

                        SentenceId = record.sentence_id,
                        EnglishTranslation = "",
                        LanguageCode = record.language,
                        Transcript = record.sentence,
                        UrlKey = record.file_name,
                        UploadedAt = DateTime.Now.ToUniversalTime()
                    });
                }
            }

            await _filesRepository.UploadAudioAsync(audios);
            return Ok("Audio files uploaded successfully");
        }

        [HttpGet("audios")]
        public async Task<IActionResult> GetAllAudios()
        {
            var audios = await _filesRepository.GetAllAudiosAsync();
            return Ok(audios);
        }

        [HttpGet("images")]
        public async Task<IActionResult> GetAllImages()
        {
            var images = await _filesRepository.GetAllImagesAsync();
            return Ok(images);
        }

        [HttpGet("search/audios/{search}")]
        public async Task<IActionResult> GetAllAudios(string search)
        {
            var audios = await _filesRepository.GetAllAudiosAsync();
            return Ok(audios);
        }

        [HttpGet("search/images/{search}")]
        public async Task<IActionResult> GetImagesSearch(string search)
        {
            var images = await _filesRepository.GetAllImagesAsync();
            return Ok(images);
        }

        [HttpGet("audio/{id}")]
        public async Task<IActionResult> GetAudioById(int id)
        {
            var audio = await _filesRepository.GetAudioByIdAsync(id);
            if (audio == null)
                return NotFound();

            return Ok(audio);
        }

        [HttpGet("image/{id}")]
        public async Task<IActionResult> GetImageById(int id)
        {
            var image = await _filesRepository.GetImageByIdAsync(id);
            if (image == null)
                return NotFound();

            return Ok(image);
        }
    }
}