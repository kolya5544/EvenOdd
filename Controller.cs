using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace EvenOdd
{
    class Controller : WebApiController
    {
        public static string JSON(object o)
        {
            return JsonConvert.SerializeObject(o);
        }

        public static JsonResponse NumberError = new JsonResponse() { error = true, message = "Incorrect input number!" };
        public static JsonResponse UsageError = new JsonResponse() { error = true, message = "Pass number using /?number=123 GET query field or use POST with 'number' x-www-form-urlencoded field." };
        public static JsonResponse LongError = new JsonResponse() { error = true, message = "Hey! Your GET query string is too long! Consider using POST with 'number' x-www-form-urlencoded field!" };

        public static async Task AsJSON(IHttpContext context, object? data)
        {
            if (data is null)
            {
                // Send an empty response
                return;
            }

            context.Response.ContentType = "application/json";
            using var text = context.OpenResponseText();
            // string.ToString returns the string itself
            await text.WriteAsync((string)data).ConfigureAwait(false);
        }

        public static string CutAndTrim(string inp)
        {
            if (inp.Length < 2048)
            {
                return inp;
            }

            return $"{inp.Substring(0, 64)}...{inp.Substring(inp.Length - 64, 64)}";
        }

        [Route(HttpVerbs.Get, "/")]
        public string IsEvenGET([QueryField] string? number)
        {
            if (string.IsNullOrEmpty(number))
            {
                return JSON(UsageError);
            }

            if (number.Length > 2048)
            {
                return JSON(LongError);
            }

            try
            {
                BigInteger bi = BigInteger.Parse(number);

                return JSON(new JsonResponse() { error = false, isEven = bi.IsEven, isOdd = !bi.IsEven, message = $"It looks like number {CutAndTrim(bi.ToString())} is {(bi.IsEven ? "even" : "odd")}!{CheckEasterEggs(bi)}"});
            } catch
            {
                return JSON(NumberError);
            }
        }

        private string CheckEasterEggs(BigInteger bi)
        {
            try
            {
                long a = (long)bi;
                switch (a)
                {
                    case 69:
                        return " Nice!";
                    case 420:
                        return " Yes!";
                    case 5544:
                        return " This service is hosted by IKTeam.";
                    case 1337:
                        return " What a hacker!";
                    case Int32.MaxValue:
                    case Int32.MaxValue - 1:
                    case (long)Int32.MaxValue + 1:
                    case byte.MaxValue:
                    case byte.MaxValue + 1:
                    case byte.MaxValue - 1:
                    case Int64.MaxValue:
                    case Int64.MaxValue - 1:
                    case Int16.MaxValue - 1:
                    case Int16.MaxValue:
                    case Int16.MaxValue + 1:
                    case UInt32.MaxValue:
                    case (long)UInt32.MaxValue + 1:
                    case UInt32.MaxValue - 1:
                    case UInt16.MaxValue:
                    case (long)UInt16.MaxValue + 1:
                    case UInt16.MaxValue - 1:
                        return " What did you expect to see here? A crash?";
                    case 0:
                        return " But is it something anyone is sure about?";
                }
            } catch
            {
            }
            return "";
        }

        [Route(HttpVerbs.Post, "/")]
        public string IsEvenPOST([FormField] string? number)
        {
            if (string.IsNullOrEmpty(number))
            {
                return JSON(UsageError);
            }

            try
            {
                BigInteger bi = BigInteger.Parse(number);

                return JSON(new JsonResponse() { error = false, isEven = bi.IsEven, isOdd = !bi.IsEven, message = $"It looks like number {CutAndTrim(bi.ToString())} is {(bi.IsEven ? "even" : "odd")}!{CheckEasterEggs(bi)}" });
            }
            catch
            {
                return JSON(NumberError);
            }
        }
    }

    public class JsonResponse
    {
        public bool error = false;
        public string message = null;
        public bool? isEven = null;
        public bool? isOdd = null;
    }
}
