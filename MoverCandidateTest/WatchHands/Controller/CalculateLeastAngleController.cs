using System;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MoverCandidateTest.WatchHands.Model;
using MoverCandidateTest.WatchHands.Service;

namespace MoverCandidateTest.WatchHands.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class CalculateLeastAngleController : ControllerBase
    {
        private readonly ILogger<CalculateLeastAngleController> _logger;
        private readonly ICalculateLeastAngleService _calculateLeastAngleService;
        private readonly IValidator<CalculateLeastAngleRequestModel> _validator;

        public CalculateLeastAngleController(
            ILogger<CalculateLeastAngleController> logger,
            ICalculateLeastAngleService calculateLeastAngleService,
            IValidator<CalculateLeastAngleRequestModel> validator)
        {
            _logger = logger ?? throw new ArgumentNullException();
            _calculateLeastAngleService = calculateLeastAngleService ?? throw new ArgumentNullException();
            _validator = validator ?? throw new ArgumentNullException();
        }

        [HttpGet("least-angle")]
        public IActionResult CalculateLeastAngle([FromQuery] CalculateLeastAngleRequestModel requestModel)
        {
            var validationResult = _validator.Validate(requestModel);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            // Calculate the least angle
            var leastAngle = _calculateLeastAngleService.CalculateLeastAngle(
                requestModel.DateTime);

            // Return the result
            return Ok(new CalculateLeastAngleResponseModel(leastAngle));
        }
    }
}