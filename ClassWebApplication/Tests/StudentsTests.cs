using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace ClassWebApplication.Tests
{
    [TestFixture]
    public class StudentsTests
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
            driver.Navigate().GoToUrl(baseUrl + "/Students/");
            Assert.IsTrue(driver.PageSource.Contains("Студенти"));
            Assert.IsTrue(driver.PageSource.Contains("ПІБ"));
            Assert.IsTrue(driver.PageSource.Contains("Електронна адреса"));
        }

        [Test]
        public void TestDetailsPage()
        {
            driver.Navigate().GoToUrl(baseUrl + "/Students/Details/1");
            Assert.IsTrue(driver.PageSource.Contains("Деталі"));
            Assert.IsTrue(driver.PageSource.Contains("ПІБ"));
            Assert.IsTrue(driver.PageSource.Contains("Електронна адреса"));
        }

        [Test]
        public void TestCreateStudent()
        {
            driver.Navigate().GoToUrl(baseUrl + "/Students/Create");
            driver.FindElement(By.Id("FullName")).SendKeys("ПІБ нового студента");
            driver.FindElement(By.Id("Email")).SendKeys("example@gmail.com");
            var studentDropdown = driver.FindElement(By.Id("StudentCourses"));
            var select = new SelectElement(studentDropdown);
            select.SelectByText("Веб-технології");
            driver.FindElement(By.CssSelector(".btn.btn-primary")).Click();

            // Assert
            Assert.IsTrue(driver.PageSource.Contains("ПІБ нового студента"));
            Assert.IsTrue(driver.PageSource.Contains("example@gmail.com"));
        }

        [Test]
        public void TestEditStudent()
        {
            driver.Navigate().GoToUrl(baseUrl + "/Students/Edit/19");
            driver.FindElement(By.Id("FullName")).Clear();
            driver.FindElement(By.Id("FullName")).SendKeys("Updated Student FullName");
            driver.FindElement(By.Id("Email")).Clear();
            driver.FindElement(By.Id("Email")).SendKeys("example@gmail.com");
            var studentDropdown = driver.FindElement(By.Id("StudentCourses"));
            var select = new SelectElement(studentDropdown);
            select.SelectByText("Системне програмування");
            driver.FindElement(By.CssSelector(".btn.btn-primary")).Click();

            // Assert
            Assert.IsTrue(driver.PageSource.Contains("Updated Student FullName"));
            Assert.IsTrue(driver.PageSource.Contains("example@gmail.com"));
        }

        [Test]
        public void TestDeleteStudent()
        {
            driver.Navigate().GoToUrl(baseUrl + "/Students/Delete/19");
            driver.FindElement(By.CssSelector(".btn.btn-danger")).Click();

            // Assert
            Assert.IsFalse(driver.PageSource.Contains("Updated Student FullName"));
            driver.Quit();
        }
    }
}
