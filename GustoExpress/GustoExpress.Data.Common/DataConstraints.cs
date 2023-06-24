namespace GustoExpress.Data.Common
{
    public static class DataConstraints
    {
        public static class Restaurant
        {
            public const int RESTAURANT_NAME_MIN_LENGHT = 3;
            public const int RESTAURANT_NAME_MAX_LENGHT = 20;
            public const int RESTAURANT_DESCRIPTION_MAX_LENGHT = 100;
            public const int RESTAURANT_DESCRIPTION_MIN_LENGHT = 5;
        }

        public static class Product
        {
            public const int PRODUCT_NAME_MIN_LENGHT = 3;
            public const int PRODUCT_NAME_MAX_LENGHT = 35;
            public const int PRODUCT_DESCRIPTION_MAX_LENGHT = 100;
            public const int PRODUCT_DESCRIPTION_MIN_LENGHT = 5;
        }

        public static class Offer
        {
            public const int OFFER_NAME_MIN_LENGHT = 3;
            public const int OFFER_NAME_MAX_LENGHT = 35;
            public const int OFFER_DESCRIPTION_MAX_LENGHT = 100;
            public const int OFFER_DESCRIPTION_MIN_LENGHT = 5;
        }

        public static class City
        {
            public const int CITY_NAME_MIN_LENGHT = 1;
            public const int CITY_NAME_MAX_LENGHT = 85;
        }

        public static class Review
        {
            public const int REVIEW_TITLE_MIN_LENGHT = 3;
            public const int REVIEW_TITLE_MAX_LENGHT = 40;
            public const int REVIEW_TEXT_MIN_LENGHT = 5;
            public const int REVIEW_TEXT_MAX_LENGHT = 250;
        }
    }
}
