using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using AirlinePlanner.Models;

namespace AirlinePlanner
{
  public class City
  {
    private int _id;
    private string _name;

    public City(string name, int id = 0)
    {
      _name = name;
      _id = id;
    }

    public string GetName()
    {
      return _name;
    }

    public int GetCityId()
    {
      return _id;
    }
    public override bool Equals(System.Object otherCity)
    {
      if (!(otherCity is City))
      {
        return false;
      }
      else
      {
         City newCity = (City) otherCity;
         bool idEquality = this.GetCityId() == newCity.GetCityId();
         bool nameEquality = this.GetName() == newCity.GetName();
         return (idEquality && nameEquality);
       }
    }
    public override int GetHashCode()
    {
         return this.GetName().GetHashCode();
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
        City newCity = new City(name, id);
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

      cmd.CommandText = @"INSERT INTO cities (name) VALUES (@thisName);";

      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@thisName";
      name.Value = this._name;
      cmd.Parameters.Add(name);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static City Find (int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;

      cmd.CommandText= @"SELECT * FROM cities WHERE id = (@searchId);";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int cityId = 0;
      string cityName = "";

      while(rdr.Read())
      {
        cityId = rdr.GetInt32(0);
        cityName = rdr.GetString(1);
      }
      City newCity = new City(cityName, cityId);

      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
      return newCity;
    }

    public void AddFlight(Flight newFlight)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO flights_cities (flight_id, city_id) VALUES (@FlightId, @CityId);";

      MySqlParameter flight_id = new MySqlParameter();
      flight_id.ParameterName = "@FlightId";
      flight_id.Value = newFlight.GetId();
      cmd.Parameters.Add(flight_id);

      MySqlParameter city_id = new MySqlParameter();
      city_id.ParameterName = "@CityId";
      city_id.Value = _id;
      cmd.Parameters.Add(city_id);

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public List<Flight> GetFlights()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT flights.* FROM cities
      JOIN flights_cities ON (city_id = flights_cities.city_id)
      JOIN flights ON (flights_cities.flight_id=flights.id)
      WHERE city_id = @CityId;";

      MySqlParameter cityIdParameter = new MySqlParameter();
      cityIdParameter.ParameterName = "@CityId";
      cityIdParameter.Value = _id;
      cmd.Parameters.Add(cityIdParameter);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      List<Flight> flights = new List<Flight> {};
      while(rdr.Read())
      {
        int flightId = rdr.GetInt32(0);
        string flightName = rdr.GetString(1);
        string flightDate = rdr.GetString(2);
        string departureCity = rdr.GetString(3);
        string arrivalCity = rdr.GetString(4);
        string flightStatus = rdr.GetString(5);
        Flight newFlight = new Flight(flightName, flightDate, departureCity, arrivalCity, flightStatus, flightId);
        flights.Add(newFlight);
      }
      rdr.Dispose();

      conn.Close();
      if (conn != null)
      {
         conn.Dispose();
      }
      return flights;
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM cities;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
    }

  }
}
