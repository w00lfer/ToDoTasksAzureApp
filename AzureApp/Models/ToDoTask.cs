using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AzureApp.Models
{
    public class ToDoTask
    {
        //[JsonPropertyName("id")]
        public string id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("toDoTime")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ToDoTime { get; set; }

        [JsonPropertyName("completed")]
        public bool Completed { get; set; }

    }
}
