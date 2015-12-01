using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;

namespace CreateEventTests
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

            Driver.FindElement(By.Id("plcContent_LoginControl_UserName")).SendKeys("superlion");
            Driver.FindElement(By.Id("plcContent_LoginControl_Password")).SendKeys("CTAKAH");

            Driver.FindElement(By.Id("plcContent_LoginControl_lnkLogin")).Click();

        }

        [TestMethod]
        public void CreateEvent504Reverral()
        {
            Thread.Sleep(1000);
            var homeElement = Driver.FindElement(By.CssSelector("a[href='Home.aspx']"));
           // Driver.ExecuteScript("arguments[0].scrollIntoView(true);", homeElement);
            Thread.Sleep(500);

            Actions actions = new Actions(Driver);
            actions.MoveToElement(homeElement);
            //actions.click();
            actions.Perform();

            var spedElement = Driver.FindElement(By.CssSelector("a[href='../IEP/']"));
            spedElement.Click(); 

            Thread.Sleep(3000);
            var hampton504Tab = Driver.FindElement(By.CssSelector("[aria-controls='pnlMultipleProgramHome-2']"));
            hampton504Tab.Click();
        }
    }
}
