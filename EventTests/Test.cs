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
                Logger.Log.Info("Logging successed");
            }
            catch (Exception e)
            {
                Logger.Log.Error(String.Format("Logging failed {0}", e.Message));
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

            try
            {
                _locationElements.StudentsTab.Click();
            }
            catch (Exception e)
            {
                Logger.Log.Error(String.Format("Сhoice student failed {0}", e.Message));
                Assert.Fail();
            }

            try
            {
                _eventService.GetStudent();
            }
            catch (Exception e)
            {
                Logger.Log.Error(String.Format("Student did not get {0}", e.Message));
                Assert.Fail();
            }

            _studentSummaryUrl = Driver.Url;

            try
            {
                Assert.AreEqual("Accelify - Eli Aaron", Driver.Title);
                Logger.Log.Info("Сhoice Student successed");
            }
            catch (Exception e)
            {
                Logger.Log.Error(String.Format("Сhoice student failed {0}", e.Message));
                Assert.Fail();
            }
        }

        [TestMethod]
        public void T3_CreateEvent504Referral()
        {
            Driver.Navigate().GoToUrl(_studentSummaryUrl);

            try
            {
                _eventService.GetEvent();
                Logger.Log.Info("Hampton 504 open successed");
            }
            catch (Exception e)
            {
                Logger.Log.Error(String.Format("Hampton 504 did not open {0}", e.Message));
                Assert.Fail();
            }


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
                Logger.Log.Info("Event 504 Referral open successed");
            }
            catch (Exception e)
            {
                Logger.Log.Error(String.Format("Event 504 Referral did not create{0}", e.Message));
                Assert.Fail();
            }
        }

        [TestMethod]
        public void T4_LockEvent504Reverral()
        {
            Driver.Navigate().GoToUrl(_createEventUrl);

            _eventService.WaitIsVisibleAndClickable(By.Id("btnCreateEventGroup"));
            _eventService.AddTimeOut(2000);

            var event504ReverralLinks = _locationElements.Event504ReverralLinksFromFirstTable;

            var eventsReverralLock = _locationElements.Event504ReverralLinksLock;
            var event504EligibilityMeetingLinks = _locationElements.Event504EligibilityMeetingLinks;

            if (event504ReverralLinks.Count != 0)
            {
                try
                {
                    event504ReverralLinks[0].Click();

                    _eventService.WaitIsVisibleAndClickable(By.LinkText("Section 504 Referral Form"));
                    _eventService.AddTimeOut(1000);
                    _locationElements.FormLink.Click();

                    Logger.Log.Info("Section 504 Referral Form open successed");
                }
                catch (Exception e)
                {
                    Logger.Log.Error(String.Format("Section 504 Referral Form did not open {0}", e.Message));
                    Assert.Fail();
                }

                try
                {
                    _eventService.AddValueToFields();
                    Logger.Log.Info("The values to fields added successed");
                }
                catch (Exception e)
                {
                    Logger.Log.Error(String.Format("The values to fields did not add {0}", e.Message));
                    Assert.Fail();
                }

                try
                {
                    _eventService.LockEvent();
                }
                catch (Exception e)
                {
                    Logger.Log.Error(String.Format("Event does not lock {0}", e.Message));
                    Assert.Fail();
                }

                var eventsReverralLockActual = _locationElements.Event504ReverralLinksLock;
                var event504EligibilityMeetingLinksActual = _locationElements.Event504EligibilityMeetingLinks;

                try
                {
                    Assert.AreEqual(eventsReverralLock.Count + 1, eventsReverralLockActual.Count);
                    Assert.AreEqual(event504EligibilityMeetingLinks.Count + 1, event504EligibilityMeetingLinksActual.Count);
                    Logger.Log.Info("Lock Event 504 Referral lock successed");
                }
                catch (Exception e)
                {
                    Logger.Log.Error(String.Format("Lock Event 504 Referral failed {0}", e.Message));
                    Assert.Fail();
                }
            }
            else
            {
                Logger.Log.Info("You do not have Events 504 Referral");
            }
        }

        [TestMethod]
        public void T5_UnLockEvent504Reverral()
        {
            Driver.Navigate().GoToUrl(_createEventUrl);

            _eventService.WaitIsVisibleAndClickable(By.Id("btnCreateEventGroup"));
            _eventService.AddTimeOut(2000);

            var eventsReverralLock = _locationElements.Event504ReverralLinksLock;

            if (eventsReverralLock.Count != 0)
            {
                eventsReverralLock[0].Click();

                _eventService.WaitIsVisibleAndClickable(By.Id("btnUnlockEvent"));
                _eventService.AddTimeOut(2000);

                _locationElements.UnLockButton.Click();
                Driver.SwitchTo().Alert().Accept();

                _eventService.WaitIsVisibleAndClickable(By.Id("btnCreateEventGroup"));
                _eventService.AddTimeOut(2000);

                var eventsReverralLockActual = _locationElements.Event504ReverralLinksLock;

                try
                {
                    Assert.AreEqual(eventsReverralLock.Count - 1, eventsReverralLockActual.Count);
                    Logger.Log.Info("Lock Event 504 Referral unlock successed");
                }
                catch (Exception e)
                {
                    Logger.Log.Error(String.Format("UnLock Event 504 Referral failed {0}", e.Message));
                    Assert.Fail();
                }
            }
            else
            {
                Logger.Log.Info("You don't have lock Events 504 Referral");
            }
        }

        [TestMethod]
        public void T6_DeleteEvent504Reverral()
        {
            Driver.Navigate().GoToUrl(_createEventUrl);

            _eventService.WaitIsVisibleAndClickable(By.Id("btnCreateEventGroup"));
            _eventService.AddTimeOut(3000);

            var event504ReverralLinks = _locationElements.Event504ReverralLinksFromFirstTable;

            if (event504ReverralLinks.Count != 0)
            {
                _locationElements.DeleteEvent504Referral.Click();
                Driver.SwitchTo().Alert().Accept();


                _eventService.WaitIsVisibleAndClickable(By.Id("btnCreateEventGroup"));
                _eventService.AddTimeOut(2000);

                var event504ReverralLinksActual = _locationElements.Event504ReverralLinksFromFirstTable;

                try
                {
                    Assert.AreEqual(event504ReverralLinks.Count - 1, event504ReverralLinksActual.Count);
                    Logger.Log.Info("Delete Event 504 Referral successed");
                }
                catch (Exception e)
                {
                    Logger.Log.Error(String.Format("Delete Event 504 Referral failed {0}", e.Message));
                    Assert.Fail();
                }
            }
            else
            {
                Logger.Log.Info("You don't have Event 504 Referral");
            }
        }

        [TestMethod]
        public void T7_DeleteNoticeAndEligibilityMeeting()
        {
            Driver.Navigate().GoToUrl(_createEventUrl);

            _eventService.WaitIsVisibleAndClickable(By.Id("btnCreateEventGroup"));
            _eventService.AddTimeOut(2000);

            var event504EligibilityMeetingLinks = _locationElements.Event504EligibilityMeetingLinks;

            if (event504EligibilityMeetingLinks.Count != 0)
            {
                _locationElements.DeleteEligibilityEvent.Click();
                Driver.SwitchTo().Alert().Accept();

                _eventService.WaitIsVisibleAndClickable(By.Id("btnCreateEventGroup"));
                _eventService.AddTimeOut(4000);

                _locationElements.DeleteEligibilityEvent.Click();
                Driver.SwitchTo().Alert().Accept();

                _eventService.WaitIsVisibleAndClickable(By.Id("btnCreateEventGroup"));
                _eventService.AddTimeOut(2000);

                var event504EligibilityMeetingLinksActual = _locationElements.Event504EligibilityMeetingLinks;

                try
                {
                    Assert.AreEqual(event504EligibilityMeetingLinks.Count - 1, event504EligibilityMeetingLinksActual.Count);
                    Logger.Log.Info("Delete Event 504 Eligibility successed");
                }
                catch (Exception e)
                {
                    Logger.Log.Error(String.Format("Delete Event 504 Eligibility failed {0}", e.Message));
                    Assert.Fail();
                }
            }
            else
            {
                Logger.Log.Info("You don't have Event 504 Eligibility");
            }
        }
    }
}
