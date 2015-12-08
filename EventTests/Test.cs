using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using Logger = Model.Logger;

namespace EventTests
{
    [TestClass]
    public class Test
    {
        private const string BaseUrl = "http://hampton-demo.accelidemo.com/Login.aspx";

        private static readonly FirefoxDriver Driver = new FirefoxDriver();
        private readonly LoginPage _loginPage = new LoginPage(Driver);
        private readonly LocationElements _locationElements = new LocationElements(Driver);
        private readonly WebDriverWait _wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(20));
        private static string _studentSummaryUrl;
        private static string _createEventUrl;

        [TestInitialize]
        public void SetUp()
        {
            Logger.Initialize();
        }

        [TestMethod]
        public void CreateLogin()
        {
            Driver.Navigate().GoToUrl(BaseUrl + "/Login.aspx");
            _loginPage.UserName.SendKeys("superlion");
            _loginPage.Password.SendKeys("CTAKAH");
            _loginPage.SubmitButton.Click();
            const string homePageUrl = "http://hampton-demo.accelidemo.com/AcceliTrack/Home.aspx";

            try
            {
                Assert.AreEqual(Driver.Url, homePageUrl);
                Logger.Log.Info("Login successed");
            }
            catch (Exception e)
            {
                Logger.Log.Info(String.Format("Login Failed {0}", e.Message));
                Assert.Fail();
            }
        }

        [TestMethod]
        public void DOpenStudent()
        {
            var homeElement = _locationElements.HomeElement;
            Actions actions = new Actions(Driver);
            actions.MoveToElement(homeElement).Build().Perform();
            _locationElements.SpedElement.Click();

            WaitIsVisibleAndClickable(By.Id("HamptonIEPchtStudentCompliance"));
            AddTimeOut(1000);

            _locationElements.StudentsTab.Click();

            GetStudent();
       
            _studentSummaryUrl = Driver.Url;
        }

        [TestMethod]
        public void Event504ReverraLCreate()
        {
           // _wait.Until(ExpectedConditions.UrlToBe(_studentSummaryUrl));
            Driver.Navigate().GoToUrl(_studentSummaryUrl);
            GetEvent();

            WaitIsVisibleAndClickable(By.Id("pnlEventLockedList"));
            AddTimeOut(2000);
            var referral504EventsCount = _locationElements.Reverral504Elements.Count;

            CreateEvent(3);

            AddTimeOut(5000);
            var eventCountActual = _locationElements.Reverral504Elements.Count;
            var eventCountExpected = referral504EventsCount + 1;
            _createEventUrl = Driver.Url;
            Assert.AreEqual(eventCountExpected, eventCountActual);

        }

        [TestMethod]
        public void LockEvent504ReverraL()
        {
            Driver.Navigate().GoToUrl(_createEventUrl);

            AddTimeOut(10000);

            var event504ReverralLinks = _locationElements.Event504ReverralLinksFromFirstTable;

            var eventsReverralLock = _locationElements.Event504ReverralLinksLock;
            var event504EligibilityMeetingLinks = _locationElements.Event504EligibilityMeetingLinks;

            if (event504ReverralLinks.Count != 0)
            {
                event504ReverralLinks[0].Click();

                WaitIsVisibleAndClickable(By.LinkText("Section 504 Referral Form"));
                AddTimeOut(1000);
                _locationElements.FormLink.Click();

                AddValueToFields();
                LockEvent();

                var eventsReverralLockActual = _locationElements.Event504ReverralLinksLock;
                var event504EligibilityMeetingLinksActual = _locationElements.Event504EligibilityMeetingLinks;

                Assert.AreEqual(eventsReverralLock.Count + 1, eventsReverralLockActual.Count);
                Assert.AreEqual(event504EligibilityMeetingLinks.Count + 1, event504EligibilityMeetingLinksActual.Count);
            }
        }
        private void WaitIsVisibleAndClickable(By location)
        {
            _wait.Until(ExpectedConditions.ElementIsVisible(location));
            _wait.Until(ExpectedConditions.ElementToBeClickable(location));
        }
        private void LockEvent()
        {
            WaitIsVisibleAndClickable(By.CssSelector("[required='required']"));
            WaitIsVisibleAndClickable(By.Id("btnUpdateForm"));
            AddTimeOut(3000);
            _locationElements.LockButton.Click();

            Driver.SwitchTo().Alert().Accept();
            WaitIsVisibleAndClickable(By.Id("pnlEventLockedList"));
            AddTimeOut(2000);
        }

        private static void AddTimeOut(int timeOut)
        {
            Thread.Sleep(timeOut);
        }

        private void CreateEvent(int eventNumber)
        {
            WaitIsVisibleAndClickable(By.Id("btnCreateEventGroup"));
            AddTimeOut(2000);
            _locationElements.CreateEventButton.Click();

            WaitIsVisibleAndClickable(By.ClassName("k-input"));
            ((IJavaScriptExecutor)Driver).ExecuteScript(String.Format("$('#{0}').data('kendoDropDownList').select({1});", "EventGroupDefinitionId", eventNumber));

            _locationElements.DateControl.SendKeys("11/11/2015 12:00 AM");
            WaitIsVisibleAndClickable(By.Id("btnSaveEventGroup"));
            AddTimeOut(1000);

            _locationElements.SaveEventButton.Click();
        }

        private void AddValueToFields()
        {
            WaitIsVisibleAndClickable(By.Id("btnUpdateForm"));
            AddTimeOut(2000);

            var fieldsInput = _locationElements.FieldsInputRequired;
            InputValueForFields(fieldsInput);

            var fieldsTestarea = _locationElements.FieldsTextAreaRequired;
            InputValueForFields(fieldsTestarea);

            _locationElements.UpdateFormButton.Click();
        }

        private static void InputValueForFields(IList<IWebElement> fields)
        {
            foreach (var field in fields)
            {
                field.Clear();
                field.SendKeys("1");
            }
        }

        private void GetStudent()
        {
            WaitIsVisibleAndClickable(By.ClassName("k-icon"));
            AddTimeOut(1000);
            var links = _locationElements.Links;

            for (int i = 0; i < links.Count; i++)
            {
                var hrefAtr = links[i].GetAttribute("href").Contains("/IEP/Students/ViewStudent");

                if (hrefAtr)
                {
                    links[i].Click();
                    break;
                }
            }
        }

        private void GetEvent()
        {
            WaitIsVisibleAndClickable(By.Id("btnAddStudentToCaseload"));
            var eventsLink = _locationElements.EventLinks;

            for (int i = 0; i < eventsLink.Count; i++)
            {
                var hrefElementHampton504 = eventsLink[i].GetAttribute("href").Contains("Events&programType=Hampton504");

                if (hrefElementHampton504)
                {
                    eventsLink[i].Click();
                    break;
                }
            }
        }
    }
}
