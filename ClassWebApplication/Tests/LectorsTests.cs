using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace ClassWebApplication.Tests
{
    [TestFixture]
    public class LectorsTests
    {
        private IWebDriver driver;
        private string baseUrl;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            baseUrl = "https://localhost:44350"; 
            driver.Navigate().GoToUrl(baseUrl);
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }

        [Test]
        public void TestIndexPage()
        {
            driver.Navigate().GoToUrl(baseUrl + "/Lectors/");
            Assert.IsTrue(driver.PageSource.Contains("Лектори"));
            Assert.IsTrue(driver.PageSource.Contains("ПІБ"));
            Assert.IsTrue(driver.PageSource.Contains("Електронна адреса"));
        }

        [Test]
        public void TestCreateLector()
        {
            
            driver.Navigate().GoToUrl(baseUrl + "/Lectors/Create");
            driver.FindElement(By.Id("FullName")).SendKeys("Нове ПІБ лектора");
            driver.FindElement(By.Id("Email")).SendKeys("example@gmail.com");
            driver.FindElement(By.Id("University")).SendKeys("Новий університет");
            driver.FindElement(By.CssSelector(".btn.btn-primary")).Click();
            // Assert
            Assert.IsTrue(driver.PageSource.Contains("Нове ПІБ лектора"));
            Assert.IsTrue(driver.PageSource.Contains("example@gmail.com"));
            Assert.IsTrue(driver.PageSource.Contains("Новий університет"));
        }

        [Test]
        public void TestDetails()
        {
            driver.Navigate().GoToUrl(baseUrl + "/Lectors/Details/1");
            Assert.IsTrue(driver.PageSource.Contains("ПІБ"));
            Assert.IsTrue(driver.PageSource.Contains("Електронна адреса"));
        }
        [Test]
        public void EditLector()
        {
            // Act
            driver.Navigate().GoToUrl(baseUrl + "/Lectors/Edit/15");
            driver.FindElement(By.Id("FullName")).Clear();
            driver.FindElement(By.Id("FullName")).SendKeys("Updated FullName");
            driver.FindElement(By.Id("Email")).Clear();
            driver.FindElement(By.Id("Email")).SendKeys("example@gmail.com");
            driver.FindElement(By.Id("University")).Clear();
            driver.FindElement(By.Id("University")).SendKeys("Updated university");
            driver.FindElement(By.CssSelector(".btn.btn-primary")).Click();

            // Assert
            Assert.IsTrue(driver.PageSource.Contains("Updated FullName"));
            Assert.IsTrue(driver.PageSource.Contains("Updated university"));
        }

        [Test]
        public void TestDeleteLector()
        {
            driver.Navigate().GoToUrl(baseUrl + "/Lectors/Delete/15");
            driver.FindElement(By.CssSelector(".btn.btn-danger")).Click();
            Assert.IsFalse(driver.PageSource.Contains("Updated FullName"));
        }
    }
}
