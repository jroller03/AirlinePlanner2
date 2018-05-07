using System;
using System.Collections.Generic;
using AirlinePlanner.Models;

namespace AirlinePlanner
{
  class City
  {
    private int _id;
    private string _name;
    private string _airline;
    private static List<City> cities = new List<City> {};

    public City(string name, string airline, int id=0)
    {
      _name = name;
      _airline = airline;
      _id = id;
    }

    public string GetName()
    {
      return _name;
    }

    public string GetAirline()
    {
      return _airline;
    }

    public int GetCityId()
    {
      return _id;
    }

    public static List<City> GetAllCities()
    {
      List<City> allCities = new List<City>{};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText= @"SELECT * FROM cities;";

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while (rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        string airline = rdr.GetString(2);
        City newCity = new City(name, airline, id);
        allCities.Add(newCity);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allCities;
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;

      cmd.CommandText = @"INSERT INTO cities (name, airline) VALUES (@thisName, @thisAirline);";

      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@thisName";
      name.Value = this._name;
      cmd.Parameters.Add(name);

      MySqlParameter airline = new MySqlParameter();
      airline.ParameterName = "@thisName";
      airline.Value = this._airline;
      cmd.Parameters.Add(airline);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }

    }
  }
}
