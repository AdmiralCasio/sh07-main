using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using SimpleJSON;

public class WeatherManager : MonoBehaviour
{
    public string apiKey = "530fc21dd3934afbeb230b6edba1b2bf";
    public string currentWeatherApi = "https://api.openweathermap.org/data/2.5/weather?";
    [SerializeField] TMP_Text[] weather;
    private LocationInfo lastLocation;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Weather : Get weather");
        StartCoroutine(FetchLocationData());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator FetchLocationData()
    {
        Debug.Log("Weather : Getting Location Data");

        if (!Input.location.isEnabledByUser)
            yield break;

        Input.location.Start();

        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait < 1) {
            Debug.Log("Weather : Location Timed out");
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed) {
            Debug.Log("Weather : Unable to determine device location");
            yield break;
        }
        else
        {
            lastLocation = Input.location.lastData;
            Debug.Log("Weather : lastlocation set to "+ lastLocation);
            UpdateWeatherData();
        }

        Input.location.Stop();
    }
    private IEnumerator FetchWeather(string lat, string lon)
    {
        string url = string.Format($"{currentWeatherApi}lat={lat}&lon={lon}&appid={apiKey}&units=metric");
        Debug.Log("Weather : URL is " +  url);
        UnityWebRequest fetchWeatherRequest = UnityWebRequest.Get( url );
        yield return fetchWeatherRequest.SendWebRequest();

        if (fetchWeatherRequest.result == UnityWebRequest.Result.ConnectionError|| fetchWeatherRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("Weather: Error getting weather");
            Debug.Log(fetchWeatherRequest.error);
        }
        else
        {
            Debug.Log(fetchWeatherRequest.downloadHandler.text);
            var response = JSON.Parse(fetchWeatherRequest.downloadHandler.text);
            Debug.Log("Weather: "+ response["main"]["temp"]);

            var temp = format(response["main"]["temp"]);
            var humidity = response["main"]["humidity"];
            var max = format(response["main"]["temp_max"]);
            var min = format(response["main"]["temp_min"]);
            Debug.Log("Weather:" + temp + humidity + max + min);

            weather[0].text = $"{temp}°";
            weather[1].text = $"Humidity: {humidity}%\nMax: {max}°\nMin: {min}°";
        }
    }

    private string format(string str)
    {
        return str.Substring(0, str.Length - 3);
    }

    private void UpdateWeatherData()
    {
        Debug.Log("Weather : UpdateWeatherData");
        StartCoroutine(FetchWeather(lastLocation.latitude.ToString(), lastLocation.longitude.ToString()));
    }
}
