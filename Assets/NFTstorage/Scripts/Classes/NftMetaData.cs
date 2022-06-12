using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace NFTstorage.ERC721
{
    [Serializable]
    public class NftMetaData
    {
        // A human readable description of the item
        [DefaultValue("")]
        public string description;

        // This is the URL that will appear below the asset's image on OpenSea
        // and will allow users to leave OpenSea and view the item on your site.
        [DefaultValue("")]
        public string external_url;

        // This is the URL to the image of the item.
        // Can be just about any type of image
        // (including SVGs, which will be cached into PNGs by OpenSea),
        // and can be IPFS URLs or paths. We recommend using a 350 x 350 image.
        // eg "ipfs://bafybeihrumg5hfzqj6x47q63azflcpf6nkgcvhzzm6f6nhic2eg6uvozlq/test-356.jpg"

        [DefaultValue("")]
        public string image;

        // Name of the item.
        [DefaultValue("")]
        public string name;

        // These are the attributes for the item, which will
        // show up on the OpenSea page for the item
        [DefaultValue(null)]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<Attribute> attributes;

        //Background color of the item on OpenSea.
        //Must be a six-character hexadecimal without a pre-pended #.
        [DefaultValue("")]
        [JsonProperty("background_color")]
        public string BackgroundColor;

        //A URL to a multi-media attachment for the item.
        [DefaultValue("")]
        public string animation_url;

        //A URL to a YouTube video.
        [DefaultValue("")]
        public string youtube_url;


        public NftMetaData(string _name)
        {
            name = _name;
        }

        /// <summary>
        /// Set IPFS path by passing CID
        /// </summary>
        /// <param name="cid"></param>
        public void SetIPFS(string cid)
        {
            image = "ipfs://" + cid;
        }
    }

    // key - value pair for nft traits.
    // Value may be a number or a string with ""s
    // unix timestamp for dates

    // For numeric traits, OpenSea currently supports three different options,
    // number (lower right in the image below),
    // boost_percentage (lower left in the image above),
    // and boost_number (similar to boost_percentage
    // but doesn't show a percent sign).
    // If you pass in a value that's a number and you don't set a display_type,
    // the trait will appear in the Rankings section (top right in the image above).
    // Opensea also supports 'date' type. Such as display_type: date, type: birthday, unix time
    [Serializable]
    public class Attribute
    {
        // optional. field indicating how a numeric value should be displayed
        [DefaultValue("")]
        public string display_type;

        // optional. the name of the trait.
        [DefaultValue("")]
        public string trait_type;

        // the value of the trait. Not optional.
        public object value;
    }
}