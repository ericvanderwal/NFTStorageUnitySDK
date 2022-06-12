using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace NFTstorage
{
    public static class Handler
    {
        /// <summary>
        /// Handle response from get requests to NFT storage api
        /// </summary>
        /// <param name="root"></param>
        /// <param name="valueType"></param>
        /// <returns></returns>
        public static DataResponse ObjectDataResponseHandler(JObject root, ValueType valueType)
        {
            DataResponse dataResponse = new DataResponse();

            if (root["ok"] != null)
            {
                bool isOk = root["ok"].ToObject<bool>();

                // how to handle value response in the case of an "Ok" response
                if (isOk)
                {
                    List<Value> responseValues = new List<Value>();

                    switch (valueType)
                    {
                        // do you expect more than one value from the response?
                        case ValueType.Multi:
                            responseValues = root["value"]?.ToObject<List<Value>>();
                            break;
                        case ValueType.Single:
                        {
                            Value singleValue = root["value"]?.ToObject<Value>();
                            responseValues.Add(singleValue);
                            break;
                        }
                        case ValueType.None:
                            responseValues = null;
                            break;
                    }


                    dataResponse.Success = true;
                    dataResponse.Values = responseValues;
                }
                else
                {
                    if (root["error"] != null)
                    {
                        Error error = root["error"].ToObject<Error>();
                        dataResponse.Error = error;
                    }
                }
            }
            else
            {
                dataResponse.Success = false;
            }

            return dataResponse;
        }
        
        /// <summary>
        /// Create a Unity Web request for NFT.Storage. This function automatically configures the web request and authorization
        /// base on settings in your keys and constants files.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static UnityWebRequest CreateRequest(string path, RequestType type = RequestType.GET, object data = null)
        {
            var request = new UnityWebRequest(path, type.ToString());
            request.SetRequestHeader("Authorization", "Bearer " + Keys.key);

            if (data != null)
            {
                var bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            }

            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            return request;
        }

        public enum RequestType
        {
            GET = 0,
            POST = 1,
            PUT = 2,
            DELETE = 4
        }

        public enum ValueType
        {
            Single,
            Multi,
            None,
        }
    }
}