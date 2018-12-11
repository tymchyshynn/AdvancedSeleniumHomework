using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace AdvansedSeleniumHomework1
{
    [TestFixture]
    public class Homework
    {
        IWebDriver driver;

        [SetUp]
        public void SetUp()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments
                (
                    "--headless",
                    "--start-maximized"         
                );

            //Creating the driver
            TestContext.WriteLine("Creating driver with headles option");
            driver = new ChromeDriver(chromeOptions);

        }

        [TearDown]
        public void TearDown()
        {
            TestContext.WriteLine("Kill driver");
            driver.Quit();
        }


        [Test]
        [Author("Vasyl Tymchyshyn")]
        public void OneLargeTest()
        {
            //Locators on Home page
            By hyperLinkPageLocator = By.LinkText("HyperLink");

            //Open Leafground page 
            TestContext.WriteLine("Go to Leafground page");
            driver.Navigate().GoToUrl("http://www.leafground.com/home.html");

            //Open HyperLink page in new tab
            TestContext.WriteLine("Open HyperLink page in new tab");
            new Actions(driver).KeyDown(Keys.Control).Click(driver.FindElement(hyperLinkPageLocator)).KeyUp(Keys.Control).Perform();

            //Switch to second tab
            TestContext.WriteLine("Switch to second tab");
            driver.SwitchTo().Window(driver.WindowHandles[1]);

            //Locators on HyperLink page 
            var homePageLinkLocator = driver.FindElements(By.XPath("//a[text()='Go to Home Page']")).First();

            //Hover on "Go to Home Page" link
            TestContext.WriteLine("Hover on 'Go to Home Page' link");
            new Actions(driver).MoveToElement(homePageLinkLocator).Perform();

            //Take a screenshot
            TestContext.WriteLine("Take a screenshot");
            var screenshot = ((ITakesScreenshot)driver).GetScreenshot();

            //Saving a screenshoot 
            TestContext.WriteLine("Save a screenshot");
            var destinationPath = AppDomain.CurrentDomain.BaseDirectory;
            var screenshotPath = Path.Combine(destinationPath, "screenshot.png");
            screenshot.SaveAsFile(screenshotPath);

            //Adding screenshot to the test output.
            TestContext.AddTestAttachment(screenshotPath);

            //Close the second tab
            TestContext.WriteLine("Close the second tab");
            driver.Close();

            //Switch to first tab
            TestContext.WriteLine("Switch to first tab");
            driver.SwitchTo().Window(driver.WindowHandles[0]);

            //Open jQuery UI Demos page 
            TestContext.WriteLine("Go to jQuery UI Demos page");
            driver.Navigate().GoToUrl("https://jqueryui.com/demos/");

            //"Droppable" item locator
            var droppableLocator = driver.FindElement(By.XPath("//a[@href='https://jqueryui.com/droppable/']"));

            //Navigate to "Droppable" demo (Interactions section)
            TestContext.WriteLine("Navigate to 'Droppable' demo (Interactions section)");
            droppableLocator.Click();

            //Frame locators
            By frameLocator = By.CssSelector("iframe[src='/resources/demos/droppable/default.html']");
            By dropTargetLocator = By.Id("droppable");
            By draggableLocator = By.Id("draggable");
            By successText = By.CssSelector("div#droppable p");

            //Switch to frame
            TestContext.WriteLine("Switch to frame");
            driver.SwitchTo().Frame(driver.FindElement(frameLocator));

            //Finding target and draggable elements
            var dropTargetElement = driver.FindElement(dropTargetLocator);
            var draggableElement = driver.FindElement(draggableLocator);

            //Drag & Drop the small box into a big one
            TestContext.WriteLine("Drag & Drop the small box into a big one");
            //new Actions(driver).DragAndDrop(draggableElement, dropTargetElement).Perform();
            new Actions(driver).ClickAndHold(draggableElement).MoveToElement(dropTargetElement).MoveByOffset(0, 23).Release().Perform();

            //Verify that big box now contains text "Dropped!"
            TestContext.WriteLine("Verify that big box now contains text 'Dropped!'");
            Assert.That(driver.FindElement(successText).Text, Is.EqualTo("Dropped!"));

            //Closing window
            TestContext.WriteLine("Close window");
            driver.Close();
        }

    }
}
