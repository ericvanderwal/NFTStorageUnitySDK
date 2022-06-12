using System;
using System.Collections;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

namespace NFTstorage
{
    public static class Helper
    {
        /// <summary>
        /// Take a screenshot from main camera and return as byte array
        /// </summary>
        /// <param name="callbackOnFinish"></param>
        /// <returns></returns>
        public static IEnumerator TakeScreenShot(Action<byte[]> callbackOnFinish)
        {
            // required to prevent this unity error "ReadPixels was called to read pixels from system frame buffer, while not inside drawing frame."
            yield return new WaitForEndOfFrame();
            int width = Screen.width;
            int height = Screen.height;
            Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
            tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            tex.Apply();

            byte[] bytes = tex.EncodeToPNG();

            // Destroy(tex);

            callbackOnFinish(bytes);
        }

        /// <summary>
        /// Generate a path (url) to a IFPS gateway by passing the gateway, type and CID.
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="gateway"></param>
        /// <param name="isSubdomain"></param>
        /// <returns></returns>
        public static string GenerateGatewayPath(string cid, string gateway, bool isSubdomain)
        {
            string path = String.Empty;
            if (isSubdomain)
            {
                path = cid + gateway;
            }
            else
            {
                path = gateway + path;
            }

            return path;
        }

        /// <summary>
        /// Return byte array from absolute path. Do no include any slashes in the path or 'asset' in the directory
        /// Example directory = "myImages", pathFile = "image.png"
        /// Returns null if no path found
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="pathFile"></param>
        /// <returns></returns>
        public static byte[] AbsoluteFileToBytes(string directory, string pathFile)
        {
            string dPath = Application.dataPath + "/" + directory;
            string fPath = dPath + "/" + pathFile;

            if (!Directory.Exists(dPath))
            {
                return null;
            }

            if (!File.Exists(fPath))
            {
                return null;
            }

            byte[] data = File.ReadAllBytes(fPath);
            return data;
        }

        /// <summary>
        /// Convert ERC721 MetaData type to bytes for upload
        /// </summary>
        /// <param name="metaData"></param>
        /// <returns></returns>
        public static byte[] ERC721MetaDataToBytes(NFTstorage.ERC721.NftMetaData metaData)
        {
            if (metaData == null) return null;
            string jsonMetaData =
                JsonConvert.SerializeObject(metaData, Formatting.Indented, new JsonSerializerSettings
                {
                    DefaultValueHandling = DefaultValueHandling.Ignore
                });

            byte[] data = Encoding.ASCII.GetBytes(jsonMetaData);
            return data;
        }
    }
}