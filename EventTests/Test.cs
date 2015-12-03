using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;

namespace EventTests
{
    [TestClass]
    public class Test
    {
        private const string BaseUrl = "http://hampton-demo.accelidemo.com/";

        private FirefoxDriver _driver;
        private LoginPage _loginPage;
        private CreateEventElements _createEventElements;


        [TestInitialize]
        public void SetUp()
        {
            _driver = new FirefoxDriver();
            _loginPage = new LoginPage(_driver);
            _createEventElements = new CreateEventElements(_driver);

            _driver.Navigate().GoToUrl(BaseUrl + "/Login.aspx");
       
            _loginPage.UserName.SendKeys("superlion");
            _loginPage.Password.SendKeys("CTAKAH");
            _loginPage.SubmitButton.Click();
        }

        [TestMethod]
        public void LockEvent504ReverraL()
        {
            AddTimeOut(2000);
            var homeElement = _createEventElements.HomeElement;
            AddTimeOut(500);

            Actions actions = new Actions(_driver);
            actions.MoveToElement(homeElement).Build().Perform();

            _createEventElements.SpedElement.Click();

            AddTimeOut(7000);
            _createEventElements.StudentsTab.Click();

            GetStudent();

            GetEvent();

            AddTimeOut(6000);
            var referral504EventsCount = _createEventElements.Reverral504Elements.Count;
            AddTimeOut(3000);
            CreateEvent504Reverral();
           
            var eventCountActual = _createEventElements.Reverral504Elements.Count;
            var eventCountExpected = referral504EventsCount + 1;
            var allEventCount = _createEventElements.AllEventsElements.Count;
            AddTimeOut(11000);

            Assert.AreEqual(eventCountExpected, eventCountActual);

            AddTimeOut(3000);
           _createEventElements.Reverral504Elements[0].Click();

            AddTimeOut(7000);
           _createEventElements.FormLink.Click();

            AddValueToFields();
         
           _createEventElements.LockButton.Click();
            AddTimeOut(2000);

            _driver.SwitchTo().Alert().Accept();
            AddTimeOut(8000);

            var event504ReverralAfterLock = _createEventElements.Reverral504Elements.Count;
            var allEventCountActual = _createEventElements.AllEventsElements.Count;
            AddTimeOut(11000);

            Assert.AreEqual(eventCountActual, event504ReverralAfterLock);
            Assert.AreEqual(allEventCount + 1, allEventCountActual);

            _driver.Close();
        }

        private static void AddTimeOut(int timeOut)
        {
            Thread.Sleep(timeOut);
        }

        private void CreateEvent504Reverral()
        {
            AddTimeOut(2000);
            _createEventElements.CreateEventButton.Click();

            AddTimeOut(6000);
            _createEventElements.SelectEventInputElement.Click();

            ((IJavaScriptExecutor)_driver).ExecuteScript(String.Format("$('#{0}').data('kendoDropDownList').select({1});", "EventGroupDefinitionId", 3));
            _createEventElements.Referral504OptionSelect.Click();

            _createEventElements.DateControl.SendKeys("11/15/2015 12:00 AM");
            _createEventElements.SaveEventButton.Click();
            AddTimeOut(5000);
        }

        private void AddValueToFields()
        {
            AddTimeOut(5000);
            var fieldsInput = _createEventElements.FieldsInputRequired;
            InputValueForFields(fieldsInput);

            var fieldsTestarea = _createEventElements.FieldsTextAreaRequired;
            InputValueForFields(fieldsTestarea);

            AddTimeOut(2000);
            _createEventElements.UpdateFormButton.Click();
            AddTimeOut(5000);
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
           AddTimeOut(7000);
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

        private void GetEvent()
        {
            AddTimeOut(10000);
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
