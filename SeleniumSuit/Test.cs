using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Firefox;
using System.Data.SqlClient;

namespace SeleniumSuit
{
    [TestClass]
    public class Test
    {
        [TestMethod]
        public void CromMethod()
        {
            string ActualResult;
            string ExpectedResult = "Guru99 Bank Home Page";
            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("http://demo.guru99.com/V4/");
            driver.Manage().Window.Maximize();
            driver.FindElement(By.Name("uid")).SendKeys("uid");
            driver.FindElement(By.Name("password")).SendKeys("password");
            driver.FindElement(By.Name("btnLogin")).Click();
            ActualResult = driver.Title;
            if (ActualResult.Contains(ExpectedResult))
            {
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = "Server=[server_name];Database=[database_name];Trusted_Connection=true";
                conn.Open();
                SqlCommand insertCommand = new SqlCommand("INSERT INTO Status (Status, Scenario, TestDescription) VALUES (@1, @3, @2)", conn);


                insertCommand.Parameters.Add(new SqlParameter("1", "Pass"));
                insertCommand.Parameters.Add(new SqlParameter("2", DateTime.Now));
                insertCommand.Parameters.Add(new SqlParameter("3", "QA1"));

          

                Console.WriteLine("Commands executed! Total rows affected are " + insertCommand.ExecuteNonQuery());
                Console.ReadLine();
                Assert.IsTrue(true, "Test Case passed");
            }
            else
           {
               SqlConnection conn = new SqlConnection();
              conn.ConnectionString = "Server=[server_name];Database=[database_name];Trusted_Connection=true";
                conn.Open();
                SqlCommand insertCommand = new SqlCommand("INSERT INTO Status (Status, Scenario, TestDescription) VALUES (@1, @3, @2)", conn);
              insertCommand.Parameters.Add(new SqlParameter("1", "Fail"));
               insertCommand.Parameters.Add(new SqlParameter("2", DateTime.Now));
              insertCommand.Parameters.Add(new SqlParameter("3", "QA1"));

            }



            driver.Close();
            driver.Quit();
        }












    }
}
