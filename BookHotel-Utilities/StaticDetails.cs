namespace BookHotel_Utilities
{
    public static class StaticDetails
    {
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
        public static string SessionToken = "JWTToken";
        public const string Admin = "admin";
        public const string Customer = "customer";

        
        public enum ContentType
        {
            json,
            MultipartFormData,
        }

    }
}
