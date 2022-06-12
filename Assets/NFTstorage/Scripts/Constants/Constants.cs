using UnityEditor;

namespace NFTstorage
{
    /// <summary>
    /// Constant paths for NFT.Storage and IPFS gateways
    /// </summary>
    public static class Constants
    {
        public const string Server = "https://api.nft.storage";
        public const string IP = "http://127.0.0.1:8787";
        public const string NoRedirect = "#x-ipfs-companion-no-redirect";

        /// <summary>
        /// List of IPFS gateway paths to use as a subdomain
        /// </summary>
        public static readonly string[] GatewaysSubdomain = new[]
        {
            ".ipfs.nftstorage.link",
        };

        /// <summary>
        /// List of IPFS gateway paths to use
        /// </summary>
        public static readonly string[] GatewaysPath = new[]
        {
            "https://gateway.pinata.cloud/ipfs/",
            "https://ipfs.io/ipfs/",
            "https://ipfs.fleek.co/ipfs/",
            "https://cloudflare-ipfs.com/",
            "https://gateway.ipfs.io/ipfs/",
            "https://tth-ipfs.com/ipfs/",
        };
    }
}