using NUnit.Framework;
using nunit_datadriven.pageobjects;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace specflowdemo.stepsdefinitions
{
    [Binding]
    public class HomeSteps
    {
        private string price = null;

        // public LoginPage login = new LoginPage();
        //if method is not static then declare object like below
        //method is static then call direct through classname.
        public HomePage home = new HomePage();

        public LoginPage login = new LoginPage();

        [Given(@"I am on the Homepage")]
        public void GivenIAmOnTheHomepage()
        {
            home.validateHome();
        }

        [When(@"Product ""(.*)"" is searched")]
        public void WhenProductIsSearched(string product)
        {
            home.searchprodonly(product);
        }

        [Then(@"Correct price '(.*)' is diplayed")]
        public void ThenCorrectPriceIsDiplayed(string priceRequired)
        {
            Assert.AreEqual(priceRequired, price);
        }

        [Then(@"Correct Productname ""(.*)"" is displayed")]
        public void ThenCorrectProductnameIsDisplayed(string productname)
        {
            //here we are returning the price hence stored in price variable
            price = home.searchProduct(productname);
        }
    }
}