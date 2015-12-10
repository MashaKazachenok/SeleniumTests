using System;
using System.Collections.Generic;
using System.Threading;
using Model;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace EventTests
{
    public class EventService
    {
        private static readonly FirefoxDriver Driver = new FirefoxDriver();
        private readonly LocationElements _locationElements = new LocationElements(Driver);
        private readonly WebDriverWait _wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(30));

         public static FirefoxDriver GetDriver()
         {
             return Driver;
         }
        public  void AddTimeOut(int timeOut)
        {
            Thread.Sleep(timeOut);
        }

        public void CreateEvent(int eventNumber)
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

        public void AddValueToFields()
        {
            WaitIsVisibleAndClickable(By.Id("btnUpdateForm"));
            AddTimeOut(2000);

            var fieldsInput = _locationElements.FieldsInputRequired;
            InputValueForFields(fieldsInput);

            var fieldsTestarea = _locationElements.FieldsTextAreaRequired;
            InputValueForFields(fieldsTestarea);

            _locationElements.UpdateFormButton.Click();
        }

        public static void InputValueForFields(IList<IWebElement> fields)
        {
            foreach (var field in fields)
            {
                field.Clear();
                field.SendKeys("1");
            }
        }

        public void GetStudent()
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

        public void GetEvent()
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

        public void WaitIsVisibleAndClickable(By location)
        {
            _wait.Until(ExpectedConditions.ElementIsVisible(location));
            _wait.Until(ExpectedConditions.ElementToBeClickable(location));
        }
        public void LockEvent()
        {
            WaitIsVisibleAndClickable(By.CssSelector("[required='required']"));
            WaitIsVisibleAndClickable(By.Id("btnUpdateForm"));
            AddTimeOut(4000);
            _locationElements.LockButton.Click();

            Driver.SwitchTo().Alert().Accept();
            WaitIsVisibleAndClickable(By.Id("btnCreateEventGroup"));
            AddTimeOut(3000);
        }
    }
}
