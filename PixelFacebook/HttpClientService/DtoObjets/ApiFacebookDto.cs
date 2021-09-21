using System.Collections.Generic;

namespace PixelFacebook.HttpClientService.DtoObjets
{
    public class ApiFacebookDto
    {
        public List<Datum> data { get; set; }
    }

    public class Datum
    {
        public string event_name { get; set; }
        public string event_id { get; set; }
        public long event_time { get; set; }
        public string action_source { get; set; }
        public User_Data user_data { get; set; }
        public Custom_Data custom_data { get; set; }
    }

    public class User_Data
    {
        public string[] em = { "7b17fb0bd173f625b58636fb796407c22b3d16fc78302d79f0fd30c2fc2fc068" };
        public string[] ph = { "15e2b0d3c33891ebb0f1ef609ec419420c20e320ce94c65fbc8c3312448eb225" };
    }

    public class Custom_Data
    {
        public string monto { get; set; }
        public string idCredito { get; set; }
    }
}
