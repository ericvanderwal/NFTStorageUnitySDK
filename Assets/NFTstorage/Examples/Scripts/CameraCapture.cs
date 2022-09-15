using System.Collections.Generic;
using NFTstorage;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CameraCapture : MonoBehaviour
{
    [Header("Input")]
    [SerializeField]
    private Button button;

    [SerializeField]
    private TMP_InputField inputField;

    [SerializeField]
    private SetMetaData SetMetaData;

    [Header("Output")]
    public List<Value> value;

    void Start()
    {
        value = new List<Value>();
        button.onClick.AddListener(TakePicture);
    }

    /// <summary>
    /// Take a picture of the screen using the NFT Storage Helper function
    /// </summary>
    private void TakePicture()
    {
        button.interactable = false;
        SetText("Taking photo");
        StartCoroutine(NFTstorage.Helper.TakeScreenShot(UploadScreenShot));
    }

    /// <summary>
    /// Upload the screenshot to NFT storage
    /// </summary>
    /// <param name="bytes"></param>
    private void UploadScreenShot(byte[] bytes)
    {
        SetText("Uploading photo");
        StartCoroutine(NFTstorage.NetworkManager.UploadObject(UploadCompleted, bytes));
    }

    /// <summary>
    /// Upload has been completed to NFT storage
    /// </summary>
    /// <param name="obj"></param>
    private void UploadCompleted(NFTstorage.DataResponse obj)
    {
        if (!obj.Success)
        {
            SetText("Error " + obj.Error.message);
            return;
        }

        if (obj.Values != null && obj.Values.Count > 0)
        {
            value = obj.Values;
            if (value != null)
            {
                var path = Helper.GenerateGatewayPath(value[0].cid, Constants.GatewaysSubdomain[0], true);
                SetText(path);
            }
        }

        if (SetMetaData != null) SetMetaData.Send(value[0].cid);
        button.interactable = true;
    }

    /// <summary>
    /// Set the text of the input field on screen
    /// </summary>
    /// <param name="text"></param>
    private void SetText(string text)
    {
        inputField.text = text;
    }
}