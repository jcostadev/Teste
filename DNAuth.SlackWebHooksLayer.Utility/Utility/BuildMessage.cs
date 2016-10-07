using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace DNAuth.SlackWebHooksLayer.Utility.Utility {

    /// <summary>
    /// Returns an HttpResponseMessage, Web API converts the return value directly into an HTTP response message
    /// </summary>
    public static class BuildMessage {

        public static HttpResponseMessage BuildHttpMessage(string baseAddress, string requestUri) {
            HttpResponseMessage responseMessage;

            //The using statement obtains the resource specified, executes the statements and finally calls the Dispose method of the object to clean up the object.
            using (HttpClient client = new HttpClient()) {
                
                //Sets the base adress of URI of the Internet Resource used when sending requests
                client.BaseAddress = new Uri(baseAddress);

                //Gets and removes the headers 
                client.DefaultRequestHeaders.Accept.Clear();

                //Adds application/json as the header
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sends a get request to the specified URI as an asynchronous operation
                responseMessage = client.GetAsync(requestUri).Result;
            }

            return responseMessage;
        }
    }
}

