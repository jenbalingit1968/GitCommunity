using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GitCommunity.Shared.Models
{
    public class User
    {
        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("company")]
        public string Company { get; set; }

        [JsonPropertyName("followers")]
        public int Followers { get; set; }

        public int Public_Repos { get; set; }

        public double NumberOfFollowerPerRepositories
        {
            get; set;
        }
        public string  Message { get; set; }

        public string Source { get; set; }
    }

    internal static class UserExtension
    {
        public static void GenerateNumberOfFollowerPerRepositories(this User user)
        {
            try
            {
                user.NumberOfFollowerPerRepositories = user.Followers / user.Public_Repos;
            }
            catch (Exception)
            {
                user.NumberOfFollowerPerRepositories = 0;
            }

            
        }
    }
}
