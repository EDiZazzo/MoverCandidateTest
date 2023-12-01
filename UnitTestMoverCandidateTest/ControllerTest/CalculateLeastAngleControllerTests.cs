using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using MoverCandidateTest.WatchHands.Controller;
using MoverCandidateTest.WatchHands.Model;
using MoverCandidateTest.WatchHands.Service;

namespace UnitTestMoverCandidateTest.ControllerTest
{
    [TestFixture]
    public class CalculateLeastAngleControllerTests
    {
        private CalculateLeastAngleController _calculateLeastAngleController;
        private Mock<ILogger<CalculateLeastAngleController>> _loggerMock;
        private Mock<ICalculateLeastAngleService> _calculateLeastAngleServiceMock;
        private Mock<IValidator<CalculateLeastAngleRequestModel>> _validatorMock;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<CalculateLeastAngleController>>();
            _calculateLeastAngleServiceMock = new Mock<ICalculateLeastAngleService>();
            _validatorMock = new Mock<IValidator<CalculateLeastAngleRequestModel>>();

            _calculateLeastAngleController = new CalculateLeastAngleController(
                _loggerMock.Object,
                _calculateLeastAngleServiceMock.Object,
                _validatorMock.Object
            );
        }

        [Test]
        public void CalculateLeastAngle_WhenValidInput_ReturnsOkResult()
        {
            // Arrange
            var validRequest = new CalculateLeastAngleRequestModel(new DateTime(1, 1, 1, 0,30,0));
            var leastAngle = 172.5; // Replace this with the expected result

            _validatorMock.Setup(validator => validator.Validate(validRequest))
                .Returns(new FluentValidation.Results.ValidationResult());

            _calculateLeastAngleServiceMock.Setup(service => service.CalculateLeastAngle(validRequest.DateTime))
                .Returns(leastAngle);

            // Act
            var result = _calculateLeastAngleController.CalculateLeastAngle(validRequest) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.IsNotNull(result.Value);
            Assert.IsInstanceOf<CalculateLeastAngleResponseModel>(result.Value);
            Assert.That(((CalculateLeastAngleResponseModel)result.Value).LeastAngle, Is.EqualTo(leastAngle)); // Verify the result
        }

        [Test]
        public void CalculateLeastAngle_WhenInvalidInput_ReturnsBadRequest()
        {
            // Arrange
            var invalidRequest = new CalculateLeastAngleRequestModel(new DateTime()); // Invalid model without DateTime
            var validationErrors = new FluentValidation.Results.ValidationResult();
            validationErrors.Errors.Add(
                new FluentValidation.Results.ValidationFailure("DateTime", "DateTime is required."));

            _validatorMock.Setup(validator => validator.Validate(invalidRequest))
                .Returns(validationErrors);

            // Act
            var result = _calculateLeastAngleController.CalculateLeastAngle(invalidRequest) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
            Assert.IsNotNull(result.Value);
        }
    }
}
