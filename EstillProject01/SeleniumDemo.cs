using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace TestProjectEMich
{
    // Chrome, Firefox, IE, Safari, Edge, Opera
    //
    // latest-beta, latest, latest - 1, latest - 2
    //
    // Windows, OS X
    // XP, 7, 8, 8.1 and 10
    // Snow Leopard, Lion, Mountain Lion, Mavericks, Yosemite, El Capitan, Sierra, High Sierra, Mojave, Catalina (10.15)
    //[TestFixture("chrome", "latest", "pixel 4", "11.0")]
    //[TestFixture("iphone 11", "14")]
    //
    [TestFixture("chrome", "87.0", "Windows", "10")]
    [TestFixture("chrome", "83.0", "Windows", "10")]
    [TestFixture("chrome", "81.0", "Windows", "10")]
    [TestFixture("chrome", "80.0", "Windows", "10")]
    [TestFixture("MicrosoftEdge", "86.0", "Windows", "10")]
    [TestFixture("MicrosoftEdge", "83.0", "Windows", "10")]
    [TestFixture("MicrosoftEdge", "81.0", "Windows", "10")]
    [TestFixture("MicrosoftEdge", "80.0", "Windows", "10")]
    [TestFixture("internet explorer", "11.0", "Windows", "10")]
    [TestFixture("Firefox", "83.0", "Windows", "10")]
    [TestFixture("Firefox", "77.0", "Windows", "10")]
    [TestFixture("Firefox", "76.0", "Windows", "10")]
    [TestFixture("Firefox", "75.0", "Windows", "10")]
    [TestFixture("Safari", "13.1", "OS X", "Catalina")]
    [TestFixture("Safari", "12.1", "OS X", "Mojave")]
    [TestFixture("Safari", "11.1", "OS X", "High Sierra")]
    [Parallelizable(ParallelScope.All)]
    
    //


    public class SeleniumDemo
    {
        //specify the test URL
        String test_url = "https://cmstest.michigan.gov/uatsite1/The-Team/Estill-UAT-Page/4-33-Off-Domain-Link";
        string test_name = "Estill_4_33";
        IWebDriver driver;
        private String browser;
        private String version;
        private String os;
        private String osVersion;

        public SeleniumDemo(String browser, String version, String os, String osVersion)
        {
            this.browser = browser;
            this.version = version;
            this.os = os;
            this.osVersion = osVersion;
        }

        [SetUp]
        public void SetConfig()
        {
            string USERNAME = "johnestill1";
            string AUTOMATE_KEY = "vVwRLsPYFnBPCJwe2mrC";

            DesiredCapabilities caps = new DesiredCapabilities();

            caps.SetCapability("os", os);
            caps.SetCapability("os_version", osVersion);
            caps.SetCapability("browser", browser);
            caps.SetCapability("browser_version", version);

            //   Additional capability for minimum browser width because some are coming in at the 970 breakpoint by default
            //   1280x1024 is common between OS X & Windows and provids sufficient width
            caps.SetCapability("resolution", "1280x1024");


            caps.SetCapability("browserstack.user", USERNAME);
            caps.SetCapability("browserstack.key", AUTOMATE_KEY);
            caps.SetCapability("build", "nunit-browserstack");
            caps.SetCapability("browserstack.debug", "true");
            caps.SetCapability("build", "unit-browserstack");
            caps.SetCapability("name", test_name + "_" + os + "_" + osVersion + "_" + browser + "_" + version + "_" + DateTime.Now.ToShortTimeString());




            driver = new RemoteWebDriver(
              new Uri("https://hub-cloud.browserstack.com/wd/hub/"), caps
           );
        }

        [Test]

        public void Run_Test()
        {
            //  Max the window to take advantage of the definded width
            driver.Manage().Window.Maximize();

            //
            driver.Navigate().GoToUrl(test_url);
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver; 
            //js.ExecuteScript("window.scrollTo(0, Math.max(document.documentElement.scrollHeight, document.body.scrollHeight, document.documentElement.clientHeight));");
            Console.WriteLine(driver.Title);
            //System.Threading.Thread.Sleep(5000);

            //   Bottom most id:  id=footer4

            //attempt to get height of window and height of page
            //  implicit typing wont let me assign the return to an integer

            //Console.WriteLine((string)((IJavaScriptExecutor)driver).ExecuteScript("return document.documentElement.scrollHeight"));
            IJavaScriptExecutor client_height = (IJavaScriptExecutor)driver; client_height.ExecuteScript("return document.documentElement.clientHeight");
            IJavaScriptExecutor client_width = (IJavaScriptExecutor)driver; client_width.ExecuteScript("return document.documentElement.clientWidth");
            IJavaScriptExecutor page_height = (IJavaScriptExecutor)driver; page_height.ExecuteScript("return document.documentElement.scrollHeight");
            
            //string client_height_string = client_height.getType();
            //Console.WriteLine(client_height_string);

            int client_height_int = 300;
            int page_height_int = 3000;
            
            //  Try to intelligently scroll one screen at a time until bottom
            //
            //  couldn't get it to work in the universal code.   looks like   driver. will work for each browser
            try
            {            
               //client_height_int = (long)client_height;
               //page_height_int = (long)page_height;
            }
            catch (FormatException e)
            {
                Console.WriteLine(e.Message);
            }

            // errors out
            //int client_height = driver.execute_script("return document.documentElement.clientHeight");
            //int page_height = driver.execute_script("return document.documentElement.scrollHeight");
            
            
            //  Just scroll to the bottom the hard way 300 pixels at time
            //  Could update it such that one runs a single test case, reviews it for screen height and page height
            //   and changes variables accordingly.

            int current_height = 0;
            
            do {
                Console.WriteLine("Inside the Do. current_height: " + current_height);
                IJavaScriptExecutor je = (IJavaScriptExecutor)driver; je.ExecuteScript("window.scrollBy(0, " +client_height_int+");");
                System.Threading.Thread.Sleep(500);
                
                if (current_height == page_height_int)
                {
                    break;
                }
                current_height = current_height + client_height_int;
            }
            while (current_height < page_height_int);

            // Clean up -> scroll to the bottom, just in case & wait 1
            js.ExecuteScript("window.scrollTo(0, Math.max(document.documentElement.scrollHeight, document.body.scrollHeight, document.documentElement.clientHeight));");
            System.Threading.Thread.Sleep(1000);  

        }


       



        [TearDown]
        public void Close_Test()
        {
            driver.Quit();
        }
    }
}