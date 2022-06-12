using System;
using System.Collections.Generic;

// Classes to manage data response from NFT Storage after requests are made
namespace NFTstorage
{
    [Serializable]
    public class DataResponse
    {
        public List<Value> Values;
        public bool Success;
        public Error Error;

        // set a default error message by default
        public DataResponse()
        {
            Error = new Error
            {
                message = "An unknown error has occured",
                name = "Unknown Error"
            };

            Values = new List<Value>();
        }
    }

    [Serializable]
    public class Deal
    {
        public string status;
        public DateTime lastChanged;
        public int chainDealID;
        public string datamodelSelector;
        public string statusText;
        public DateTime dealActivation;
        public DateTime dealExpiration;
        public string miner;
        public string pieceCid;
        public string batchRootCid;
    }

    [Serializable]
    public class Pin
    {
        public string cid;
        public DateTime created;
        public int size;
        public string status;
    }

    [Serializable]
    public class Value
    {
        public string cid;
        public DateTime created;
        public string type;
        public string scope;
        public List<object> files;
        public int size;
        public string name;
        public Pin pin;
        public List<Deal> deals;
    }

    public class Error
    {
        public string name;
        public string message;
    }
}