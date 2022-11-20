using JobAppLib.Common;
using JobAppLib.Models;
using JobAppLib.Services;

namespace JobAppLib
{
    public class AppEvaluator
    {
        private int minAge = 18;
        private const int autoAcceptedYearOfAcceptedExperience = 15;
        private List<string> list = new() { "C#", "RabbitMQ", "MicroService", "Visual Studio", "AWS" };
        private IIdentityValidator identityValidator;

        public AppEvaluator(IIdentityValidator identityValidator)
        {
            this.identityValidator = identityValidator;
        }

        public ApplicationResult Evaluate(JobApplications form)
        {
            if (form.Applicant.Age < minAge)
                return ApplicationResult.AutoRejected;

            var similarityRate = GetTechStackSimilarityRate(form.TechStackList);
            var validIdentity = identityValidator.IsValid(form.Applicant.IdentiyNumber);

            if (!validIdentity)
                return ApplicationResult.TransferedToHR;
            if (similarityRate < 25)
                return ApplicationResult.AutoRejected;
            if (similarityRate > 75 && form.YearsOfExperience > autoAcceptedYearOfAcceptedExperience)
                return ApplicationResult.AutoAccept;

            return ApplicationResult.AutoAccept;
        }

        private int GetTechStackSimilarityRate(List<string> techList)
        {
            if(techList == null) return 0;
            var machedCount = techList.Where(i => list
            .Contains(i, StringComparer.OrdinalIgnoreCase))
                .Count();

            return (int)((double)machedCount / list.Count()) * 100;
        }
    }
}