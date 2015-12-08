using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using Logger = Model.Logger;

namespace EventTests
{
    [TestClass]
    public class Test
    {
        private static readonly FirefoxDriver Driver = EventService.GetDriver();
        private const string BaseUrl = "http://hampton-demo.accelidemo.com/Login.aspx";

        private readonly LoginPage _loginPage = new LoginPage(Driver);
        private readonly LocationElements _locationElements = new LocationElements(Driver);
        private readonly EventService _eventService = new EventService();

        private static string _studentSummaryUrl;
        private static string _createEventUrl;

        [TestInitialize]
        public void SetUp()
        {
            Logger.Initialize();
        }

        [TestMethod]
        public void T1_Logging()
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
                Logger.Log.Error(String.Format("Login Failed {0}", e.Message));
                Assert.Fail();
            }
        }

        [TestMethod]
        public void T2_СhoiceStudent()
        {
            var homeElement = _locationElements.HomeElement;
            Actions actions = new Actions(Driver);
            actions.MoveToElement(homeElement).Build().Perform();
            _locationElements.SpedElement.Click();

            _eventService.WaitIsVisibleAndClickable(By.Id("HamptonIEPchtStudentCompliance"));
            _eventService.AddTimeOut(1000);

            _locationElements.StudentsTab.Click();

            _eventService.GetStudent();
            _studentSummaryUrl = Driver.Url;

            try
            {
                Assert.AreEqual("Accelify - Eli Aaron", Driver.Title);
                Logger.Log.Info("Сhoice Student Successed");
            }
            catch (Exception e)
            {
                Logger.Log.Error(String.Format("Сhoice student Failed {0}", e.Message));
                Assert.Fail();
            }
        }

        [TestMethod]
        public void T3_CreateEvent504ReferraL()
        {
            Driver.Navigate().GoToUrl(_studentSummaryUrl);
            _eventService.GetEvent();

            _eventService.WaitIsVisibleAndClickable(By.Id("pnlEventLockedList"));
            _eventService.AddTimeOut(2000);
            _createEventUrl = Driver.Url;
            var referral504EventsCount = _locationElements.Reverral504Elements.Count;

            _eventService.CreateEvent(3);

            _eventService.AddTimeOut(5000);
            var eventCountActual = _locationElements.Reverral504Elements.Count;
            var eventCountExpected = referral504EventsCount + 1;

            try
            {
                Assert.AreEqual(eventCountExpected, eventCountActual);
                Logger.Log.Info("Create Event 504 Referral successed");
            }
            catch (Exception e)
            {
                Logger.Log.Error(String.Format("Create Event 504 Referral Failed {0}", e.Message));
                Assert.Fail();
            }
        }

        [TestMethod]
        public void T4_LockEvent504ReverraL()
        {
            Driver.Navigate().GoToUrl(_createEventUrl);

            _eventService.WaitIsVisibleAndClickable(By.Id("btnUpdateForm"));
            _eventService.AddTimeOut(2000);

            var event504ReverralLinks = _locationElements.Event504ReverralLinksFromFirstTable;

            var eventsReverralLock = _locationElements.Event504ReverralLinksLock;
            var event504EligibilityMeetingLinks = _locationElements.Event504EligibilityMeetingLinks;

            if (event504ReverralLinks.Count != 0)
            {
                event504ReverralLinks[0].Click();

                _eventService.WaitIsVisibleAndClickable(By.LinkText("Section 504 Referral Form"));
                _eventService.AddTimeOut(1000);
                _locationElements.FormLink.Click();

                _eventService.AddValueToFields();
                _eventService.LockEvent();

                var eventsReverralLockActual = _locationElements.Event504ReverralLinksLock;
                var event504EligibilityMeetingLinksActual = _locationElements.Event504EligibilityMeetingLinks;

                try
                {
                    Assert.AreEqual(eventsReverralLock.Count + 1, eventsReverralLockActual.Count);
                    Assert.AreEqual(event504EligibilityMeetingLinks.Count + 1, event504EligibilityMeetingLinksActual.Count);
                    Logger.Log.Info("Lock Event 504 Referral successed");
                }
                catch (Exception e)
                {
                    Logger.Log.Error(String.Format("Lock Event 504 Referral Failed {0}", e.Message));
                    Assert.Fail();
                }
            }
        }
    }
}
