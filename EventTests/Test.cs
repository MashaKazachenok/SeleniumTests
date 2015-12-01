using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;

namespace EventTests
{
    [TestClass]
    public class Test
    {
        private const string BaseUrl = "http://hampton-demo.accelidemo.com/";

        private readonly FirefoxDriver _driver = new FirefoxDriver();

        [TestInitialize]
        public void Start()
        {
            _driver.Navigate().GoToUrl(BaseUrl + "/Login.aspx");

            _driver.FindElement(By.Id("plcContent_LoginControl_UserName")).SendKeys("superlion");
            _driver.FindElement(By.Id("plcContent_LoginControl_Password")).SendKeys("CTAKAH");
            _driver.FindElement(By.Id("plcContent_LoginControl_lnkLogin")).Click();

        }

        [TestMethod]
        public void LockEvent504ReverraL()
        {
            AddTimeOut(2000);
            var homeElement = _driver.FindElementByLinkText("Home");
            AddTimeOut(500);

            Actions actions = new Actions(_driver);
            actions.MoveToElement(homeElement).Build().Perform();

            var spedElement = _driver.FindElement(By.CssSelector("a[href='../IEP/']"));
            spedElement.Click();

            AddTimeOut(7000);
            var studentsTab = _driver.FindElement(By.CssSelector("a[href='/IEP/Students']"));

            AddTimeOut(2000);
            studentsTab.Click();

            AddTimeOut(6000);
            GetStudent();

            GetEvent();
            AddTimeOut(6000);
            var eventsCount = _driver.FindElementsByLinkText("504 Referral").Count; 

            CreateEvent504Reverral();
            AddTimeOut(5000);

            var eventCountActual = _driver.FindElementsByLinkText("504 Referral").Count;
            AddTimeOut(1500);
            var eventCountExpected = eventsCount + 1;
            var allEventCount = _driver.FindElements(By.ClassName("k-reset")).Count;

            Assert.AreEqual(eventCountExpected, eventCountActual);

            AddTimeOut(3000);
            var refferalEventLinks = _driver.FindElementsByLinkText("504 Referral");
            AddTimeOut(2000);
            refferalEventLinks[0].Click();

            AddTimeOut(3000);
            var formLink = _driver.FindElement(By.CssSelector("a[href='#Section504ReferralForm']"));

            AddTimeOut(3000);
            formLink.Click();
            AddTimeOut(10000);

            AddValueToField();
            AddTimeOut(5000);

            var lockButton = _driver.FindElement(By.Id("btnLockEvent"));
            lockButton.Click();
            AddTimeOut(2000);

            _driver.SwitchTo().Alert().Accept();
            AddTimeOut(8000);

            var event504ReverralAfterLock = _driver.FindElementsByLinkText("504 Referral").Count; 
            var allEventCountActual = _driver.FindElements(By.ClassName("k-reset")).Count;
            AddTimeOut(3000);

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

            var createButtton = _driver.FindElement(By.Id("btnCreateEventGroup"));
            AddTimeOut(2000);
            createButtton.Click();

            AddTimeOut(3000);
            var selectEvent = _driver.FindElementByClassName("k-input");
            AddTimeOut(1000);
            selectEvent.Click();
            ((IJavaScriptExecutor) _driver).ExecuteScript(String.Format("$('#{0}').data('kendoDropDownList').select({1});",
                "EventGroupDefinitionId", 3));
            var referralEvent = _driver.FindElement(By.CssSelector("[data-offset-index='3']"));
            AddTimeOut(500);
            referralEvent.Click();

            var dateControl = _driver.FindElement(By.Id("ScheduleDate"));
            AddTimeOut(1000);
            dateControl.SendKeys("11/15/2015 12:00 AM");

            var saveButtton = _driver.FindElement(By.Id("btnSaveEventGroup"));
            AddTimeOut(2000);

            saveButtton.Click();
        }

        private void AddValueToField()
        {
            var fieldsInput = _driver.FindElementsByCssSelector("input[required='required']");

            foreach (var field in fieldsInput)
            {
                field.Clear();
                field.SendKeys("1");
            }

            var fieldsTestarea = _driver.FindElementsByCssSelector("textarea[required='required']");

            foreach (var field in fieldsTestarea)
            {
                field.Clear();
                field.SendKeys("1");
            }

            var updateButton = _driver.FindElement(By.Id("btnUpdateForm"));
            updateButton.Click();
        }

        private void GetStudent()
        {
            var links = _driver.FindElementsByTagName("a");
            AddTimeOut(3000);

            for (int i = 0; i < links.Count; i++)
            {
                var hrefAtr = links[i].GetAttribute("href");

                if (hrefAtr != null)
                {
                    var hrefStudents = hrefAtr.Contains("/IEP/Students/ViewStudent");

                    if (hrefStudents)
                    {
                        AddTimeOut(3000);
                        links[i].Click();
                        break;
                    }
                }
            }
        }

        private void GetEvent()
        {
            AddTimeOut(4000);
            var eventsLink = _driver.FindElements(By.PartialLinkText("Events"));
            AddTimeOut(5000);

            for (int i = 0; i < eventsLink.Count; i++)
            {
                var hrefElementHampton504 = eventsLink[i].GetAttribute("href").Contains("Events&programType=Hampton504");

                if (hrefElementHampton504)
                {
                    AddTimeOut(3000);
                    eventsLink[i].Click();
                    break;
                }
            }
        }
    }
}
