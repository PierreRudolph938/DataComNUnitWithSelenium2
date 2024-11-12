using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using OpenQA.Selenium.Firefox;

namespace DataComSeleniumTestProject
{
    public class Tests
    {
        private const string browserName = "Chromium"; //"Firefox";  
        private const string chromeDriverPath = @"C:\SeleniumDrivers\chromedriver-win64\";
        private const string firefoxDriverPath = @"C:\SeleniumDrivers\geckodriver-v0.35.0-win64\";

        private IWebDriver Driver { get; set; }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            //string path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

            if (browserName == "Chromium")
            {
                var chromeOptions = new ChromeOptions();
                // Set the geolocation permission to "allow"
                chromeOptions.AddUserProfilePreference("profile.default_content_setting_values.geolocation", 1); // 1 allows, 2 blocks
                Driver = new ChromeDriver(chromeDriverPath, chromeOptions);
            }
            else if (browserName == "Firefox")
            {
                var firefoxOptions = new FirefoxOptions();
                // Enable geolocation and auto-approve
                firefoxOptions.SetPreference("geo.prompt.testing", true);
                firefoxOptions.SetPreference("geo.prompt.testing.allow", true);
                firefoxOptions.SetPreference("geo.enabled", true);

                Driver = new FirefoxDriver(firefoxDriverPath, firefoxOptions);
            }
            else
            {
                var chromeOptions = new ChromeOptions();
                // Set the geolocation permission to "allow"
                chromeOptions.AddUserProfilePreference("profile.default_content_setting_values.geolocation", 1); // 1 allows, 2 blocks
                Driver = new ChromeDriver(chromeDriverPath, chromeOptions);
            }

            Assert.IsNotNull(Driver);

            Driver.Manage().Window.Maximize();
            Driver.Url = "https://datacom.com/nz/en/contact-us";

            //Except all cookies
            try
            {
                WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(5));
                IWebElement acceptCookies = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div#onetrust-policy")));

                if (acceptCookies.Displayed)
                {
                    var acceptAll = Driver.FindElement(By.XPath("//button[@id='onetrust-accept-btn-handler' and contains(text(), 'Accept all')]"));
                    acceptAll.Click();
                }
            }
            catch (NoSuchElementException)
            {
                //Log("No cookie consent pop-up found.");
            }
            catch (WebDriverTimeoutException)
            {
                //Log("Timed out waiting for the cookie consent pop-up.");
            }
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Driver?.Dispose();
        }

        [SetUp]
        public void SetUp()
        {
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void TestLoad_ContactUs()
        {
            var logoElement = Driver.FindElement(By.CssSelector("img[alt='Datacom logo']"));
            Assert.IsTrue(logoElement.Displayed);
            Assert.That(logoElement.GetAttribute("src"), Is.EqualTo("https://assets.datacom.com/is/content/datacom/Datacom-Primary-Logo-RGB?$header-mega-logo$"));
        }

        [Test]
        public void TestLoadOurLocations()
        {
            //await Page.GotoAsync("https://datacom.com/nz/en/contact-us");

            //Main Logo
            var logoLocator = Driver.FindElement(By.CssSelector("img[alt='Datacom logo']"));
            Assert.IsTrue(logoLocator.Displayed);
            Assert.That(logoLocator.GetAttribute("src"), Is.EqualTo("https://assets.datacom.com/is/content/datacom/Datacom-Primary-Logo-RGB?$header-mega-logo$"));

            //Header Our Locations and text description
            var locationHdrLocator = Driver.FindElement(By.CssSelector("h2.cmp-title__text"));
            Assert.IsTrue(locationHdrLocator.Displayed);

            var locationText = Driver.FindElement(By.XPath("//div[contains(@class, 'cmp-text')]//p[contains(text(), 'Contact one of our global offices')]"));
            Assert.That(locationText.Text, Is.EqualTo("Contact one of our global offices or one of our teams to find out more about how we can help you, or to answer any query you may have."));

            //Location tab elements - New Zealand, Australia, Asia
            var defaultLocation = Driver.FindElement(By.CssSelector("li.cmp-location__nav__items__item[data-tab-section-id='.item0']"));
            Assert.That(defaultLocation.Text, Is.EqualTo("New Zealand"));
            Assert.That(defaultLocation.GetAttribute("class"), Does.Contain("cmp-location__nav__items__item"));
            Assert.That(defaultLocation.GetAttribute("class"), Does.Contain("active"));
            Assert.That(defaultLocation.GetAttribute("class"), Does.Contain("focus-border-bottom"));
            Assert.IsTrue(defaultLocation.Displayed);
        }

        [Test]
        public void TestNewZealandContactDetail()
        {
            //Adress detail in New Zealand list
            var addressElements = Driver.FindElements(By.CssSelector("div.item0.cmp-location__location[region='New Zealand'] p.cmp-location__location__address"));
            Assert.That(addressElements.Count, Is.EqualTo(10));
            Assert.That(addressElements.First().Text, Is.EqualTo("58 Gaunt Street, Auckland CBD, Auckland 1010"));

            var phoneElements = Driver.FindElements(By.CssSelector("div.item0.cmp-location__location[region='New Zealand'] p.cmp-location__location__phone"));
            Assert.That(phoneElements.First().Text, Is.EqualTo("+64 9 303 1489"));

            var emailElements = Driver.FindElements(By.CssSelector("div.item0.cmp-location__location[region='New Zealand'] p.cmp-location__location__email"));
            Assert.That(emailElements.First().Text, Is.EqualTo("reception.gaunt@datacom.co.nz"));

            //await Expect(addressLocator.First).ToHaveTextAsync("58 Gaunt Street, Auckland CBD, Auckland 1010");
            //await Expect(addressLocator.Nth(1)).ToHaveTextAsync("67 Gloucester Street, Christchurch 8013");
            //await Expect(addressLocator.Nth(2)).ToHaveTextAsync("Level 1, 77 Vogel Street, Dunedin 9011");
            //await Expect(addressLocator.Nth(3)).ToHaveTextAsync("Level 2, 94 Bryce Street, Hamilton 3204");
            //await Expect(addressLocator.Nth(4)).ToHaveTextAsync("2/117 Heretaunga Street East, Hastings 4122");
            //await Expect(addressLocator.Nth(5)).ToHaveTextAsync("Level 1, 190 Trafalgar Street, Nelson 7010");
            //await Expect(addressLocator.Nth(6)).ToHaveTextAsync("Level 1, 2 Devon Street East, New Plymouth 4310");
            //await Expect(addressLocator.Nth(7)).ToHaveTextAsync("8 Railway Road, Rotorua 3015");
            //await Expect(addressLocator.Nth(8)).ToHaveTextAsync("15-17 Harington Street, Tauranga 3110");
            //await Expect(addressLocator.Nth(9)).ToHaveTextAsync("55 Featherston Street, Pipitea, Wellington 6011,");

            //    //Phone detail in New Zealand list
            //    var phoneLocator = Page.Locator("div.item0.cmp-location__location[region = 'New Zealand'] >> p.cmp-location__location__phone");
            //    await Expect(phoneLocator).ToHaveCountAsync(10);
            //    await Expect(phoneLocator.First).ToHaveTextAsync("+64 9 303 1489");
            //    await Expect(phoneLocator.Nth(1)).ToHaveTextAsync("+64 3 379 7775");
            //    await Expect(phoneLocator.Nth(2)).ToHaveTextAsync("+64 3 379 7775");
            //    await Expect(phoneLocator.Nth(3)).ToHaveTextAsync("+64 7 834 1666");
            //    await Expect(phoneLocator.Nth(4)).ToHaveTextAsync("+64 6 835 0793");
            //    await Expect(phoneLocator.Nth(5)).ToHaveTextAsync("+64 3 546 5558");
            //    await Expect(phoneLocator.Nth(6)).ToHaveTextAsync("+64 7 834 1666");
            //    await Expect(phoneLocator.Nth(7)).ToHaveTextAsync("+64 7 834 1666");
            //    await Expect(phoneLocator.Nth(8)).ToHaveTextAsync("+64 7 834 1666");
            //    await Expect(phoneLocator.Nth(9)).ToHaveTextAsync("+64 4 460 1500");

            //    //Email detail in New Zealand list
            //    var emailLocator = Page.Locator("div.item0.cmp-location__location[region = 'New Zealand'] >> p.cmp-location__location__email");
            //    await Expect(emailLocator).ToHaveCountAsync(10);
            //    await Expect(emailLocator.First).ToHaveTextAsync("reception.gaunt@datacom.co.nz");
            //    await Expect(emailLocator.Nth(1)).ToHaveTextAsync("reception.christchurch@datacom.co.nz");
            //    await Expect(emailLocator.Nth(2)).ToHaveTextAsync("reception.christchurch@datacom.co.nz");
            //    await Expect(emailLocator.Nth(3)).ToHaveTextAsync("reception.hamilton@datacom.co.nz");
            //    await Expect(emailLocator.Nth(4)).ToHaveTextAsync("reception.hamilton@datacom.co.nz");
            //    await Expect(emailLocator.Nth(5)).ToHaveTextAsync("reception.nelson@datacom.co.nz");
            //    await Expect(emailLocator.Nth(6)).ToHaveTextAsync("reception.hamilton@datacom.co.nz");
            //    await Expect(emailLocator.Nth(7)).ToHaveTextAsync("reception.hamilton@datacom.co.nz");
            //    await Expect(emailLocator.Nth(8)).ToHaveTextAsync("reception.tauranga@datacom.co.nz");
            //    await Expect(emailLocator.Nth(9)).ToHaveTextAsync("reception.wellington@datacom.co.nz");
        }


        [Test]
        public void TestAustraliaContactDetail()
        {
            var listItems = Driver.FindElements(By.CssSelector("ul.cmp-location__nav__items > li[data-tab-section-id='.item1']"));

            // Filter for the item with the text "Asia"
            var australiaItem = listItems.FirstOrDefault(item => item.Text.Contains("Australia"));
            if (australiaItem != null) australiaItem.Click();

            //Adress detail in Australia list
            //var addressLocator = Page.Locator("div.item1.cmp-location__location[region = 'Australia'] >> p.cmp-location__location__address");
            var addressElements = Driver.FindElements(By.CssSelector("div.item1.cmp-location__location[region='Australia'] p.cmp-location__location__address"));
            Assert.That(addressElements.Count, Is.EqualTo(8));
            Assert.That(addressElements.First().Text, Is.EqualTo("118 Franklin Street, Adelaide, South Australia 5000"));

            var phoneElements = Driver.FindElements(By.CssSelector("div.item1.cmp-location__location[region='Australia'] p.cmp-location__location__phone"));
            Assert.That(phoneElements.First().Text, Is.EqualTo("+61 8 7221 7900"));

            var emailElements = Driver.FindElements(By.CssSelector("div.item1.cmp-location__location[region='Australia'] p.cmp-location__location__email"));
            Assert.That(emailElements.First().Text, Is.EqualTo("contactsa@datacom.com.au"));
        }

        [Test]
        public void TestAsiaRegionalContactNavigation()
        {
            var listItems = Driver.FindElements(By.CssSelector("ul.cmp-location__nav__items > li[data-tab-section-id='.item2']"));

            // Filter for the item with the text "Asia"
            var asiaItem = listItems.FirstOrDefault(item => item.Text.Contains("Asia"));
            if (asiaItem != null) asiaItem.Click();

            var malaysiaAddress = Driver.FindElement(By.CssSelector("div.item2.cmp-location__location[region='Asia'] p.cmp-location__location__address"));
            Assert.That(malaysiaAddress.Displayed);
            Assert.That(malaysiaAddress.Text, Is.EqualTo("Level 3A, 1 Sentral, Jalan Rakyat, Kuala Lumpur Sentral, Kuala Lumpur 50470"));

            //var malaysiaPhone = Driver.FindElement(By.CssSelector(".item2 #section-0 > p.cmp-location__location__phone"));
            var malaysiaPhone = Driver.FindElement(By.CssSelector("div.item2.cmp-location__location[region='Asia'] p.cmp-location__location__phone"));
            Assert.That(malaysiaPhone.Displayed);
            Assert.That(malaysiaPhone.Text, Is.EqualTo("+60 3 2109 1000"));

            var malaysiaEmail = Driver.FindElement(By.CssSelector("div.item2.cmp-location__location[region='Asia'] p.cmp-location__location__email"));
            Assert.That(malaysiaEmail.Displayed);
            Assert.That(malaysiaEmail.Text, Is.EqualTo("info-kl@datacom.com.au"));


            var singaporeContact = Driver.FindElement(By.CssSelector("div.item2[region='Asia'] > div#section-2 > div.cmp-location__location__name.focus-outline-no-offset-location"));
            if (singaporeContact != null) singaporeContact.Click();

            //# section-2 > div.cmp-location__location__details > div:nth-child(1) > p.cmp-location__location__address    #section-2 > div.cmp-location__location__details
            //var singaporeAddress = Driver.FindElement(By.CssSelector("#section-2 > div.cmp-location__location__details > div:nth-child(1) > p.cmp-location__location__address"));
            var singaporeAddress = Driver.FindElement(By.CssSelector("div.item2[region='Asia'] > div#section-2 > div.cmp-location__location__details > div > p.cmp-location__location__address"));
            Assert.That(singaporeAddress.Displayed);
            Assert.That(singaporeAddress.Text, Is.EqualTo("38 Beach Road, South Beach Tower, #29-11 Singapore 189767"));

            var singaporePhone = Driver.FindElement(By.CssSelector("div.item2[region='Asia'] > div#section-2 > div.cmp-location__location__details > p.cmp-location__location__phone"));
            Assert.That(singaporePhone.Displayed);
            Assert.That(singaporePhone.Text, Is.EqualTo("+60 3 2109 1000"));

            //await Expect(Page.GetByText("felicisimo.gadaingan@datacom.com.au")).ToBeVisibleAsync();
            var singaporeEmail = Driver.FindElement(By.CssSelector("div.item2[region='Asia'] > div#section-2 > div.cmp-location__location__details > p.cmp-location__location__email"));
            Assert.That(singaporeEmail.Displayed);
            Assert.That(singaporeEmail.Text, Is.EqualTo("felicisimo.gadaingan@datacom.com.au"));
        }

        [Ignore("This test is still under development")]
        public void TestContactUs()
        {
            if (Driver == null) return;

            Driver.FindElement(By.Id("#cmp-mrkto-modal-thank-you")).Click();    //GetByText("Contact us")

            var firstName = Driver.FindElement(By.Name("*First name"));
            firstName.Click();
            firstName.SendKeys("Anton");

            var lastName = Driver.FindElement(By.Name("*Last name"));
            lastName.Click();
            lastName.SendKeys("Ohlson");

            var businessEmail = Driver.FindElement(By.Name("*Business email"));
            businessEmail.Click();
            businessEmail.SendKeys("anton.ohlson@yahoo.com");

            var phoneNumber = Driver.FindElement(By.Name("*Phone number"));
            phoneNumber.Click();
            phoneNumber.SendKeys("0210687777");

            var jobTitle = Driver.FindElement(By.Name("*Job title"));
            jobTitle.Click();
            jobTitle.SendKeys("QA Analyst");

            var companyName = Driver.FindElement(By.Name("*Company name"));
            companyName.Click();
            companyName.SendKeys("CountDown");

            var country = Driver.FindElement(By.Name("*Country"));
            country.Click();
            country.SendKeys("New Zealand");

            SelectElement stateList = new SelectElement(Driver.FindElement(By.Name("*State")));
            stateList.SelectByText("Christchurch");

            SelectElement careerList = new SelectElement(Driver.FindElement(By.Name("*How can we help you?")));
            careerList.SelectByText("Careers");

            SelectElement careerType = new SelectElement(Driver.FindElement(By.Name("*What type of career are you looking for?")));
            careerType.SelectByText("Internship");

            var role = Driver.FindElement(By.Name("*What role/business area are you interested in?"));
            role.Click();
            role.SendKeys("QA Analyst");

            var submit = Driver.FindElement(By.Name("Submit"));
            submit.Click();
        }
    }
}