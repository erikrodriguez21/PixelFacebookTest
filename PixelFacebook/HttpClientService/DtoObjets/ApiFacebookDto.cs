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
        public User_Data user_data { get; set; }
        public Custom_Data custom_data { get; set; }
        public string event_source_url { get; set; }
        public string action_source { get; set; }
    }

    public class User_Data
    {
        public string[] em { get; set; }
        public string client_ip_address { get; set; }
        public string client_user_agent { get; set; }
    }

    public class Custom_Data 
    {
        public string value { get; set; }
        public string currency { get; set; }
    }
}
