using System;
using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

namespace CENSIS.Runtime
{
   public class WeatherManager : MonoBehaviour
{
    public string apiKey;
    public string currentWeatherApi;

    [SerializeField]
    TMP_Text[] weather;
    private LocationInfo lastLocation;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Weather : Get weather");

        // Config api
        string config = Resources.Load<TextAsset>("OpenWeatherMap").text;
        string[] openWeatherMap = config.Split(",");
        apiKey = openWeatherMap[0].Trim();
        currentWeatherApi = openWeatherMap[1].Trim();
        Debug.Log("weather : " + apiKey);
        Debug.Log("weather : " + currentWeatherApi);

        Debug.Log("Weather : lastlocation set to " + lastLocation);
        StartCoroutine(FetchLocationData());
    }

    private IEnumerator FetchLocationData()
    {
        Debug.Log("Weather : Getting Location Data");

        if (!Input.location.isEnabledByUser)
            yield break;

        Input.location.Start();

        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait < 1)
        {
            Debug.Log("Weather : Location Timed out");
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Weather : Unable to determine device location");
            yield break;
        }
        else
        {
            lastLocation = Input.location.lastData;
            Debug.Log("Weather : lastlocation set to " + lastLocation);
            UpdateWeatherData();
        }

        Input.location.Stop();
    }

    private IEnumerator FetchWeather(string lat, string lon)
    {
        string url = string.Format(
            $"{currentWeatherApi}lat={lat}&lon={lon}&appid={apiKey}&units=metric"
        );
        Debug.Log("Weather : URL is " + url);
        UnityWebRequest fetchWeatherRequest = UnityWebRequest.Get(url);
        yield return fetchWeatherRequest.SendWebRequest();

        if (
            fetchWeatherRequest.result == UnityWebRequest.Result.ConnectionError
            || fetchWeatherRequest.result == UnityWebRequest.Result.ProtocolError
        )
        {
            Debug.Log("Weather: Error getting weather");
            Debug.Log(fetchWeatherRequest.result);
        }
        else
        {
            Debug.Log(fetchWeatherRequest.downloadHandler.text);
            var response = JSON.Parse(fetchWeatherRequest.downloadHandler.text);
            Debug.Log("Weather: " + response["main"]["temp"]);

            var temp = Format(response["main"]["temp"]);
            var humidity = response["main"]["humidity"];
            var max = Format(response["main"]["temp_max"]);
            var min = Format(response["main"]["temp_min"]);
            Debug.Log("Weather:" + temp + humidity + max + min);

            weather[0].text = $"{temp}°";
            weather[1].text = $"Humidity: {humidity}%\nMax: {max}°\nMin: {min}°";
        }
    }

    private double Format(string str)
    {
        Debug.Log("Weather: " + str);
        Debug.Log("Weather: " + float.Parse(str));
        Debug.Log("Weather: " + Math.Round(float.Parse(str)));
        return Math.Round(float.Parse(str), 0);
    }

    private void UpdateWeatherData()
    {
        Debug.Log("Weather : UpdateWeatherData");
        StartCoroutine(
            FetchWeather(lastLocation.latitude.ToString(), lastLocation.longitude.ToString())
        );
    }
} 
}

