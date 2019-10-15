using System.Linq;
using UnityEngine;

namespace YleSearch
{
    public class SearchListItemData
    {
        private const string CountryCodeFinland = "fi";
        private const string DefaultFieldValue = "N/A";

        public string Title { get; }
        public string Info { get; }
        public bool IsExpanded { get; set; }

        public SearchListItemData(YleProgramData data)
        {
            if (!(data is YleProgramData listItemData))
            {
                Debug.LogError("Missing data");
                return;
            }

            string title = null;

            if (listItemData.title != null &&
                listItemData.title.Any() && 
                !listItemData.title.TryGetValue(CountryCodeFinland, out title))
            {
                title = listItemData.title.Values.FirstOrDefault() ?? DefaultFieldValue;
            }

            Title = title;
            Info =
                $@"ID: {listItemData.id}
Duration: {(!string.IsNullOrEmpty(listItemData.duration) ? listItemData.duration : DefaultFieldValue)}
Type: {(!string.IsNullOrEmpty(listItemData.type) ? listItemData.type : DefaultFieldValue)}
Type Media: {(!string.IsNullOrEmpty(listItemData.typeMedia) ? listItemData.typeMedia : DefaultFieldValue)}
Type Creative: {(!string.IsNullOrEmpty(listItemData.typeCreative) ? listItemData.typeCreative : DefaultFieldValue)}";
            
            IsExpanded = false;
        }
    }
}