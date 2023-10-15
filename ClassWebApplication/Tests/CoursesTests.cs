using NUnit;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace ClassWebApplication.Tests
{
    [TestFixture]
    public class CoursesTests
    {
        private IWebDriver driver;
        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }

        [Test]
        public void IndexPageDisplaysCourses()
        {
            // Act
            driver.Navigate().GoToUrl("https://localhost:44350/Courses");

            // Assert
            Assert.IsTrue(driver.PageSource.Contains("Назва"));
            Assert.IsTrue(driver.PageSource.Contains("Опис"));
            Assert.IsTrue(driver.PageSource.Contains("Матеріали"));
        }

        [Test]
        public void CreateNewCourse()
        {
            // Arrange
            var driver = new ChromeDriver(); 

            // Act
            driver.Navigate().GoToUrl("https://localhost:44350/Courses/Create");
            driver.FindElement(By.Id("Title")).SendKeys("Нова назва курсу");
            driver.FindElement(By.Id("Description")).SendKeys("Опис нового курсу");
            driver.FindElement(By.Id("MaterialLink")).SendKeys("https://drive.google.com/file/d/1pxOSXucSiSDfle_HTsP4NYtbR_t5PWj9/view");
            var lectorDropdown = driver.FindElement(By.Id("LectorCourses"));
            var select = new SelectElement(lectorDropdown);
            select.SelectByText("Іванов Іван Іванович");
            driver.FindElement(By.CssSelector(".btn.btn-primary")).Click();

            // Assert
            Assert.IsTrue(driver.PageSource.Contains("Нова назва курсу"));
            Assert.IsTrue(driver.PageSource.Contains("Опис нового курсу"));
        }

        [Test]
        public void CourseDetails()
        {
            // Act
            driver.Navigate().GoToUrl("https://localhost:44350/Courses/Details/1"); 

            // Assert
            Assert.IsTrue(driver.PageSource.Contains("Деталі"));
            Assert.IsTrue(driver.PageSource.Contains("Назва"));
            Assert.IsTrue(driver.PageSource.Contains("Опис"));
            Assert.IsTrue(driver.PageSource.Contains("Матеріали"));
        }

        [Test]
        public void EditCourse()
        {
            // Act
            driver.Navigate().GoToUrl("https://localhost:44350/Courses/Edit/22"); 
            driver.FindElement(By.Id("Title")).Clear();
            driver.FindElement(By.Id("Title")).SendKeys("Updated Course Title");
            driver.FindElement(By.Id("Description")).Clear();
            driver.FindElement(By.Id("Description")).SendKeys("Updated Course Description");
            driver.FindElement(By.Id("MaterialLink")).Clear();
            driver.FindElement(By.Id("MaterialLink")).SendKeys("https://drive.google.com/file/d/1pxOSXucSiSDfle_HTsP4NYtbR_t5PWj9/view");
            var lectorDropdown = driver.FindElement(By.Id("LectorCourses"));
            var select = new SelectElement(lectorDropdown);
            select.SelectByText("Шевченко Тарас Григорович");
            driver.FindElement(By.CssSelector(".btn.btn-primary")).Click();

            // Assert
            Assert.IsTrue(driver.PageSource.Contains("Updated Course Title"));
            Assert.IsTrue(driver.PageSource.Contains("Updated Course Description"));
        }

        [Test]
        public void DeleteCourse()
        {
            // Act
            driver.Navigate().GoToUrl("https://localhost:44350/Courses/Delete/8");
            driver.FindElement(By.CssSelector(".btn.btn-danger")).Click();

            // Assert
            Assert.IsFalse(driver.PageSource.Contains("Updated Course Title"));
        }

    }
}
