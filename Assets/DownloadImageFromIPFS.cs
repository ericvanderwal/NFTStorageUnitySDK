using System.Collections;
using System.Collections.Generic;
using NFTstorage;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DownloadImageFromIPFS : MonoBehaviour
{
    [SerializeField]
    private Button button;

    [SerializeField]
    private Image image;

    [SerializeField]
    private string cid;

    private TextMeshProUGUI buttonText;


    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(DownloadImage);
    }

    private void DownloadImage()
    {
        buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = "Downloading ...";
        button.interactable = false;
        var path = Helper.GenerateGatewayPath(cid, Constants.GatewaysSubdomain[0], true);
        StartCoroutine(NetworkManager.DownloadImage(CallbackResponse, path));
    }

    private void CallbackResponse(DownloadResponse obj)
    {
        if (obj.IsSuccess)
        {
            var texture2d = obj.Texture2D;
            image.sprite = Sprite.Create(texture2d, new Rect(0.0f, 0.0f, texture2d.width, texture2d.height),
                new Vector2(0.5f, 0.5f), 100.0f);
            buttonText.text = "Completed";
        }

        else
        {
            buttonText.text = obj.ErrorMessage;
        }

        button.interactable = true;
    }
}