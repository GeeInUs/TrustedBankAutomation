namespace TrustedBankAutomation.Core
{
    using AventStack.ExtentReports;
    using AventStack.ExtentReports.Reporter;
    using FluentAssertions;
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Text;
    using AventStack.ExtentReports.Gherkin.Model;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TechTalk.SpecFlow;
    using RazorEngine.Compilation.ImpromptuInterface.InvokeExt;

    public class Reporting
    {

        public struct Info
        {


            public  string ReportFilePath { set; get; }
            public  string ReportTitle { set; get; }
            public  string ReportDocName { set; get; }
            public ScenarioContext ReportScenarioContext { set; get; }
            public FeatureContext ReportFeatureContext { set; get; }

    }
        /// <summary>
        /// Access reporting property structure from another class
        /// </summary>
        public Info BDDReports;

        private ExtentTest extentLogger { set; get; }
        private ExtentReports extentRprt { set; get; }

        private ExtentHtmlReporter extentHtmlReporter { set; get; }

        private ExtentTest extentScenario { set; get; }
        private ExtentTest extentFeature { set; get; }

        private TestContext testContext { get; set; }

        public Reporting()
        {

        }

        /// <summary>
        /// Initailize all the necessary configuration for reporting
        /// </summary>
        public void InitReporting()
        {

            extentHtmlReporter = new ExtentHtmlReporter(BDDReports.ReportFilePath);

            //initialize ExtentReports and attach the HtmlReporter
            extentRprt = new ExtentReports();
            extentRprt.AttachReporter(extentHtmlReporter);

            //To add system or environment info by using the setSystemInfo method.
            extentRprt.AddSystemInfo("Domain Name", Environment.UserDomainName);
            extentRprt.AddSystemInfo("OS  Platform", System.Environment.OSVersion.Platform.ToString());
            extentRprt.AddSystemInfo("OS  Version", System.Environment.OSVersion.ToString());
            extentRprt.AddSystemInfo("Machine Name", Environment.MachineName);
            extentRprt.AddSystemInfo("User Name", Environment.UserName);
            

            //set BDD report Style
            extentRprt.AnalysisStrategy = AnalysisStrategy.BDD;
            extentRprt.GherkinDialect = "de";

            //configuration items to change the look and feel
            //add content, manage tests etc
            extentHtmlReporter.Config.DocumentTitle = BDDReports.ReportTitle;
            extentHtmlReporter.AnalysisStrategy = AnalysisStrategy.Test;

            extentHtmlReporter.Start();
            extentHtmlReporter.Config.ReportName = BDDReports.ReportDocName;
            extentHtmlReporter.Config.EnableTimeline = true;
            extentHtmlReporter.Config.Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Standard;

            // Create Report featue and scenario 
            extentFeature = extentRprt.CreateTest<Feature>(BDDReports.ReportFeatureContext.FeatureInfo.Title);
            extentScenario = extentFeature.CreateNode<Scenario>(BDDReports.ReportScenarioContext.ScenarioInfo.Title);

      
        }


        /// <summary>
        ///  Assign status and screenshot  to test reporter 
        /// </summary>
        /// <param name="screenShotFilePath"></param>
      public void LogStatus(string screenShotFilePath)
        {

            StepInfo StepInfo = BDDReports.ReportScenarioContext.StepContext.StepInfo;

            ExtentTest StepNode = GetStepNode( StepInfo);

            StepNode = AssignTestStatus( StepNode, screenShotFilePath);

            if (screenShotFilePath != null && File.Exists(screenShotFilePath))
                StepNode.AddScreenCaptureFromPath(screenShotFilePath);

            StepNode.Log(Status.Info, "Step Details : <br> "
                 + BDDReports.ReportScenarioContext.StepContext.StepInfo.Text
                + "<img src=\"" + screenShotFilePath  + "\" >"
                );


          } 


        /// <summary>
        /// Create an instance of step Node for each Gherkin Step defined
        /// </summary>
        /// <param name="StepInfo"></param>
        /// <param name="screenShotFilePath"></param>
        /// <returns></returns>
        private ExtentTest AssignTestStatus( ExtentTest StepNode, string screenShotFilePath)
        {
            switch (BDDReports.ReportScenarioContext.ScenarioExecutionStatus)
            {
                case  ScenarioExecutionStatus.OK:
                    StepNode.Pass("Step Pass" );
                    break;
                case ScenarioExecutionStatus.BindingError:
                    StepNode = StepNode.Fail("Binding Error :" + BDDReports.ReportScenarioContext.TestError.Message);
                    break;
                case ScenarioExecutionStatus.Skipped:
                    StepNode.Skip("Step Skipped");
                    break;
                case ScenarioExecutionStatus.StepDefinitionPending:
                    StepNode.Warning("Step Pending Implementation");
                    break;
                case ScenarioExecutionStatus.TestError:
                    StepNode.Fail("Test Error :" + BDDReports.ReportScenarioContext.TestError.Message);
                    break;
                case ScenarioExecutionStatus.UndefinedStep:
                    StepNode.Warning("Undefined Step with " + BDDReports.ReportScenarioContext.ScenarioInfo.Arguments.ToString() + " arguements");
                    break;
            }

            return StepNode;
        }


        /// <summary>
        /// Create an instance of step Node for each Gherkin Step defined
        /// </summary>
        /// <param name="StepInfo"></param>
        /// <returns></returns>
        private ExtentTest GetStepNode(StepInfo StepInfo)
        {
            string StepType = StepInfo.StepInstance.StepDefinitionType.ToString();
            ExtentTest StepNode = null;

            switch (StepType)
            {
                case "Given":
                    StepNode = extentScenario.CreateNode<Given>(StepInfo.Text);
                    break;
                case "When":
                    StepNode = extentScenario.CreateNode<When>(StepInfo.Text);
                    break;
                case "Then":
                    StepNode = extentScenario.CreateNode<Then>(StepInfo.Text);
                    break;
                case "And":
                    StepNode = extentScenario.CreateNode<And>(StepInfo.Text);
                    break;
            }

            return StepNode;
        }



        /// <summary>
        /// Close the reporter
        /// </summary>
        public void Close()
        {
            //to write or update test information to reporter
            extentHtmlReporter.Stop();
            extentRprt.Flush();


        }
    }
}