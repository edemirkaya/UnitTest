using JobAppLib.Common;
using JobAppLib.Models;
using JobAppLib.Services;
using Moq;

namespace JobAppLib.UnitTest
{
    public class AppEvaluateUnitTest
    {
        [Test]
        public void CheckApplication_CompareAgebyMinAge_TransferedToAutoRejected()
        {
            //Arrange
            var mockValidator = new Mock<IIdentityValidator>();
            var evaluator = new AppEvaluator(mockValidator.Object);
            var form = new JobApplications
            {
                Applicant = new Applicant
                {
                    Age = 17
                }
            };
            //Action
            var appResult = evaluator.Evaluate(form);

            Assert.AreEqual(appResult,ApplicationResult.AutoRejected);
        }


        [Test]
        public void CheckApplication_WithNoTechStackList_TransferedToAutoRejected()
        {
            //Arrange
            var mockValidator = new Mock<IIdentityValidator>();
            mockValidator.Setup(i=> i.IsValid(It.IsAny<string>())).Returns(true);
            var evaluator = new AppEvaluator(mockValidator.Object);

            var form = new JobApplications
            {
                Applicant = new Applicant
                {
                    Age = 19,
                    IdentiyNumber="123"
                },
                TechStackList= new List<string> { "" },
                YearsOfExperience = 19
            };
            //Action
            var appResult = evaluator.Evaluate(form);

            Assert.AreEqual(appResult, ApplicationResult.AutoRejected);
        }

        [Test]
        public void CheckApplication_CompareTechPoint75_TransferedToAutoAccepted()
        {
            //Arrange
            var mockValidator = new Mock<IIdentityValidator>();
            mockValidator.Setup(i => i.IsValid(It.IsAny<string>())).Returns(true);
            var evaluator = new AppEvaluator(mockValidator.Object);

            var form = new JobApplications
            {
                Applicant = new Applicant
                {
                    Age = 19,
                    IdentiyNumber = "123"
                },
                TechStackList = new List<string> { "C#", "Visual Studio", "RabbitMQ", "AWS", "MicroService" },
                YearsOfExperience = 19
            };
            //Action
            var appResult = evaluator.Evaluate(form);

            Assert.AreEqual(appResult, ApplicationResult.AutoAccept);
        }

        [Test]
        public void CheckApplication_WithInValidIdentity_TransferedToHR()
        {
            //Arrange
            var mockValidator = new Mock<IIdentityValidator>(MockBehavior.Strict);
            mockValidator.Setup(i => i.IsValid(It.IsAny<string>())).Returns(false);
            var evaluator = new AppEvaluator(mockValidator.Object);

            var form = new JobApplications()
            {
                Applicant = new Applicant{Age = 19}
            };
            //Action
            var appResult = evaluator.Evaluate(form);

            Assert.AreEqual(appResult, ApplicationResult.TransferedToHR);
        }
    }
}