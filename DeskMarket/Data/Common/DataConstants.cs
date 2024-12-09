namespace DeskMarket.Data.Common
{
    public static class DataConstants
    {
        // Product constants:
        public const int ProductNameMinLength = 2;
        public const int ProductNameMaxLength = 60;

        public const string ProductNameRequiredErrorMessage = "Product Name is required,";
        public const string ProductNameStringLengthMessage = "Product Name should be between {2} and {1} charcters long.";

        public const int DescriptionMinLength = 10;
        public const int DescriptionMaxLength = 250;

        public const string PriceMinValue = "1.00";
        public const string PriceMaxValue = "3000.00";
        public const decimal BgnToEurRate = 1.95583m;

        public const int ImageUrlMaxLength = 250;

        public const string DateFormat = "dd-MM-yyyy";
        public const string DateRegex = @"^\d{2}-\d{2}-\d{4}$";
        public const string DateFormatErrorMessage = "Invalid date format.";
        public const string DateInvalidErrorMessage = "Invalid date.";
        public const string AddedOnRequiredErrorMessage = "Added on id required.";

        // Category constants:
        public const int CategoryNameMinLength = 3;
        public const int CategoryNameMaxLength = 20;

        public const string CategoryErrorMessage = "This category does not exist.";

        // Names of actions and controllers:
        public const string IndexAction = "Index";
        public const string ProductContr = "Product";
    }
}
