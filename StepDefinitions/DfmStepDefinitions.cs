using DeepEqual;
using SpecFlow.Internal.Json;
using SpecFlowAPITestDemo.Support;
using TechTalk.SpecFlow.Infrastructure;

namespace SpecFlowAPITestDemo.StepDefinitions
{
    [Binding]
    public sealed class DfmStepDefinitions :CommonMethods
    {
        public ScenarioContext scenarioContext;
        public ISpecFlowOutputHelper specFlowOutputHelper;

        public DfmStepDefinitions(ISpecFlowOutputHelper specFlowOutputHelper, ScenarioContext scenarioContext)
        {
            this.scenarioContext = scenarioContext;
            this.specFlowOutputHelper = specFlowOutputHelper;
        }

        [When(@"user sends post request with '([^']*)'")]
        public void WhenUserSendsPostRequestWith(string payloadFileName)
        {
            var response = RunPostApiAndGetResponseAsString(payloadFileName);
            scenarioContext["response"] = response;
        }

        [Then(@"response should match with '([^']*)'")]
        public void ThenResponseShouldMatchWith(string expectedResponseFileName)
        {
            var actualResponse = scenarioContext["response"].ToString();
            Assert.IsTrue(DoesExpectedResponseMatchesActual(actualResponse, expectedResponseFileName));
        }

        [When(@"user sends post request with '([^']*)' response should match with '([^']*)' object")]
        public void WhenUserSendsPostRequestWithResponseShouldMatchWithObject(string payloadFileName, string expectedResponseFileName)
        {
            var actualResponse = RunPostApiAndGetResponseAsObject(payloadFileName);
            var expectedResponse = GetExpectedResponseObject(expectedResponseFileName);

            if (!actualResponse.Equals(expectedResponse)) 
            {
                var differences = CompareObjectsAndGetDifferences(expectedResponse,actualResponse);
                specFlowOutputHelper.WriteLine(payloadFileName);
            }

            var areSkillsEqual = new HashSet<Skill>(listOfExpectedSkills).SetEquals(listOfActualSkills);
            var areRolesEqual = new HashSet<Role>(listOfExpectedRoles).SetEquals(listOfActualRoles);
        }
    }
}
