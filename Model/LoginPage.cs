using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Model
{
    public class LoginPage
    {
        public LoginPage(IWebDriver webDriver)
        {
            this._webDriver = webDriver;
            PageFactory.InitElements(webDriver, this);
        }

        private IWebDriver _webDriver;
        private const string UserNameButtonId = "plcContent_LoginControl_UserName";
        private const string PasswordButtonId = "plcContent_LoginControl_Password";
        private const string SubmitButtonId = "plcContent_LoginControl_lnkLogin";

        [FindsBy(How = How.Id, Using = UserNameButtonId)]
        public IWebElement UserName { get; set; }
        [FindsBy(How = How.Id, Using = PasswordButtonId)]
        public IWebElement Password { get; set; }

        [FindsBy(How = How.Id, Using = SubmitButtonId)]
        public IWebElement SubmitButton { get; set; }
    }
}
