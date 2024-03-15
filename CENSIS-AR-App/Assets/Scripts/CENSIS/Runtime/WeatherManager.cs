using System;
using System.Collections;
using System.Globalization;
using SimpleJSON;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

namespace CENSIS.Runtime
{
    public class WeatherManager : MonoBehaviour
    {
        #region API

        public string apiKey;
        public string currentWeatherApi;

        #endregion
        
        [SerializeField]
        TMP_Text[] weather;
        private LocationInfo lastLocation;

        void Start()
        {
            string config = Resources.Load<TextAsset>("OpenWeatherMap").text;
            var openWeatherMap = JSON.Parse(config);
            apiKey = openWeatherMap["key"];
            currentWeatherApi = openWeatherMap["currentAPI"];
            
            StartCoroutine(FetchLocationData());
        }

        private IEnumerator FetchLocationData()
        {
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
                yield break;
            }

            if (Input.location.status == LocationServiceStatus.Failed)
            {
                yield break;
            }
            else
            {
                lastLocation = Input.location.lastData;
                UpdateWeatherData();
            }

            Input.location.Stop();
        }

        private IEnumerator FetchWeather(string lat, string lon)
        {
            string url = string.Format(
                $"{currentWeatherApi}lat={lat}&lon={lon}&appid={apiKey}&units=metric"
            );
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
                var response = JSON.Parse(fetchWeatherRequest.downloadHandler.text);
                var temp = Format(response["main"]["temp"]);
                var humidity = response["main"]["humidity"];
                var max = Format(response["main"]["temp_max"]);
                var min = Format(response["main"]["temp_min"]);

                weather[0].text = $"{temp}°";
                weather[1].text = $"Humidity: {humidity}%\nMax: {max}°\nMin: {min}°";
            }
        }

        private double Format(string str)
        {
            return Math.Round(float.Parse(str), 0);
        }

        private void UpdateWeatherData()
        {
            StartCoroutine(
                FetchWeather(lastLocation.latitude.ToString(CultureInfo.CurrentCulture), 
                    lastLocation.longitude.ToString(CultureInfo.CurrentCulture))
            );
        }
    }
}
