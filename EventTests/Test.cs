using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
        private readonly CreateEventElements _createEventElements = new CreateEventElements(Driver);
        private readonly WebDriverWait _wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(20));

        [TestInitialize]
        public void SetUp()
        {
            Logger.Initialize();
        }

        [TestMethod]
        [Priority(1)]
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
        [Priority(2)]
        public void Event504ReverraLCreate()
        {
            var homeElement = _createEventElements.HomeElement;
            Actions actions = new Actions(Driver);
            actions.MoveToElement(homeElement).Build().Perform();
            _createEventElements.SpedElement.Click();


            _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("HamptonIEPchtStudentCompliance")));
            AddTimeOut(1000);

            _createEventElements.StudentsTab.Click();


            GetStudent(_wait);

            GetEvent(_wait);

            AddTimeOut(10000);
            var referral504EventsCount = _createEventElements.Reverral504Elements.Count;

            CreateEvent(_wait, 3);

            AddTimeOut(5000);
            var eventCountActual = _createEventElements.Reverral504Elements.Count;
            var eventCountExpected = referral504EventsCount + 1;

            Assert.AreEqual(eventCountExpected, eventCountActual);
           
        }

        [TestMethod]
        [Priority(3)]
        public void LockEvent504ReverraL()
        {
            Driver.Navigate().GoToUrl("http://hampton-demo.accelidemo.com/IEP/Students/ViewStudent?commonStudentId=11250&studentViewType=Events&programType=Hampton504");

            AddTimeOut(10000);

            var event504ReverralLinks = _createEventElements.Event504ReverralLinksFromFirstTable;

            var eventsReverralLock = _createEventElements.Event504ReverralLinksLock.Count;
            var event504EligibilityMeetingLinks = _createEventElements.Event504EligibilityMeetingLinks.Count;

            if (event504ReverralLinks.Count != 0)
            {
                event504ReverralLinks[0].Click();

                var linkForm = _wait.Until(d => d.FindElement(By.LinkText("Section 504 Referral Form")));
                AddTimeOut(1000);
                linkForm.Click();

                AddValueToFields(_wait);
                LockEvent(_wait);

                var eventsReverralLockActual = _createEventElements.Event504ReverralLinksLock.Count;
                var event504EligibilityMeetingLinksActual = _createEventElements.Event504EligibilityMeetingLinks.Count;

                Assert.AreEqual(eventsReverralLock + 1, eventsReverralLockActual);
                Assert.AreEqual(event504EligibilityMeetingLinks + 1, event504EligibilityMeetingLinksActual);
            }
        }

        private void LockEvent(WebDriverWait wait)
        {
            wait.Until(d => d.FindElements(By.CssSelector("[required='required']")));
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("[required='required']")));
            AddTimeOut(3000);
            _createEventElements.LockButton.Click();

            Driver.SwitchTo().Alert().Accept();
            AddTimeOut(10000);
        }

        private static void AddTimeOut(int timeOut)
        {
            Thread.Sleep(timeOut);
        }

        private void CreateEvent(WebDriverWait wait, int eventNumber)
        {

            wait.Until(d => d.FindElement(By.Id("btnCreateEventGroup")));
            AddTimeOut(2000);
            _createEventElements.CreateEventButton.Click();

            wait.Until(d => d.FindElement(By.ClassName("k-input")));
            ((IJavaScriptExecutor)Driver).ExecuteScript(String.Format("$('#{0}').data('kendoDropDownList').select({1});", "EventGroupDefinitionId", eventNumber));

            _createEventElements.DateControl.SendKeys("11/11/2015 12:00 AM");
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("btnSaveEventGroup")));
            AddTimeOut(1000);

            _createEventElements.SaveEventButton.Click();
        }

        private void AddValueToFields(WebDriverWait wait)
        {
            wait.Until(d => d.FindElement(By.Id("btnUpdateForm")));
            AddTimeOut(2000);

            var fieldsInput = _createEventElements.FieldsInputRequired;
            InputValueForFields(fieldsInput);

            var fieldsTestarea = _createEventElements.FieldsTextAreaRequired;
            InputValueForFields(fieldsTestarea);

            _createEventElements.UpdateFormButton.Click();
        }

        private static void InputValueForFields(IList<IWebElement> fields)
        {
            foreach (var field in fields)
            {
                field.Clear();
                field.SendKeys("1");
            }
        }

        private void GetStudent(WebDriverWait wait)
        {
            wait.Until(d => d.FindElement(By.ClassName("k-icon")));
            AddTimeOut(1000);
            var links = _createEventElements.Links;

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

        private void GetEvent(WebDriverWait wait)
        {
           wait.Until(d => d.FindElement(By.Id("btnAddStudentToCaseload")));
            var eventsLink = _createEventElements.EventLinks;

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
