using nunit_datadriven.commons;
using nunit_datadriven.pageobjects;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using System.Configuration;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework;
using AventStack.ExtentReports.Gherkin.Model;
using System.IO;
using TechTalk.SpecFlow.Bindings;

namespace specflowdemo.commons
{
    [Binding]
    public class SetUpClass
    {
        public static IWebDriver driver = InitBrowser.Getbrowser();

        private static string fileName = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), @"reports");
        private static ExtentTest featureName;
        private static ExtentTest scenario;
        private static ExtentReports extent;

        public HomePage home;
        public LoginPage login;
        public Read_ini read;
        public ScenarioContext _scenarioContext;
        public static FeatureContext _featureContext;
        private static long milliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

        private SetUpClass(ScenarioContext scenarioContext)
        {
            read = new Read_ini();
            home = new HomePage();
            login = new LoginPage();
        }

        [BeforeTestRun]
        public static void initApplication()
        {
            var htmlReporter = new ExtentHtmlReporter(fileName + "/" + milliseconds + ".html");
            htmlReporter.Config.Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Dark;
            //Attach report to reporter
            extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);

            driver.Navigate().GoToUrl(ReadEnv.ReadData("base", "appUrl"));
            driver.Manage().Window.Maximize();
        }

        [BeforeFeature]
        public static void InvokeURl(FeatureContext featureContext)
        {
            _featureContext = featureContext;
            featureName = extent.CreateTest<Feature>(_featureContext.FeatureInfo.Title);
        }

        [AfterFeature]
        public static void RefreshPage()
        {
            // driver.Navigate().Refresh();
        }

        [BeforeScenario]
        public void loginToApp(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            scenario = featureName.CreateNode<Scenario>(_scenarioContext.ScenarioInfo.Title);
            home.TapLoginLink();
            login.AssertLogin();
            login.loginToApplication(ReadEnv.ReadData("base", "username"), ReadEnv.ReadData("base", "password"));
            login.tapSubmit();
        }

        [AfterScenario]
        public void logout(ScenarioContext scenarioContext)
        {
            home.logout();
        }

        [BeforeStep]
        public void BeforeStep()
        {
            System.Console.WriteLine("BeforeStep- Start Timer");
        }

        [AfterStep]
        public void AfterStep(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            var stepType = _scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();

            if (_scenarioContext.TestError == null)
            {
                if (stepType == "Given")
                    CreateScenario(scenario, StepDefinitionType.Given);
                else if (stepType == "When")

                    CreateScenario(scenario, StepDefinitionType.When);
                else if (stepType == "Then")

                    CreateScenario(scenario, StepDefinitionType.Then);
            }
            else if (_scenarioContext.TestError != null)
            {
                if (stepType == "Given")
                {
                    CreateScenarioFailOrError(scenario, StepDefinitionType.Given, _scenarioContext);
                }
                else if (stepType == "When")
                {
                    CreateScenarioFailOrError(scenario, StepDefinitionType.When, _scenarioContext);
                }
                else if (stepType == "Then")
                {
                    CreateScenarioFailOrError(scenario, StepDefinitionType.Then, _scenarioContext);
                }
            }
            _scenarioContext.Clear();
        }

        // [BeforeScenarioBlock]

        /// [AfterScenarioBlock]
        [AfterTestRun]
        public static void CloseApplication()
        {
            driver.Quit();
            extent.Flush();
        }

        private static ExtentTest CreateScenario(ExtentTest extent, StepDefinitionType stepDefinitionType)
        {
            // o SpecFlow nos permite pegar o nome do Step usando o ScenarioStepContext.Current
            var scenarioStepContext = ScenarioStepContext.Current.StepInfo.Text;

            switch (stepDefinitionType)
            {
                case StepDefinitionType.Given:
                    return extent.CreateNode<Given>(scenarioStepContext);

                case StepDefinitionType.Then:
                    return extent.CreateNode<Then>(scenarioStepContext);

                case StepDefinitionType.When:
                    return extent.CreateNode<When>(scenarioStepContext);

                default:
                    throw new ArgumentOutOfRangeException(nameof(stepDefinitionType), stepDefinitionType, null);
            }
        }

        private static void CreateScenarioFailOrError(ExtentTest extent, StepDefinitionType stepDefinitionType, ScenarioContext scenarioContext)
        {
            var error = scenarioContext.TestError;

            if (error.InnerException == null)
            {
                CreateScenario(extent, stepDefinitionType).Error(error.Message);
            }
            else
            {
                CreateScenario(extent, stepDefinitionType).Fail(error.InnerException);
            }
        }
    }
}