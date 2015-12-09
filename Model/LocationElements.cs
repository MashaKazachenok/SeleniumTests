using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System.Collections.Generic;

namespace Model
{
    public class LocationElements
    {
        private readonly IWebDriver _webDriver;

        public LocationElements(IWebDriver webDriver)
        {
            this._webDriver = webDriver;
            PageFactory.InitElements(webDriver, this);
        }

        private const string HomeLinkText = "Home";
        private const string SpedElCssSel = "a[href='../IEP/']";
        private const string StudentsTabCssSel = "a[href='/IEP/Students']";
        private const string Reverral504LinkText = "504 Referral";
        private const string CreateEventButtonId = "btnCreateEventGroup";
        private const string SaveEventButtonId = "btnSaveEventGroup";
        private const string DateControlId = "ScheduleDate";
        private const string SelectEventInputClassName = "k-input";
        private const string Referral504OptionSelectCssSel = "[data-offset-index='3']";
        private const string AllEventsClassName = "k-reset";
        private const string FormLinkCssSelector = "a[href='#Section504ReferralForm']";
        private const string FieldsInputRequiredCssSelector = "input[required='required']";
        private const string FieldsTextAreaRequiredCssSelector = "textarea[required='required']";
        private const string UpdateFormButtonId = "btnUpdateForm";
        private const string LockButtonId = "btnLockEvent";
        private const string LinksCssSelector = "a[href]";
        private const string EventLinksLinkText = "Events";
        private const string UnLockButtonId = "btnUnlockEvent";

        [FindsBy(How = How.LinkText, Using = HomeLinkText)]
        public IWebElement HomeElement { get; set; }

        [FindsBy(How = How.CssSelector, Using = SpedElCssSel)]
        public IWebElement SpedElement { get; set; }

        [FindsBy(How = How.CssSelector, Using = StudentsTabCssSel)]
        public IWebElement StudentsTab { get; set; }

        [FindsBy(How = How.LinkText, Using = Reverral504LinkText)]
        public IList<IWebElement> Reverral504Elements { get; set; }

        [FindsBy(How = How.Id, Using = CreateEventButtonId)]
        public IWebElement CreateEventButton { get; set; }

        [FindsBy(How = How.Id, Using = SaveEventButtonId)]
        public IWebElement SaveEventButton { get; set; }

        [FindsBy(How = How.Id, Using = DateControlId)]
        public IWebElement DateControl { get; set; }

        [FindsBy(How = How.ClassName, Using = SelectEventInputClassName)]
        public IWebElement SelectEventInputElement { get; set; }

        [FindsBy(How = How.CssSelector, Using = Referral504OptionSelectCssSel)]
        public IWebElement Referral504OptionSelect { get; set; }

        [FindsBy(How = How.ClassName, Using = AllEventsClassName)]
        public IList<IWebElement> AllEventsElements { get; set; }

        [FindsBy(How = How.CssSelector, Using = FormLinkCssSelector)]
        public IWebElement FormLink { get; set; }

        [FindsBy(How = How.CssSelector, Using = FieldsInputRequiredCssSelector)]
        public IList<IWebElement> FieldsInputRequired { get; set; }

        [FindsBy(How = How.CssSelector, Using = FieldsTextAreaRequiredCssSelector)]
        public IList<IWebElement> FieldsTextAreaRequired { get; set; }

        [FindsBy(How = How.Id, Using = UpdateFormButtonId)]
        public IWebElement UpdateFormButton { get; set; }

        [FindsBy(How = How.Id, Using = LockButtonId)]
        public IWebElement LockButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = LinksCssSelector)]
        public IList<IWebElement> Links { get; set; }

        [FindsBy(How = How.LinkText, Using = EventLinksLinkText)]
        public IList<IWebElement> EventLinks { get; set; }

        public IList<IWebElement> Event504ReverralLinksFromFirstTable
        {
            get
            {
                return
                    _webDriver.FindElement(By.XPath("//div[@id='pnlEventList']")).FindElements(By.LinkText("504 Referral"));
            }
        }

        public IList<IWebElement> Event504ReverralLinksLock
        {
            get
            {
                return
                    _webDriver.FindElement(By.XPath("//div[@id='pnlEventLockedList']")).FindElements(By.LinkText("504 Referral"));
            }
        }

        public IList<IWebElement> Event504EligibilityMeetingLinks
        {
            get
            {
                return
                    _webDriver.FindElement(By.XPath("//div[@id='pnlEventList']")).FindElements(By.LinkText("504 Eligibility Meeting"));
            }
        }

        [FindsBy(How = How.Id, Using = UnLockButtonId)]
        public IWebElement UnLockButton { get; set; }

        public IWebElement DeleteEligibilityEvent
        {
            get
            {
                return
                    _webDriver.FindElement(By.XPath("//div[@id='pnlEventList']//table//tr//td[contains(text(),'Eligibility Meeting')]/following-sibling::td[7]"));
            }
        }

        public IWebElement DeleteEvent504Referral
        {
            get
            {
                return
                    _webDriver.FindElement(By.XPath("//div[@id='pnlEventList']//table//tr//td[contains(text(),'504 Referral')]/following-sibling::td[7]"));
            }
        }

    }
}
