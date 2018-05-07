using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using AirlinePlanner.Models;
using AirlinePlanner;
using MySql.Data.MySqlClient;


namespace AirlinePlanner.Tests
{

   [TestClass]
   public class ItemTests : IDisposable
   {
       public ItemTests()
       {
           DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=airline_planner;";
       }
       [TestMethod]
       public void Save_SavesItemToDatabase_CityList()
       {
         City testCity = new City("Seattle", "Delta");
         testCity.Save();

         List<City> testResult = City.GetAllCities();
         List<City> allCities = new List<City> {testCity};

         Collection.Assert.AreEqual(testResult, allCities);

       }
    }
}
