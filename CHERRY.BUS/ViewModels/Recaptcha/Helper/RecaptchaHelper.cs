
//using CHERRY.BUS.ViewModels.Recaptcha.Option;
//using CHERRY.BUS.ViewModels.Recaptcha.Reponse;
//using Microsoft.Extensions.Options;
//using Newtonsoft.Json;
//using System.Net;

//namespace CHERRY.BUS.ViewModels.Recaptcha.Helper
//{
//    public class RecaptchaHelper
//    {
//        private readonly RecaptchaOption _option;

//        public RecaptchaHelper(IOptions<RecaptchaOption> option)
//        {
//            _option = option.Value;
//        }
//        public RecaptchaResponse ValidateCaptcha(string response)
//        {
//            using (var client = new WebClient())
//            {
//                var secret = _option.SecretKey;
//                var url = $"{_option.Url}secret={secret}&response={response}";
//                var result = client.DownloadString(url);

//                try
//                {
//                    var data = JsonConvert.DeserializeObject<RecaptchaResponse>(result.ToString());

//                    return data;
//                }
//                catch (Exception)
//                {
//                    return default;
//                }
//            }
//        }
//    }
//}
