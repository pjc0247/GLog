using UnityEngine;
using System.Text;
using System.Collections.Generic;

public class GLog {
    
    private static string BuildInfo()
    {
        return string.Format(
            @"
Date : {0}
Time : {1}
Platform : {2}
AppVersion : {3}
UnityVersion : {4}
            ",
            System.DateTime.Now.ToLongDateString(), System.DateTime.Now.ToLongTimeString(),
            Application.platform.ToString(),
            Application.version,
            Application.unityVersion
            );
    }

    public static string CreateURL(string name, string log)
    {
        var url = "https://api.github.com/gists";
        var body = new Dictionary<string, object>()
            {
                {"description", name },
                {"public", true },
                {"files", new Dictionary<string,object>() {
                    {"data", new Dictionary<string, object>()
                    {
                        {"content", BuildInfo() }
                    } },
                    {"log.txt", new Dictionary<string,object>() {
                        {"content", log }
                    } }
                } }
            };
        var json = MiniJSON.Json.Serialize(body);
        var www = new WWW(url, Encoding.UTF8.GetBytes(json));

        while (www.isDone == false) ;

        if (www.error != null)
        {
            Debug.LogError(www.error);
            return null;
        }

        var resp = (Dictionary<string, object>)
            MiniJSON.Json.Deserialize(www.text);

        return (string)resp["html_url"];
    }
}
