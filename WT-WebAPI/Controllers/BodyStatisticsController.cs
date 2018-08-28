using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WT_WebAPI.Common;
using WT_WebAPI.Entities.DTO.Requests;
using WT_WebAPI.Entities.DTO.WorkoutAssets;
using WT_WebAPI.Entities.DTO.WorkoutProgress;
using WT_WebAPI.Entities.WorkoutAssets;
using WT_WebAPI.Entities.WorkoutProgress;
using WT_WebAPI.Repository.Interfaces;


namespace WT_WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/BodyStatistics/")]
    public class BodyStatisticsController : Controller
    {
        private readonly ICommonRepository _repository;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<BodyStatisticsController> _logger;

        public BodyStatisticsController(ICommonRepository repository, IHostingEnvironment hostingEnvironment, ILogger<BodyStatisticsController> logger)
        {
            _repository = repository;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }


        [HttpGet("user/{userId}", Name = "GetBodyStatistics")]
        public async Task<IActionResult> GetBodyStatistics([FromRoute]int? userId)
        {
            if (userId == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var bodyStats = await _repository.GetBodyStatisticsForUser(userId);

            if (bodyStats == null)
            {
                return NotFound("Body Statistics not found");
            }

            var mappedBodyStats = Mapper.Map<IEnumerable<BodyStatisticDTO>>(bodyStats);

            return Ok(mappedBodyStats);
        }

        [HttpGet("user/{userId}/BodyStat/{bodyStatId}", Name = "GetBodyStatistic")]
        public async Task<IActionResult> GetBodyStatistic([FromRoute]int? userId, [FromRoute]int? bodyStatId)
        {
            if (userId == null || bodyStatId == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var bodyStat = await _repository.GetBodyStatistic(userId, bodyStatId);

            if (bodyStat == null)
            {
                return NotFound("Body Statistic not found");
            }

            var mappedBodyStat = Mapper.Map<BodyStatisticDTO>(bodyStat);

            return Ok(mappedBodyStat);
        }

        [HttpGet("user/{userId}/BodyStat/ForMonth/{month}/year/{year}", Name = "GetBodyStatisticsForMonth")]
        public async Task<IActionResult> GetBodyStatisticsForMonth([FromRoute]int? userId, [FromRoute]int? month, [FromRoute]int? year)
        {
            if (userId == null || month == null || year == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var bodyStats = await _repository.GetBodyStatisticForMonth(userId, month, year);

            if (bodyStats == null)
            {
                return NotFound("Body Statistics not found");
            }

            var mappedBodyStat = Mapper.Map<IEnumerable<BodyStatisticDTO>>(bodyStats);

            return Ok(mappedBodyStat);
        }

        [HttpPost("user/{userId}/BodyStat", Name = "AddOrUpdateBodyStatistic")]
        public async Task<IActionResult> AddOrUpdateBodyStatistic([FromRoute]int? userId, [FromBody] BodyStatisticDTO bodyStatDto)
        {
            if (userId == null)
            {
                return BadRequest();
            }

            if (bodyStatDto.Month <= 0 || bodyStatDto.Month > 12 || bodyStatDto.Week <= 0 || bodyStatDto.Week > 52 || bodyStatDto.Year < 0)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var bodyStatEntity = Mapper.Map<BodyStatistic>(bodyStatDto);

            var result = await _repository.AddOrUpdateBodyStatistic(userId, bodyStatEntity);

            if (result == null)
            {
                return BadRequest("Add Failed for Body Statistic...");
            }

            if (bodyStatEntity.ImageBytes != null && bodyStatEntity.ImageBytes.Length != 0)
            {
                bodyStatEntity.ID = result.ID;
                var imageResult = await SaveStatisticImage(bodyStatEntity);
            }

            var bodyStatToReturn = Mapper.Map<BodyStatisticDTO>(result);

            return CreatedAtRoute(
                            routeName: "GetBodyStatistic",
                            routeValues: new
                            {
                                userId = userId,
                                bodyStatId = bodyStatToReturn.ID
                            },
                            value: bodyStatToReturn);
        }

        [HttpDelete("user/{userId}/BodyStat/{bodyStatId}", Name = "DeleteBodyStatistic")]
        public async Task<IActionResult> DeleteBodyStatistic([FromRoute] int? userId, [FromRoute] int? bodyStatId)
        {
            if (userId == null || bodyStatId == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var result = await _repository.DeleteBodyStatistic(userId, bodyStatId);

            if (result == false)
            {
                return NotFound();
            }

            return NoContent();
        }



        [HttpGet("user/{userId}/AttributeTemplates/", Name = "GetAttributeTemplates")]
        public async Task<IActionResult> GetAttributeTemplates([FromRoute]int? userId)
        {
            if (userId == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var attributes = await _repository.GetAttributeTemplatesForUser(userId);

            if (attributes == null)
            {
                return NotFound("Attributes not found");
            }

            var mappedAttributes = Mapper.Map<IEnumerable<BodyAttributeTemplateDTO>>(attributes);

            return Ok(mappedAttributes);
        }

        [HttpPut("user/{userId}/AttributeTemplates", Name = "UpdateAttributeTemplates")]
        public async Task<IActionResult> UpdateAttributeTemplates([FromRoute]int? userId, [FromBody] BodyStatisticsRequest bodyStatRequest)
        {
            if (userId == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var bodyAttributeTemplatesEntity = Mapper.Map<IEnumerable<BodyAttributeTemplate>>(bodyStatRequest.BodyAttributeTemplates);

            var result = await _repository.UpdateAttributeTemplates(userId, bodyAttributeTemplatesEntity.ToList());

            if (result == null)
            {
                return BadRequest("Add Failed for Template Attributes...");
            }

            var bodyAttributeTemplatesToreturn = Mapper.Map<IEnumerable<BodyAttributeTemplateDTO>>(result);

            return CreatedAtRoute(
                            routeName: "GetAttributeTemplates",
                            routeValues: new
                            {
                                userId = userId,
                            },
                            value: bodyAttributeTemplatesToreturn);

        }



        private async Task<bool> SaveStatisticImage(BodyStatistic statisticEntity)
        {
            try
            {
                // get this environment's web root path (the path
                // from which static content, like an image, is served)
                var webRootPath = _hostingEnvironment.WebRootPath;
                string folderName = "Images/Statistics/" + statisticEntity.WTUserID.ToString();
                string fullFolderPath = $"{webRootPath}/{folderName}/";

                // create path if not exists... write bytes and auto-close stream
                FileInfo file = new FileInfo(fullFolderPath);
                file.Directory.Create();

                // create the filename
                string imageName = statisticEntity.ImagePath;
                var imageExtension = imageName.Substring(imageName.LastIndexOf("."));

                string fileName = "statistic_" + statisticEntity.ID + "_" + Guid.NewGuid() + imageExtension;

                //delete previuos image
                string[] fileList = Directory.GetFiles(fullFolderPath, $"*statistic_{statisticEntity.ID}*");
                foreach (var fileToDelete in fileList)
                {
                    System.IO.File.Delete(fileToDelete);
                }

                // the full file path
                var filePath = Path.Combine($"{fullFolderPath}/{fileName}");

                System.IO.File.WriteAllBytes(filePath, statisticEntity.ImageBytes);

                // fill out the filename
                statisticEntity.ImagePath = folderName + "/" + fileName;

                await _repository.UpdateImageForStatistic(statisticEntity.ID, statisticEntity.ImagePath);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(500, ex, ex.Message);
                return false;
            }

        }




    }
}