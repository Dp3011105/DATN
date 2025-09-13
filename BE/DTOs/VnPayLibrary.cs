using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace BE.DTOs
{
    public class VnPayLibrary
    {
        private readonly SortedList<string, string> _requestData = new SortedList<string, string>(new VnPayCompare());
        private readonly SortedList<string, string> _responseData = new SortedList<string, string>(new VnPayCompare());

        public void AddRequestData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
                _requestData.Add(key, value);
        }

        public void AddResponseData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
                _responseData.Add(key, value);
        }

        public string GetResponseData(string key)
        {
            return _responseData.TryGetValue(key, out var retValue) ? retValue : string.Empty;
        }

        public string CreateRequestUrl(string baseUrl, string vnpHashSecret)
        {
            var data = new StringBuilder();

            foreach (var (key, value) in _requestData)
            {
                data.Append(WebUtility.UrlEncode(key) + "=" + WebUtility.UrlEncode(value) + "&");
            }

            var querystring = data.ToString().TrimEnd('&');
            var vnpSecureHash = HmacSha512(vnpHashSecret, querystring);

            return $"{baseUrl}?{querystring}&vnp_SecureHash={vnpSecureHash}";
        }

        public bool ValidateSignature(string inputHash, string secretKey)
        {
            var rspRaw = GetResponseDataRaw();
            var myChecksum = HmacSha512(secretKey, rspRaw);
            return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
        }

        private string GetResponseDataRaw()
        {
            var data = new StringBuilder();

            if (_responseData.ContainsKey("vnp_SecureHashType"))
                _responseData.Remove("vnp_SecureHashType");
            if (_responseData.ContainsKey("vnp_SecureHash"))
                _responseData.Remove("vnp_SecureHash");

            foreach (var (key, value) in _responseData)
            {
                data.Append(WebUtility.UrlEncode(key) + "=" + WebUtility.UrlEncode(value) + "&");
            }

            return data.ToString().TrimEnd('&');
        }

        private string HmacSha512(string key, string inputData)
        {
            var hash = new StringBuilder();
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var inputBytes = Encoding.UTF8.GetBytes(inputData);

            using (var hmac = new HMACSHA512(keyBytes))
            {
                var hashValue = hmac.ComputeHash(inputBytes);
                foreach (var theByte in hashValue)
                {
                    hash.Append(theByte.ToString("x2"));
                }
            }

            return hash.ToString();
        }

        public string GetIpAddress(HttpContext context)
        {
            var remoteIpAddress = context.Connection.RemoteIpAddress;
            if (remoteIpAddress == null) return "127.0.0.1";

            return remoteIpAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6
                ? Dns.GetHostEntry(remoteIpAddress).AddressList.FirstOrDefault(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)?.ToString() ?? "127.0.0.1"
                : remoteIpAddress.ToString();
        }
    }

    public class VnPayCompare : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            return string.CompareOrdinal(x, y);
        }
    }
}
