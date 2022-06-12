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

    private void TakePicture()
    {
        button.interactable = false;
        SetText("Taking photo");
        StartCoroutine(NFTstorage.Helper.TakeScreenShot(CallBackOnCamera));
    }

    private void CallBackOnCamera(byte[] bytes)
    {
        SetText("Uploading photo");
        StartCoroutine(NFTstorage.NetworkManager.UploadObject(CallBackOnUpload, bytes));
    }

    private void CallBackOnUpload(NFTstorage.DataResponse obj)
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

    private void SetText(string text)
    {
        inputField.text = text;
    }
}