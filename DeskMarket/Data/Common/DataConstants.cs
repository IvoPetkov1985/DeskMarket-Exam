namespace DeskMarket.Data.Common
{
    public static class DataConstants
    {
        // Category constants:
        public const int CategoryNameMinLength = 3;
        public const int CategoryNameMaxLength = 20;

        public const string CategoryErrorMsg = "Invalid category";

        // Product constants:
        public const int ProductNameMinLength = 2;
        public const int ProductNameMaxLength = 60;

        public const int ProductDescriptionMinLength = 10;
        public const int ProductDescriptionMaxLength = 250;

        public const string ProductMinPrice = "1.0";
        public const string ProductMaxPrice = "3000.0";

        public const string DateTimeFormat = "dd-MM-yyyy";
        public const string DateTimeRegex = @"^\d{2}-\d{2}-\d{4}$";
        public const string DateTimeErrorMsg = "Invalid date format!";

        // Actions' and controllers' names:
        public const string IndexActionName = "Index";
        public const string ProductContrName = "Product";
    }
}
