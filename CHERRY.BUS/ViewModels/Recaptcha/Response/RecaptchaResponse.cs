using Newtonsoft.Json;

namespace CHERRY.BUS.ViewModels.Recaptcha.Reponse
{
    public class RecaptchaResponse
    {
        [JsonProperty("success")] public bool Success { get; set; }

        [JsonProperty("error-codes")] public List<string> ErrorMessages { get; set; }
    }
}
