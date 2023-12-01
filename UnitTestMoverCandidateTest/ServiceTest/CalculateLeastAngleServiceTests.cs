using MoverCandidateTest.WatchHands.Service;

namespace UnitTestMoverCandidateTest.ServiceTest
{
    [TestFixture]
    public class CalculateLeastAngleServiceTests
    {
        private CalculateLeastAngleService _calculateLeastAngleService;

        [SetUp]
        public void SetUp()
        {
            _calculateLeastAngleService = new CalculateLeastAngleService();
        }

        [Test]
        public void CalculateLeastAngle_ReturnsExpectedAngle()
        {
            // Arrange
            var dateTime = new DateTime(2023, 12, 12, 6, 30, 0);

            // Act
            var result = _calculateLeastAngleService.CalculateLeastAngle(dateTime);

            // Assert
            Assert.That(result, Is.EqualTo(15));
        }

        [Test]
        public void CalculateLeastAngle_WithDifferentTime_ReturnsExpectedAngle()
        {
            // Arrange
            var dateTimeAm = new DateTime(2023, 12, 12,9, 0, 0);
            var dateTimePm = new DateTime(2013, 6, 1,21, 0, 0);
            

            // Act
            var resultAm = _calculateLeastAngleService.CalculateLeastAngle(dateTimeAm);
            var resultPm = _calculateLeastAngleService.CalculateLeastAngle(dateTimePm);
            
            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(resultAm, Is.EqualTo(90.0));
                Assert.That(resultPm, Is.EqualTo(resultAm));
            });
        }
    }
}