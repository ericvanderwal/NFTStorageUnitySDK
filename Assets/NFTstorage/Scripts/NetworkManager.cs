using System;
using System.Collections;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace NFTstorage
{
    public static class NetworkManager
    {
        /// <summary>
        /// Get all objects. Optionally set a max number of requests and before date. A callback is required to get the response.
        /// </summary>
        /// <param name="maxNumRequest"></param>
        /// <param name="beforeDate"></param>
        /// <param name="callbackOnFinish"></param>
        /// <returns></returns>
        public static IEnumerator GetAllObjects(Action<DataResponse> callbackOnFinish, int maxNumRequest = 100,
            DateTime beforeDate = default)
        {
            // create a web request with the proper path and credentials
            var getRequest = Handler.CreateRequest(Constants.Server);

            // send the web request and wait for its results
            yield return getRequest.SendWebRequest();

            // parse the results to an object
            JObject root = JObject.Parse(getRequest.downloadHandler.text);

            // convert the object into class(s). In this example we are expecting multi results
            var dataResponse = Handler.ObjectDataResponseHandler(root, Handler.ValueType.Multi);

            // do a call back to let the calling function know that this task was completed and pass back the results
            callbackOnFinish(dataResponse);
        }

        /// <summary>
        /// Get single object by CID. A callback is required to get the response
        /// </summary>
        /// <param name="callbackOnFinish"></param>
        /// <param name="cid"></param>
        /// <returns></returns>
        public static IEnumerator GetObjectByCid(Action<DataResponse> callbackOnFinish, string cid)
        {
            var getRequest = Handler.CreateRequest(Constants.Server + "/" + cid);
            yield return getRequest.SendWebRequest();
            JObject root = JObject.Parse(getRequest.downloadHandler.text);

            var dataResponse = Handler.ObjectDataResponseHandler(root, Handler.ValueType.Single);
            callbackOnFinish(dataResponse);
        }

        public static IEnumerator CheckObjectByCid(Action<DataResponse> callbackOnFinish, string cid)
        {
            var getRequest = Handler.CreateRequest(Constants.Server + "/check/" + cid);
            getRequest.SetRequestHeader("Authorization", "Bearer " + Keys.key);
            yield return getRequest.SendWebRequest();
            JObject root = JObject.Parse(getRequest.downloadHandler.text);

            var dataResponse = Handler.ObjectDataResponseHandler(root, Handler.ValueType.Single);
            callbackOnFinish(dataResponse);
        }


        /// <summary>
        /// Delete Object by CID
        /// </summary>
        /// <param name="callbackOnFinish"></param>
        /// <param name="cid"></param>
        /// <returns></returns>
        public static IEnumerator DeleteObjectByCid(Action<DataResponse> callbackOnFinish, string cid)
        {
            var getRequest = Handler.CreateRequest(Constants.Server + "/" + cid, Handler.RequestType.DELETE);
            yield return getRequest.SendWebRequest();
            JObject root = JObject.Parse(getRequest.downloadHandler.text);

            var dataResponse = Handler.ObjectDataResponseHandler(root, Handler.ValueType.None);
            callbackOnFinish(dataResponse);
        }

        /// <summary>
        /// Upload an object to by byte array
        /// </summary>
        /// <param name="callbackOnFinish"></param>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static IEnumerator UploadObject(Action<DataResponse> callbackOnFinish, byte[] bytes)
        {
            var request = Handler.CreateRequest(Constants.Server + "/upload", Handler.RequestType.POST);
            request.uploadHandler = new UploadHandlerRaw(bytes);
            yield return request.SendWebRequest();

            // await response to post
            JObject root = JObject.Parse(request.downloadHandler.text);
            var dataResponse = Handler.ObjectDataResponseHandler(root, Handler.ValueType.Single);
            callbackOnFinish(dataResponse);
        }

        /// <summary>
        /// Download an image by path and return a DownloadResponse class, including a Texture2D.
        /// </summary>
        /// <param name="callbackOnFinish"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IEnumerator DownloadImage(Action<DownloadResponse> callbackOnFinish, string path)
        {
            DownloadResponse downloadResponse = new DownloadResponse();
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(path);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + www.error);
                downloadResponse.IsSuccess = false;
                downloadResponse.ErrorMessage = www.error;
                callbackOnFinish(downloadResponse);
                yield break;
            }

            Texture2D loadedTexture = DownloadHandlerTexture.GetContent(www);
            downloadResponse.Texture2D = loadedTexture;
            downloadResponse.IsSuccess = true;
            callbackOnFinish(downloadResponse);
        }
    }
}