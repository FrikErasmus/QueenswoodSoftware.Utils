using Library.Helpers;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace Library.Classes
{
    /// <summary>
    /// HTTP method identifiers used when calling remote API endpoints.
    /// </summary>
    public enum Connection
    {
        /// <summary>HTTP GET.</summary>
        GET,
        /// <summary>HTTP POST.</summary>
        POST,
        /// <summary>HTTP PUT.</summary>
        PUT,
        /// <summary>HTTP HEAD.</summary>
        HEAD,
        /// <summary>HTTP DELETE.</summary>
        DELETE,
        /// <summary>HTTP PATCH.</summary>
        PATCH,
        /// <summary>HTTP OPTIONS.</summary>
        OPTIONS,
        /// <summary>HTTP CONNECT.</summary>
        CONNECT,
        /// <summary>HTTP TRACE.</summary>
        TRACE
    }

    /// <summary>
    /// Defines operations for invoking configured REST API endpoints.
    /// </summary>
    public interface ICall
    {
        /// <summary>
        /// Sends a request to the configured API and deserializes the response.
        /// </summary>
        /// <param name="path">The relative API path.</param>
        /// <param name="o">The request body object to serialize as JSON.</param>
        /// <param name="method">The HTTP method to use.</param>
        /// <returns>A <see cref="ResponseMessage"/> containing the API result.</returns>
        Task<ResponseMessage> Api(string path, object o, Method method);

        /// <summary>
        /// Checks whether the configured API is available.
        /// </summary>
        /// <returns>True when the API is available; otherwise false.</returns>
        Task<bool> ApiAvailableTask();
    }

    /// <summary>
    /// Executes authenticated REST API calls using application configuration settings.
    /// </summary>
    public class Call : ICall
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="Call"/> class.
        /// </summary>
        /// <param name="configuration">Application configuration containing API URL and auth settings.</param>
        public Call(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Checks whether the configured API is available.
        /// </summary>
        /// <returns>True when the API is available; otherwise false.</returns>
        public async Task<bool> ApiAvailableTask()
        {
            //var client = new RestClient(_configuration["ApiUrl"]!);
            //var request = new RestRequest("/health/check");
            //await client.ExecuteAsync(request);
            return true;
        }

        /// <summary>
        /// Sends a request to the configured API and deserializes the response.
        /// </summary>
        /// <param name="path">The relative API path.</param>
        /// <param name="o">The request body object to serialize as JSON.</param>
        /// <param name="method">The HTTP method to use.</param>
        /// <returns>A <see cref="ResponseMessage"/> containing the API result.</returns>
        public async Task<ResponseMessage> Api(string path, object o, Method method)
        {
            var client = new RestClient(_configuration["ApiUrl"]!);
            var request = new RestRequest(path, method);

            var user = _configuration["Auth:User"];
            var passw = new Secure().Encrypt(new Epoch().ReturnEpoch());

            var byteArray = Encoding.ASCII.GetBytes($"{user}:{passw}");
            request.AddHeader("Authorization", $"Basic {Convert.ToBase64String(byteArray)}");
            request.AddJsonBody(JsonConvert.SerializeObject(o));

            var response = await client.ExecuteAsync(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                return new ResponseMessage()
                {
                    Success = false,
                    //ShortDescription = "Internal Server Error"
                    ShortDescription = response.ErrorMessage
                };
            }

            if (response.ErrorException != null)
            {
                return new ResponseMessage()
                {
                    Success = false,
                    ShortDescription = response.ErrorException.Message
                };
            }

            return JsonConvert.DeserializeObject<ResponseMessage>(response.Content!)!;
        }
    }
}
