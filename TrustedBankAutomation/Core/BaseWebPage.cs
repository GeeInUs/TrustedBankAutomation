
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Opera;
using OpenQA.Selenium.Remote;
using System;
using System.Diagnostics;
using System.IO;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System.Runtime.InteropServices;
using System.Threading;
using System.Text;
using Microsoft.Win32;
using System.Drawing;
using System.Runtime.InteropServices.ComTypes;
using System.Drawing.Imaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TrustedBankAutomation.Core
{
    public sealed class ScreenCapture
    {
        public static string ScreenShotFileName { get; set; }

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetDesktopWindow();

        [StructLayout(LayoutKind.Sequential)]

        private struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);

        private static Image CaptureDesktop()
        {
            return CaptureWindow(GetDesktopWindow());
        }
        private static Bitmap CaptureActiveWindow()
        {
            return CaptureWindow(GetForegroundWindow());
        }



        /// <summary>
        /// Takes a screenshot of the active window
        /// </summary>
        /// <param name="screeshotDir"></param>
        /// <returns></returns>
        public static Tuple<bool, string> TakeActiveWindow(string screeshotDir)
        {
            ScreenShotFileName = screeshotDir + @"\" + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss_ffffff") + ".jpg";
            try
            {

                Bitmap image = ScreenCapture.CaptureActiveWindow();

                image.Save(ScreenShotFileName);

            }
            catch (Exception) { }
            return new Tuple<bool, string>(File.Exists(ScreenShotFileName), ScreenShotFileName);

        }



        /// <summary>
        /// Takes a screenshot of the desktop 
        /// </summary>
        /// <param name="screeshotDir"></param>
        /// <returns></returns>
        public static Tuple<bool, string> TakeDesktop(string screeshotDir)
        {
            ScreenShotFileName = screeshotDir + @"\" + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss_ffffff") + ".jpg";
            try
            {

                Image image = ScreenCapture.CaptureDesktop();

                image.Save(ScreenShotFileName);

            }
            catch (Exception) { }

            return new Tuple<bool, string>(File.Exists(ScreenShotFileName), ScreenShotFileName);

        }



        /// <summary>
        ///  Use webdreiver to take a screenshot
        /// </summary>
        /// <param name="screeshotDir"></param>
        /// <param name="WebDriver"></param>
        public static Tuple<bool, string> WebdriverScreenShot(string screeshotDir, IWebDriver WebDriver)
        {
            try
            {
                ScreenShotFileName = screeshotDir + @"\" + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss_ffffff") + ".jpg";

                var screenShot = ((ITakesScreenshot)WebDriver).GetScreenshot();

                screenShot.SaveAsFile(ScreenShotFileName);

                if (!File.Exists(ScreenShotFileName))
                {
                    using Bitmap bitmap = new Bitmap(GetWindowsScreenWidth(WebDriver), GetWindowsScreenHeight(WebDriver));
                    lock (bitmap)
                    {
                        using (Graphics g = Graphics.FromImage(bitmap))
                        {
                            g.CopyFromScreen(Point.Empty, Point.Empty, bitmap.Size);
                        }
                        bitmap.Save(ScreenShotFileName);
                    }
                }
            }
            catch (Exception) { }
            return new Tuple<bool, string>(File.Exists(ScreenShotFileName), ScreenShotFileName);

        }


        /// <summary>
        /// Gets the window height
        /// <param name="WebDriver"></param>
        /// </summary>
        /// <returns></returns>
        private static int GetWindowsScreenHeight(IWebDriver WebDriver)
        {
            return WebDriver.Manage().Window.Size.Height;
        }

        /// <summary>
        /// Gets the window width
        /// <param name="WebDriver"></param>
        /// </summary>
        /// <returns></returns>
        private static int GetWindowsScreenWidth(IWebDriver WebDriver)
        {
            return WebDriver.Manage().Window.Size.Width;
        }


        private static Bitmap CaptureWindow(IntPtr handle)
        {
            var rect = new Rect();
            GetWindowRect(handle, ref rect);
            var bounds = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
            var result = new Bitmap(bounds.Width, bounds.Height);

            using (var graphics = Graphics.FromImage(result))
            {
                graphics.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
            }

            return result;
        }

    }
    public class BaseWebPage
    {
        protected const int MAX_WAIT_SELENIUM_SERVER_TIME = 10;


        public IWebDriver BaseWebDriver { get; set; }

        public static Actions KeyBoardActions { get; set; }

        protected string BaseUrl { get; set; }

        private string SeleniumServerPath { get; set; }

        private string SeleniumServerPathPort { get; set; }

        private static Process SeleniumServer { get; set; }

        private static EdgeDriverService EdgeService { get; set; }

        private static InternetExplorerDriverService IEService { get; set; }

        private static ChromeDriverService ChromeService { get; set; }

        private static FirefoxDriverService FirefoxService { get; set; }

        private static OperaDriverService OperaService { get; set; }



        public struct BrowserName
        {

            public const string IE = "IE";
            public const string FireFox = "FireFox";
            public const string Edge = "Edge";
            public const string Chrome = "Chrome";
            public const string Opera = "Opera";
        }


        public struct Locator
        {


            public const string XPATH = "XPATH";
            public const string CSS = "CSS";
            public const string ID = "ID";
            public const string NAME = "NANE";
            public const string CLASSNAME = "CLASSNAME";
            public const string LINKTEXT = "LINKTEXT";
            public const string TAGNAME = "TAGNAME";
        }

        /// <summary>
        /// Find an elememt that contains a text 
        /// </summary>
        /// <param name="elementContainsText"></param>
        /// <returns>The element found</returns>
        public IWebElement FindElementByText(string elementContainsText)
        {

            var elements = BaseWebDriver.FindElements(By.XPath("//*[contains(text(), \"" + elementContainsText + "\") ]"));
            return elements.Count == 1 ? elements[0] : null;

        }



        /// <summary>
        /// Find an elememt by NAME attribute
        /// </summary>
        /// <param name="elementByName"></param>
        /// <returns>The element found</returns>
        public IWebElement FindElementByName(string elementByName)
        {
            var elements = BaseWebDriver.FindElements(By.Name(elementByName));
            return elements.Count == 1 ? elements[0] : null;

        }

        /// <summary>
        /// Find an elememt by ID attribute
        /// </summary>
        /// <param name="elementByID"></param>
        /// <returns>The element found</returns>
        public IWebElement FindElementByID(string elementByID)
        {
            var elements = BaseWebDriver.FindElements(By.Id(elementByID));
            return elements.Count == 1 ? elements[0] : null;

        }

        /// <summary>
        /// Find an elememt by XPATH attribute
        /// </summary>
        /// <param name="elementByXPath"></param>
        /// <returns>The element found</returns>
        public IWebElement FindElementByXPath(string elementByXPath)
        {
            var elements = BaseWebDriver.FindElements(By.XPath(elementByXPath));
            return elements.Count == 1 ? elements[0] : null;

        }


        /// <summary>
        /// Find an elememt by CSS attribute
        /// </summary>
        /// <param name="elementByCSS"></param>
        /// <returns>The element found</returns>
        public IWebElement FindElementByCSS(string elementByCSS)
        {
            var elements = BaseWebDriver.FindElements(By.CssSelector(elementByCSS));
            return elements.Count == 1 ? elements[0] : null;

        }


        /// <summary>
        /// Find an elememt by LINText attribute
        /// </summary>
        /// <param name="elementByLinkText"></param>
        /// <returns>The element found</returns>
        public IWebElement FindElementByLinkText(string elementByLinkText)
        {
            var elements = BaseWebDriver.FindElements(By.LinkText(elementByLinkText));
            return elements.Count == 1 ? elements[0] : null;

        }

        /// <summary>
        /// Find an elememt by CLASSNAME attribute
        /// </summary>
        /// <param name="elementByClassName"></param>
        /// <returns>The element found</returns>
        public IWebElement FindElementByClassName(string elementByClassName)
        {
            var elements = BaseWebDriver.FindElements(By.ClassName(elementByClassName));
            return elements.Count == 1 ? elements[0] : null;

        }


        /// <summary>
        /// The very basic to load the base class for automation
        /// </summary>
        /// <param name="url">Base url</param>
        /// <param name="browserType">The browser name</param>
        /// <param name="serverPort"> The server port </param>
        public BaseWebPage(string url, string browserType, string serverPort)
        {
            BaseUrl = url;
            SeleniumServerPathPort = serverPort;
            IntializeDriver(browserType);
            NavigateHome();

        }


        /// <summary>
        /// Default constructor with no args
        /// </summary>
        public BaseWebPage()
        { }


        /// <summary>
        ///  Waits for page to be in a ready state
        /// </summary>
        /// <returns></returns>
        private bool IsPageInReadyState()
        {
            int i = 0;
            bool StateReady = false;
            while (i < 10 && StateReady == false)
            {
                Thread.Sleep(1000);

                IJavaScriptExecutor js = (IJavaScriptExecutor)BaseWebDriver;
                string CurrState = js.ExecuteScript("return document.readyState;").ToString();

                if (CurrState.Equals("interactive") || CurrState.Equals("loading"))
                {
                    Thread.Sleep(1000);
                }

                else if (CurrState.Equals("complete"))
                {
                    StateReady = true;
                    break;
                }

                i++;
            }
            return StateReady;
        }

        /// <summary>
        /// waits for the current registration  page to load 
        /// </summary>
        /// <param name="waitTInMilliseconds">The time to wait for page to load in milliseconds</param>
        /// <param name="pageLoadedText">The time to wait for page to load in milliseconds</param>
        /// <returns>True if the page is loaded, false otherwise.</returns>
        public Boolean WaitForPageToLoad(int waitTInMilliseconds, string pageLoadedText)
        {
            Boolean isLoaded = false;
            try
            {
                if (pageLoadedText.Trim().Length > 0)
                    if (IsPageInReadyState())
                    {
                        isLoaded = new WebDriverWait(BaseWebDriver, TimeSpan.FromMilliseconds(waitTInMilliseconds)).Until<bool>((d) =>
                        {
                            IWebElement element = null;
                            try
                            {
                                element = d.FindElement(By.XPath("//*[contains(text(), \"" + pageLoadedText + "\") ]"));
                            }
                            catch (Exception) { }
                            return element != null ? (element.Displayed || element.Text != null) : false;
                        });
                    }
            }
            catch (Exception) { }

            return isLoaded;
        }


        /// <summary>
        /// Scolls down to the entire page
        /// </summary>
        public void ScrollDownEntirePage()
        {
            WaitForElementToLoad(Locator.XPATH, "//p[@load]", 1500);
            IJavaScriptExecutor js = (IJavaScriptExecutor)BaseWebDriver;
            js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");

        }


        /// <summary>
        /// Get page title
        /// </summary>
        public string GetPageTitle()
        {
            return BaseWebDriver.Title;

        }


        /// <summary>
        /// Scolls up to the entire page
        /// </summary>
        public void ScrollUpPage()
        {
            WaitForElementToLoad(Locator.XPATH, "//p[@load]", 1500);
            IJavaScriptExecutor js = (IJavaScriptExecutor)BaseWebDriver;
            js.ExecuteScript("window.scrollTo(0, 0)");

        }

        /// <summary>
        ///  Using a different way to query elements when webdriver cannot find hidden or stale elements
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ReadOnlyCollection<IWebElement> QueryCSSSelectorAll(string query)
        {
            query = "\"" + query + "\"";

            IJavaScriptExecutor js = (IJavaScriptExecutor)BaseWebDriver;
            return (ReadOnlyCollection<IWebElement>)js.ExecuteScript("return document.querySelectorAll(" + query + ");");
        }

        /// <summary>
        /// Maximizes a browser window in javascript 
        /// </summary>
        public void MaximizeBrowser()
        {
            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)BaseWebDriver;
                js.ExecuteScript("window.resizeTo(screen.width, screen.height);");
            }
            catch (Exception) { }
        }


        /// <summary>
        /// Checks to see if an alert is present
        /// </summary>
        /// <returns></returns>
        public bool IsAlertPresent()
        {
            try
            {
                BaseWebDriver.SwitchTo().Alert();
                return true;
            }
            catch (NoAlertPresentException)
            {
                return false;
            }
        }

        /// <summary>
        /// dismiss or reject  default alert
        /// This simple alert displays some information or warning on the screen.
        /// </summary>
        /// <returns></returns>
        public bool DismissAlert()
        {
            bool CloseAlertOk = false;
            try
            {
                string CurrHandle = GetCurrentWindowHandle();

                if (IsAlertPresent())
                {
                    IAlert alert = BaseWebDriver.SwitchTo().Alert();

                    alert.Dismiss();

                    BaseWebDriver.SwitchTo().Window(CurrHandle);

                    CloseAlertOk = true;
                }

            }
            catch (Exception)
            {
                CloseAlertOk = false;
            }
            return CloseAlertOk;
        }



        /// <summary>
        /// Closes driver instance and every other window associated with it
        /// </summary>
        public void CloseDriver()
        {
            BaseWebDriver.Quit();
        }

        /// <summary>
        /// Accept default alert
        /// This simple alert displays some information or warning on the screen.
        /// </summary>
        /// <returns>True if alert clicked ok, false otherwise</returns>
        public bool AcceptAlert()
        {
            bool AcceptAlertOk = false;
            try
            {
                string CurrHandle = GetCurrentWindowHandle();

                if (IsAlertPresent())
                {

                    IAlert alert = BaseWebDriver.SwitchTo().Alert();

                    alert.Accept();

                    BaseWebDriver.SwitchTo().Window(CurrHandle);

                    AcceptAlertOk = true;
                }
            }
            catch (Exception)
            {
                AcceptAlertOk = false;
            }
            return AcceptAlertOk;
        }



        /// <summary>
        /// Gets the current selenium window handle
        /// </summary>
        /// <returns>Returns the current selenium window handle </returns>
        public string GetCurrentWindowHandle()
        {
            ReadOnlyCollection<string> Handles = BaseWebDriver.WindowHandles;
            return Handles[Handles.Count - 1];

        }

        /// <summary>
        /// close default alert based on option provided
        /// </summary>
        /// <param name="text">The name of the text of the alert</param>
        /// <returns>True if alert clicked ok, false otherwise</returns>
        public bool ClickPopupButtonWithText(string text)
        {
            bool FoundTextToClick = false;
            try
            {

                var PopupButtn = BaseWebDriver.FindElements(By.XPath("//*[contains(text(), \"" + text + "\") ]"));

                if (PopupButtn.Count == 1)
                {
                    PopupButtn[0].Click();

                    FoundTextToClick = true;
                }


            }
            catch (Exception)
            {
                FoundTextToClick = false;
            }
            return FoundTextToClick;
        }


        /// Navigates to the base driver url 
        /// </summary>
        /// <returns></returns>
        public void NavigateHome()
        {
            BaseWebDriver.Navigate().GoToUrl(BaseUrl);
        }



        /// <summary>
        ///  Gets the path to selenium server jar file
        /// </summary>
        /// <returns></returns>
        private void SetStandAlonePath()
        {
            string[] FoundFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.jar", SearchOption.AllDirectories);
            foreach (string loopFile in FoundFiles)
            {
                if (loopFile.ToLower().IndexOf("selenium-server-standalone") > 0)
                {
                    SeleniumServerPath = loopFile;
                    break;
                }
            }
        }


        /// <summary>
        /// Takes screenshot of browser
        /// </summary>
        /// <param name="CurrTestContext"></param>
        /// <param name="ScreenShotDir"></param>
        public void TakeScreenShot( TestContext CurrTestContext, string ScreenShotDir)
        {
            Tuple<bool, string> SecondTake = null;
            Tuple<bool, string> ThirdTake = null;
            Tuple<bool, string> Result = null;
            try
            {

                if (!Directory.Exists(ScreenShotDir))
                    Directory.CreateDirectory(ScreenShotDir);


                Result = ScreenCapture.WebdriverScreenShot(ScreenShotDir, BaseWebDriver);

                if (Result.Item1 == false)
                    Result = ScreenCapture.TakeActiveWindow(ScreenShotDir);

                else if (SecondTake != null && SecondTake.Item1 == false)
                    Result = ScreenCapture.TakeActiveWindow(ScreenShotDir);

                else if (ThirdTake != null && ThirdTake.Item1 == false)
                    Result = ScreenCapture.TakeActiveWindow(ScreenShotDir);



            }
            catch (Exception) { }

            if (Result.Item1 == true)
                CurrTestContext.AddResultFile(Result.Item2);
        }




        /// <summary>
        /// Starts an instance of selenium server
        /// </summary>
        public void StartSeleniumServer()
        {
            try
            {
                SetStandAlonePath();
                SeleniumServer = new Process
                {
                    StartInfo =
                        {
                            CreateNoWindow = false,
                            WindowStyle = ProcessWindowStyle.Normal,
                            FileName = "cmd.exe",
                            WorkingDirectory = Directory.GetCurrentDirectory(),
                            Arguments = "/k  java -jar " +  SeleniumServerPath + " -port 4444",
                            UseShellExecute = true

                        }
                };

                SeleniumServer.EnableRaisingEvents = true;
                SeleniumServer.Start();
                Thread.Sleep(4000);

            }
            catch (Exception) { }
        }



        /// <summary>
        /// Stops selenium server
        /// </summary>
        public static void StopSeleniumServer()
        {
            if (SeleniumServer != null)
            {
                try
                {

                    SeleniumServer.Kill();

                    SeleniumServer = null;
                }
                catch (Exception) { }
                finally { SeleniumServer = null; }
            }
        }

        /// Navigates to a Page
        /// </summary>
        /// <returns></returns>
        public void NavigateTo(String url)
        {
            BaseWebDriver.Navigate().GoToUrl(url);
        }


        /// <summary>
        /// waits for a text to appear on a particular page
        /// </summary>
        /// <param name="PageLoadedText">The text value that servers as a visual cue for waiting on </param>
        /// <param name="waitTInMilliseconds">The time to wait for page to load in milliseconds</param>
        /// <returns>True if the page is loaded, false otherwise.</returns>
        public Boolean WaitForTextToAppear(string PageLoadedText, int waitTInMilliseconds)
        {
            Boolean isLoaded = false;
            try
            {

                if (IsPageInReadyState())
                {
                    isLoaded = new WebDriverWait(BaseWebDriver, TimeSpan.FromMilliseconds(waitTInMilliseconds)).Until<bool>((d) =>
                    {
                        IWebElement banner = null;
                        try
                        {
                            banner = d.FindElement(By.XPath("//*[contains(text(), \"" + PageLoadedText + "\") ]"));
                        }
                        catch (Exception) { }
                        return banner != null ? banner.Displayed : false;
                    });
                }
            }
            catch (Exception) { }

            return isLoaded;
        }

        /// <summary>
        ///  Searches and wait for an element to load in milliseconds
        /// </summary>
        /// <param name="elementBy"></param>
        /// <param name="queryString"></param>
        /// <param name="waitTInMilliseconds"></param>
        /// <returns></returns>
        public Boolean WaitForElementToLoad(string elementBy, string queryString, int waitTInMilliseconds)
        {
            Boolean isLoaded = false;
            try
            {

                if (IsPageInReadyState())
                {

                    isLoaded = new WebDriverWait(BaseWebDriver, TimeSpan.FromMilliseconds(waitTInMilliseconds)).Until<bool>((d) =>
                    {
                        ReadOnlyCollection<IWebElement> element = null;
                        try
                        {
                            switch (elementBy)
                            {
                                case Locator.XPATH:
                                    element = d.FindElements(By.XPath(queryString));
                                    break;

                                case Locator.CSS:
                                    element = d.FindElements(By.CssSelector(queryString));
                                    break;

                                case Locator.CLASSNAME:
                                    element = d.FindElements(By.ClassName(queryString));
                                    break;

                                case Locator.LINKTEXT:
                                    element = d.FindElements(By.LinkText(queryString));
                                    break;

                                case Locator.ID:
                                    element = d.FindElements(By.Id(queryString));
                                    break;

                                case Locator.NAME:
                                    element = d.FindElements(By.Name(queryString));
                                    break;

                                case Locator.TAGNAME:
                                    element = d.FindElements(By.TagName(queryString));
                                    break;

                            }
                        }
                        catch (Exception) { }
                        return element.Count > 0 ? element[0].Displayed : false;
                    });
                }
            }
            catch (Exception) { }

            return isLoaded;
        }

        ///Maximizes page
        /// </summary>
        /// <returns></returns>
        public void MaximizePage()
        {
            try
            {
                if (IsPageInReadyState())
                    BaseWebDriver.Manage().Window.FullScreen();

            }
            catch (Exception) { MaximizeBrowser(); }

        }

        /// <summary>
        /// Stops the driver services
        /// </summary>
        public void StopDriverServices()
        {
            if (EdgeService != null && EdgeService.IsRunning)
                EdgeService.Dispose();

            if (IEService != null && IEService.IsRunning)
                IEService.Dispose();

            if (ChromeService != null && ChromeService.IsRunning)
                ChromeService.Dispose();

            if (FirefoxService != null && FirefoxService.IsRunning)
                FirefoxService.Dispose();

            if (OperaService != null && OperaService.IsRunning)
                OperaService.Dispose();


        }
        ///Miniizes page
        /// </summary>
        /// <returns></returns>
        public void Minimize()
        { BaseWebDriver.Manage().Window.Minimize(); }

        /// Navigates back in history
        /// </summary>
        /// <returns></returns>
        public void NavigateBack()
        { BaseWebDriver.Navigate().Back(); }

        /// <summary>
        /// Gets the capabilities for FIREFOX
        /// </summary>
        /// <param name="_logLevel"></param>
        /// <param name="_alertBehaviour"></param>
        /// <param name="acceptSlefSignedSSL"></param>
        /// <param name="_loadStrategy"></param>
        /// <returns></returns>
        private FirefoxOptions GetFirefoxBrowserOptions(FirefoxDriverLogLevel _logLevel = FirefoxDriverLogLevel.Default,
                                                        UnhandledPromptBehavior _alertBehaviour = UnhandledPromptBehavior.Default,
                                                        bool acceptSlefSignedSSL = true,
                                                        PageLoadStrategy _loadStrategy = PageLoadStrategy.Default)
        {

            FirefoxService = FirefoxDriverService.CreateDefaultService(Directory.GetCurrentDirectory(), @"geckodriver.exe");
            FirefoxService.HideCommandPromptWindow = true;
            FirefoxService.Start();
            return new FirefoxOptions()
            {
                LogLevel = _logLevel,
                UnhandledPromptBehavior = _alertBehaviour,
                PageLoadStrategy = _loadStrategy,
                AcceptInsecureCertificates = acceptSlefSignedSSL,
                UseLegacyImplementation = true


            };
        }


        /// <summary>
        /// Gets the capabilities for chrome
        /// </summary>
        /// <param name="_alertBehaviour"></param>
        /// <param name="acceptSlefSignedSSL"></param>
        /// <param name="_leaveBrowserRunning"></param>
        /// <param name="_loadStrategy"></param>
        /// <returns></returns>
        private ChromeOptions GetChromeBrowserOptions(UnhandledPromptBehavior _alertBehaviour = UnhandledPromptBehavior.Default,
                                                       bool acceptSlefSignedSSL = true,
                                                       bool _leaveBrowserRunning = false,
                                                       PageLoadStrategy _loadStrategy = PageLoadStrategy.Default)
        {


            ChromeService = ChromeDriverService.CreateDefaultService(Directory.GetCurrentDirectory(), @"chromedriver.exe");
            ChromeService.HideCommandPromptWindow = true;
            ChromeService.EnableVerboseLogging = true;
            ChromeService.Start();

            return new ChromeOptions()
            {
                LeaveBrowserRunning = _leaveBrowserRunning,
                UnhandledPromptBehavior = _alertBehaviour,
                PageLoadStrategy = _loadStrategy,
                AcceptInsecureCertificates = acceptSlefSignedSSL

            };

        }






        /// <summary>
        /// Gets the capabilities for Opera
        /// </summary>
        /// <param name="_alertBehaviour"></param>
        /// <param name="acceptSlefSignedSSL"></param>
        /// <param name="_loadStrategy"></param>
        /// <returns></returns>
        private OperaOptions GetOperaBrowserOptions(UnhandledPromptBehavior _alertBehaviour = UnhandledPromptBehavior.Default,
                                                       bool acceptSlefSignedSSL = true,
                                                       bool _leaveBrowserRunning = false,
                                                       PageLoadStrategy _loadStrategy = PageLoadStrategy.Default)
        {

            OperaService = OperaDriverService.CreateDefaultService(Directory.GetCurrentDirectory(), @"operadriver.exe");
            OperaService.EnableVerboseLogging = true;
            OperaService.HideCommandPromptWindow = true;
            OperaService.Start();

            return new OperaOptions()
            {

                UnhandledPromptBehavior = _alertBehaviour,
                LeaveBrowserRunning = _leaveBrowserRunning,
                PageLoadStrategy = _loadStrategy,
                AcceptInsecureCertificates = acceptSlefSignedSSL

            };
        }



        private string GetEdgeExecutableDriver()
        {
            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion");
            string VersionInfo = (string)registryKey.GetValue("productName");

            return VersionInfo.Contains("Windows Server 2012 R2") ? @"msedgedriver.exe" : @"MicrosoftWebDriver.exe";
        }
        /// <summary>
        /// Gets the capabilities for Edge
        /// </summary>
        /// <param name="_alertBehaviour"></param>
        /// <param name="acceptSlefSignedSSL"></param>
        /// <param name="_useInPrivateBrowsing"></param>
        /// <param name="_loadStrategy"></param>
        /// <returns></returns>
        private EdgeOptions GetEdgeBrowserOptions(UnhandledPromptBehavior _alertBehaviour = UnhandledPromptBehavior.Default,

                                                       bool acceptSlefSignedSSL = false,
                                                       bool _useInPrivateBrowsing = false,
                                                       PageLoadStrategy _loadStrategy = PageLoadStrategy.Normal)
        {



            return new EdgeOptions()
            {
                UnhandledPromptBehavior = _alertBehaviour,
                UseInPrivateBrowsing = _useInPrivateBrowsing,
                PageLoadStrategy = _loadStrategy,
                StartPage = BaseUrl,
                AcceptInsecureCertificates = acceptSlefSignedSSL,

            };

        }





        /// <summary>
        /// Gets the capabilities for IE
        /// </summary>
        /// <param name="acceptSlefSignedSSL"></param>
        /// <param name="_ignoreZoomLevel"></param>
        /// <param name="_enableNativeEvents"></param>
        /// <param name="_ensureCleanSession"></param>
        /// <param name="browserTimeoutMilloseconds"></param>
        /// <param name="_scrollBehaviour"></param>
        /// <param name="_promtBehaviour"></param>
        /// <param name="_loadStrategy"></param>
        /// <returns></returns>
        private InternetExplorerOptions GetIEBrowserOptions(
                                                        bool acceptSlefSignedSSL = false,
                                                        bool _ignoreZoomLevel = true,
                                                        bool _enableNativeEvents = false,
                                                        bool _ensureCleanSession = false,
                                                        int browserTimeoutMilloseconds = 300000,
                                                        InternetExplorerElementScrollBehavior _scrollBehaviour = InternetExplorerElementScrollBehavior.Default,
                                                        UnhandledPromptBehavior _promtBehaviour = UnhandledPromptBehavior.Default,
                                                        PageLoadStrategy _loadStrategy = PageLoadStrategy.Eager)
        {

            IEService = InternetExplorerDriverService.CreateDefaultService(Directory.GetCurrentDirectory(), @"IEDriverServer.exe");
            IEService.LoggingLevel = InternetExplorerDriverLogLevel.Trace;
            IEService.HideCommandPromptWindow = false;
            IEService.Start();

            return new InternetExplorerOptions()
            {
                ElementScrollBehavior = _scrollBehaviour,
                EnableNativeEvents = _enableNativeEvents,
                IgnoreZoomLevel = _ignoreZoomLevel,
                ForceCreateProcessApi = false,
                ForceShellWindowsApi = true,
                EnablePersistentHover = true,
                FileUploadDialogTimeout = new TimeSpan(0, 0, 0, browserTimeoutMilloseconds),
                RequireWindowFocus = true,
                UnhandledPromptBehavior = _promtBehaviour,
                BrowserAttachTimeout = new TimeSpan(0, 0, 0, browserTimeoutMilloseconds),
                EnsureCleanSession = _ensureCleanSession,
                PageLoadStrategy = _loadStrategy,
                AcceptInsecureCertificates = acceptSlefSignedSSL

            };
        }
        /// <summary>
        /// Kills left over drivers  and services
        /// </summary>
        public void KillDrivers()
        {

            StopDriverServices();

            KillResidueProcess(Process.GetProcessesByName("geckodriver"));
            KillResidueProcess(Process.GetProcessesByName("chromedriver"));
            KillResidueProcess(Process.GetProcessesByName("IEDriverServer"));
            KillResidueProcess(Process.GetProcessesByName("IEDriverServer64"));
            KillResidueProcess(Process.GetProcessesByName("MicrosoftWebDriver"));
            KillResidueProcess(Process.GetProcessesByName("msedgedriver"));



        }


        /// <summary>
        /// Kills references to existing command console except selenium
        /// </summary>
        public void ReleaseUsedReferences()
        {
            KillDrivers();

            var CmdProcess = Process.GetProcessesByName("cmd");
            foreach (Process p in CmdProcess)
            {
                if (p.MainWindowTitle.IndexOf("-jar") <= 0 && p.MainWindowTitle.IndexOf("java") <= 0)
                    p.Kill();
            }
        }




        /// <summary>
        /// checks to see if selenium is up and running
        /// </summary>
        public bool IsSeleniumRunning()
        {
            bool IsRunning = false;

            var CmdProcess = Process.GetProcessesByName("cmd");
            foreach (Process p in CmdProcess)
            {
                if (p.MainWindowTitle.IndexOf("-jar") > 0 && p.MainWindowTitle.IndexOf("java") > 0)
                {
                    IsRunning = true;
                    break;
                }
            }

            return IsRunning;
        }

        /// <summary>
        /// e
        /// </summary>
        public void DisposeDriver()
        {
            DisposeResources();
        }


        /// <summary>
        /// Kill selenium driver and residue processes
        /// </summary>
        private void DisposeResources()
        {
            try
            {
                StopSeleniumServer();

                if (BaseWebDriver != null)
                {
                    BaseWebDriver.Quit();

                    BaseWebDriver = null;

                }
                KillDrivers();


            }
            catch (Exception) { }
            finally { SeleniumServer = null; }

        }



        /// <summary>
        /// Kills hanging processes from Selenium driver
        /// </summary>
        /// <param name="residue"></param>
        private static void KillResidueProcess(Process[] residue)
        {
            foreach (Process p in residue)
            {
                p.Kill();
            }

        }

        /// Navigates forward in history
        /// </summary>
        /// <returns></returns>
        protected void NavigateForward()
        {
            BaseWebDriver.Navigate().Forward();
        }


        /// <summary>
        /// Returns a running instance of Edge driver 
        /// </summary>
        /// <returns></returns>
        private EdgeDriverService GetEdgeService()
        {
            EdgeService = EdgeDriverService.CreateDefaultService(Directory.GetCurrentDirectory(), GetEdgeExecutableDriver());
            EdgeService.UseVerboseLogging = true;
            EdgeService.HideCommandPromptWindow = false;
            EdgeService.Start();
            return EdgeService;
        }

        /// <summary>
        ///  Create an instance of the driver browser
        /// </summary>
        /// <param name="browserType"></param>
        private void IntializeDriver(string browserType)
        {
            ReleaseUsedReferences();

            string DefaultUri = "http://localhost:" + SeleniumServerPathPort + "/wd/hub";

            if (!IsSeleniumRunning())
                StartSeleniumServer();

            BaseWebDriver = browserType switch
            {

                BrowserName.IE => new RemoteWebDriver(new Uri(DefaultUri), GetIEBrowserOptions()),
                BrowserName.FireFox => new RemoteWebDriver(new Uri(DefaultUri), GetFirefoxBrowserOptions()),
                BrowserName.Edge => new EdgeDriver(GetEdgeService(), GetEdgeBrowserOptions()),
                BrowserName.Chrome => new RemoteWebDriver(new Uri(DefaultUri), GetChromeBrowserOptions()),
                BrowserName.Opera => new RemoteWebDriver(new Uri(DefaultUri), GetOperaBrowserOptions()),
                _ => new RemoteWebDriver(new Uri(DefaultUri), GetChromeBrowserOptions()),
            };

            KeyBoardActions = new Actions(BaseWebDriver);

        }
    }
}
