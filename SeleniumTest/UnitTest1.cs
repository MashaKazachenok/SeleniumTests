using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace SeleniumTest
{
    [TestClass]
    public class UnitTest1
    {
        public static string BaseUrl = "http://hampton-demo.accelidemo.com/";

        public const int TimeOut = 30;

        public FirefoxDriver Driver = new FirefoxDriver();

        [TestInitialize]
        public void Start()
        {
            Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(TimeOut));

            Driver.Navigate().GoToUrl(BaseUrl + "/Login.aspx");

            Driver.FindElement(By.Id("plcContent_LoginControl_UserName")).SendKeys("JohnE");
            Driver.FindElement(By.Id("plcContent_LoginControl_Password")).SendKeys("success");

            Driver.FindElement(By.Id("plcContent_LoginControl_lnkLogin")).Click();

            Driver.Navigate().GoToUrl("http://hampton-demo.accelidemo.com/AcceliTrack/ServiceRecorder.aspx");

            Driver.FindElement(
                By.Id("plcContent_plcContent_plcContent_ctlStudentSelector_ctlSingleModeStudentSelection_gvStudents_0"))
                .Click();

        }

        [TestMethod]
        public void CreateServiceWithAllFields()
        {
            var dateControl =
                Driver.FindElement(By.Id("plcContent_plcContent_plcContent_ctlServiceDetails_dtServiceDate_txtDate"));
            dateControl.Clear();
            dateControl.SendKeys("10302015");

            var deliveryStatus =
                Driver.FindElement(
                    By.Id("plcContent_plcContent_plcContent_ctlServiceDetails_lstServiceDeliveryStatusInput"));
            deliveryStatus.Click();
            SelectItemFromDropdown(Driver, 3);

            var locationInput =
                Driver.FindElement(By.Id("plcContent_plcContent_plcContent_ctlServiceDetails_lstLocationInput"));
            locationInput.Click();
            SelectItemFromDropdown(Driver, 16);

            // box procedure Code 
            var procedureCode =
                Driver.FindElement(By.Id("plcContent_plcContent_plcContent_ctlServiceDetails_lstProcedureCodeInput"));
            procedureCode.Click();
            SelectItemFromDropdown(Driver, 34);

            // box goals Objectives
            var goalsObjectives =
                Driver.FindElement(By.Id("plcContent_plcContent_plcContent_ctlServiceDetails_txtGoals"));
            goalsObjectives.SendKeys("text");

            // box activity
            var activity =
                Driver.FindElement(By.Id("plcContent_plcContent_plcContent_ctlServiceDetails_lstTherapyMethodInput"));
            activity.Click();
            SelectItemFromDropdown(Driver, 40);

            // box activity Other
            var activityOther =
                Driver.FindElement(By.Id("plcContent_plcContent_plcContent_ctlServiceDetails_txtTherapyMethod"));
            activityOther.SendKeys("other");

            // box progress
            var progress =
                Driver.FindElement(By.Id("plcContent_plcContent_plcContent_ctlServiceDetails_lstTherapyProgressInput"));
            progress.Click();
            SelectItemFromDropdown(Driver, 54);

            // box Response to Procedure (must be measurable):
            var responseToPricedure =
                Driver.FindElement(By.Id("plcContent_plcContent_plcContent_ctlServiceDetails_txtComments"));
            responseToPricedure.SendKeys("response");

            var assignToSeries =
                Driver.FindElement(
                    By.Id("plcContent_plcContent_plcContent_ctlServiceDetails_ctlSeriesSelector_lnkSeriesSelection"));
            assignToSeries.Click();

            var assign =
                Driver.FindElement(
                    By.Id(
                        "plcContent_plcContent_plcContent_ctlServiceDetails_ctlSeriesSelector_pnlSeriesPopup_ctlSeriesSelection_gvSeries_0"));
            assign.Click();

            var buttonSave = Driver.FindElement(By.Id("plcContent_plcContent_plcContent_ctlServiceDetails_btnSave"));
            buttonSave.Click();

            Thread.Sleep(2000);

            var buttonOk =
                Driver.FindElement(
                    By.Id(
                        "plcContent_plcContent_plcContent_ctlServiceDetails_ctlServiceDetailsWarnings_pnlWarnings_btnOk"));
            if (buttonOk.Text != "")
            {
                buttonOk.Click();
            }

            Thread.Sleep(2000);

            var buttonOk2 =
                Driver.FindElement(
                    By.Id(
                        "plcContent_plcContent_plcContent_ctlServiceDetails_ctlServiceDetailsPeriodWarnings_pnlWarnings_btnOk"));

            if (buttonOk2.Text != "")
            {
                buttonOk2.Click();
            }
            Thread.Sleep(5000);

            var services = Driver.FindElement(
                By.Id(
                    "plcContent_plcContent_ctlAlternateMenu_ctlMenuGroupAcceliTrackRecorder_pnlAlternateMenuGroup_rptAlternateMenu_lnkAlternateMenu_5"));

            if (services.Text != "")
            {
                services.Click();
            }

            Driver.Close();
        }

        private void SelectItemFromDropdown(FirefoxDriver driver, int index)
        {
            var elements = driver.FindElements(By.CssSelector(".ui-menu-item a"));
            elements[index].Click();
        }


        [TestMethod]
        public void ChangeTimeIfItExist()
        {
            var dateControl = Driver.FindElement(By.Id("plcContent_plcContent_plcContent_ctlServiceDetails_dtServiceDate_txtDate"));
            dateControl.Clear();
            dateControl.SendKeys("10302015");

            var locationInput = Driver.FindElement(By.Id("plcContent_plcContent_plcContent_ctlServiceDetails_lstLocationInput"));
            locationInput.Click();
            SelectItemFromDropdown(Driver, 10);

            SavePage();

            Thread.Sleep(2000);

            CheckButtonExict();

            Thread.Sleep(5000);

            var services = Driver.FindElement(By.Id("plcContent_plcContent_ctlAlternateMenu_ctlMenuGroupAcceliTrackRecorder_pnlAlternateMenuGroup_rptAlternateMenu_lnkAlternateMenu_5"));

            if (services.Text != "")
            {
                services.Click();
            }
        }

        private void SavePage()
        {
            var buttonSave = Driver.FindElement(By.Id("plcContent_plcContent_plcContent_ctlServiceDetails_btnSave"));
            buttonSave.Click();

            Thread.Sleep(2000);

            var buttonOk = Driver.FindElement( By.Id("plcContent_plcContent_plcContent_ctlServiceDetails_ctlServiceDetailsWarnings_pnlWarnings_btnOk"));
            if (buttonOk.Text != "")
            {
                buttonOk.Click();
            }
        }

        private void ChangeTime()
        {
            var timeStartInput = Driver.FindElement((By.Id("plcContent_plcContent_plcContent_ctlServiceDetails_dtStartTime_lstTimeHourInput")));
            timeStartInput.Click();

            String timeCurrent = timeStartInput.GetAttribute("value");

            Thread.Sleep(2000);

            var elements = Driver.FindElements(By.CssSelector(".ui-menu-item a"));

            for (int i = 0; i < elements.Count ; i++)
            {
                var valueOption = elements[i].Text;
                if (valueOption != timeCurrent)
                {
                    elements[i].Click();
                    break;
                }
            }
          
            SavePage();
        }

        private void CheckButtonExict()
        {
            Thread.Sleep(2000);

            var buttonCancelIfTimeExict =  Driver.FindElement(By.Id(  "plcContent_plcContent_plcContent_ctlServiceDetails_ctlServiceDetailsPeriodWarnings_pnlWarnings_btnCancel"));

            if (buttonCancelIfTimeExict.Text != "")
            {
                buttonCancelIfTimeExict.Click();
                ChangeTime();
                CheckButtonExict();
            }
        }

    }
}


 




