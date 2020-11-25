﻿using NUnit.Framework;
using nunit_datadriven.commons;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nunit_datadriven.pageobjects
{
    public class LoginPage
    {
        public IWebDriver driver = InitBrowser.Getbrowser();
        public CommonCode common;
        public Read_ini read;
        public By userNameTextBox = By.Id(Elements.EmailTextBox);
        public By PasswordTextBox = By.Id(Elements.PasswordTextBox);
        public By Submitbutton = By.Id(Elements.SubmitButton);
        public By loginError = By.XPath(Elements.loginError);

        public LoginPage()
        {
            read = new Read_ini();
            common = new CommonCode();
        }

        public void AssertLogin()
        {
            Assert.AreEqual(read.GetProperty("loginPage", "loginTitle"), driver.Title);
            Console.WriteLine("Tile Matches");
        }

        public void loginToApplication(String userName, String password)
        {
            driver.FindElement(userNameTextBox).Clear();
            driver.FindElement(userNameTextBox).SendKeys(userName);
            driver.FindElement(PasswordTextBox).Clear();
            driver.FindElement(PasswordTextBox).SendKeys(password);
        }

        public void tapSubmit()
        {
            driver.FindElement(Submitbutton).Click();
        }

        //here one way of implementing by retruning into String below instead of void
        //the reason we have to get error (actual)and use this inside stepdefination-loginso returned it.
        public string validateLoginError()
        {
            return driver.FindElement(loginError).Text;
        }
    }
}