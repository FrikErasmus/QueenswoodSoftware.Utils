using QueenswoodSoftware.Utils.Models;

namespace QueenswoodSoftware.Utils.Classes;

public interface IApiCall
{
    Task<object?> ApiPost(string url, string controller, string method, object content, string token);

    Task<object?> Post(string url, string controller, string method, object content, object user);

    Task<object?> Post(string url, string api, string method, object content, User loginUser);
}

public class ApiCall : IApiCall
{
    /// <summary>
    /// API call using Bearer token authentication.
    /// </summary>
    public Task<object?> ApiPost(string url, string controller, string method, object content, string token)
    {
        try
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri(url + controller + "/" + method);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var byteContent = new ByteArrayContent(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(content)));
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var apiCallTask = client.PostAsync(method, byteContent);
            apiCallTask.Wait();
            var response = apiCallTask.Result;

            if (response.StatusCode == HttpStatusCode.Unauthorized
                || response.StatusCode == HttpStatusCode.Forbidden
                || response.StatusCode == HttpStatusCode.BadRequest)
            {
                return Task.FromResult<object?>(new ResponseMessage
                {
                    Success = false,
                    ShortDescription = $"{response.ReasonPhrase}"
                });
            }

            return Task.FromResult<object?>(response.IsSuccessStatusCode
                ? JsonConvert.DeserializeObject<ResponseMessage>(response.Content.ReadAsStringAsync().Result)
                : new ResponseMessage { Success = false, ShortDescription = $"API Call Failed. {response.ReasonPhrase}" });
        }
        catch (Exception ex)
        {
            Log.Logger.Error(ex.Message, ex);
            return Task.FromResult<object?>(new ResponseMessage { Success = false, ShortDescription = ex.Message });
        }
    }

    /// <summary>
    /// API call using JWT token and user ID passed as a two-element list: [userId, token].
    /// </summary>
    public async Task<object?> Post(string url, string controller, string method, object content, object user)
    {
        try
        {
            var theUser = (List<string>)user;
            var cts = new CancellationTokenSource();
            cts.CancelAfter(20000);

            using var client = new HttpClient();
            client.BaseAddress = new Uri(url + controller + "/" + method);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.ExpectContinue = false;
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {theUser![1]}");
            client.DefaultRequestHeaders.Add("UserId", $"{theUser[0]}");

            var httpContent = new ByteArrayContent(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(content)));
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var apiCallTask = await client.PostAsync(method, httpContent, cts.Token);

            if (apiCallTask.StatusCode == HttpStatusCode.Unauthorized || apiCallTask.StatusCode == HttpStatusCode.Forbidden)
            {
                Log.Warning(controller + ": " + method, apiCallTask.ReasonPhrase);
            }

            if (!apiCallTask.IsSuccessStatusCode)
            {
                Log.Logger.Error(controller + ": " + method, apiCallTask.ReasonPhrase);
                return new ResponseMessage { Success = false, ShortDescription = $"API Call Failed. {apiCallTask.ReasonPhrase}" };
            }

            var result = JsonConvert.DeserializeObject<ResponseMessage>(apiCallTask.Content.ReadAsStringAsync(cts.Token).Result);

            if (!(result == null || result.Success)) Log.Warning(controller + ": " + method, result.ShortDescription);
            return result;
        }
        catch (Exception ex)
        {
            Log.Logger.Error(ex.Message, ex);
            return new ResponseMessage { Success = false, ShortDescription = ex.Message };
        }
    }

    /// <summary>
    /// API call using Basic authentication via a <see cref="User"/> login model.
    /// </summary>
    public async Task<object?> Post(string url, string api, string method, object content, User loginUser)
    {
        try
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri(api + url + "/" + method);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.ExpectContinue = false;
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var byteArray = Encoding.ASCII.GetBytes($"{loginUser.Username}:{loginUser.Password}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            var byteContent = new ByteArrayContent(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(content)));
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var apiCallTask = await client.PostAsync(method, byteContent);

            if (apiCallTask.StatusCode == HttpStatusCode.Unauthorized || apiCallTask.StatusCode == HttpStatusCode.Forbidden)
            {
                Log.Warning(method, apiCallTask.ReasonPhrase);
                return new ResponseMessage { Success = false, ShortDescription = $"{apiCallTask.ReasonPhrase}" };
            }

            if (!apiCallTask.IsSuccessStatusCode)
            {
                Log.Logger.Error(url + ": " + method, apiCallTask.ReasonPhrase);
                return new ResponseMessage { Success = false, ShortDescription = $"API Call Failed. {apiCallTask.ReasonPhrase}" };
            }

            var result = JsonConvert.DeserializeObject<ResponseMessage>(apiCallTask.Content.ReadAsStringAsync().Result);

            if (!result!.Success) Log.Warning(url + ": " + method, result.ShortDescription);
            return result;
        }
        catch (Exception ex)
        {
            Log.Logger.Error(ex.Message, ex);
            return new ResponseMessage { Success = false, ShortDescription = ex.Message };
        }
    }
}