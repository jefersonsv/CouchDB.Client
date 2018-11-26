using CouchDB.Client;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Threading.Tasks;

public class RestClientWrapper
{
    internal RestClient client;
    public RestClientWrapper(RestClient client)
    {
        this.client = client;
    }

    /// <summary>
    /// http://stackoverflow.com/questions/14977848/how-to-make-sure-that-string-is-valid-json-using-json-net
    /// </summary>
    /// <param name="strInput"></param>
    /// <returns></returns>
    private static bool IsValidJson(string strInput)
    {
        try
        {
            JObject.Parse(strInput);
            return true;
        }
        catch { }

        return false;
    }

    /// <summary>
    /// http://stackoverflow.com/questions/12232653/example-of-restsharp-async-client-executeasynct-works
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public Task<CouchResponse> ExecuteAsync(IRestRequest request)
    {
        var tcs = new TaskCompletionSource<CouchResponse>();

        client.ExecuteAsync(request, response =>
        {
            tcs.SetResult(new CouchResponse(response));
        });

        return tcs.Task;
    }

    /// <summary>
    /// http://stackoverflow.com/questions/12232653/example-of-restsharp-async-client-executeasynct-works
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public Task<IRestResponse> RestExecuteAsync(IRestRequest request)
    {
        var tcs = new TaskCompletionSource<IRestResponse>();

        client.ExecuteAsync(request, response =>
        {
            tcs.SetResult(response);
        });

        return tcs.Task;
    }
}

