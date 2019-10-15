using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LitJson;
using UnityEngine;
using UnityEngine.Networking;

namespace YleSearch
{
    public static class YleAPI
    {
        private const string DomainUrl = @"https://external.api.yle.fi/v1";
        private const string APP_ID = "db8ec02a";
        private const string APP_KEY = "4cc8486c1c7d664b60513eebe631ccaf";
        
        public static IEnumerator GetPrograms(string searchTerm, int offset, int range, Action<bool, IList<YleProgramData>> onCompleted = null)
        {
            using (var webRequest = UnityWebRequest.Get($"{DomainUrl}/programs/items.json?q={searchTerm}&app_id={APP_ID}&app_key={APP_KEY}&offset={offset}&limit={range}"))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.isNetworkError)
                {
                    Debug.LogError($"WebRequest for programs failed: ({webRequest.responseCode}) {webRequest.error}");
                    onCompleted?.Invoke(false, null);
                    yield break;
                }
                
                onCompleted?.Invoke(true, DeserializeResponse(webRequest.downloadHandler.text));
            }
        }
        
        private static List<YleProgramData> DeserializeResponse(string jsonData)
        {
            var obj = JsonMapper.ToObject(jsonData);

            var programDataList = JsonMapper.ToObject<YleProgramData[]>(JsonMapper.ToJson(obj["data"]));

            return programDataList.ToList();
        }
    }
}