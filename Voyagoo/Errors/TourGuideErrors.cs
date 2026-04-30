using Voyagoo.Abstractions;

namespace Voyagoo.Errors
{
    public static class TourGuideErrors
    {
        public static readonly Error TourGuideNotFound =
            new("TourGuide.NotFound", "Tour guide not found", StatusCodes.Status404NotFound);

        public static readonly Error InvalidImageFile =
            new("TourGuide.InvalidImage", "Invalid image file. Allowed: jpg, jpeg, png, webp", StatusCodes.Status400BadRequest);

        public static readonly Error InvalidLanguage =
            new("TourGuide.InvalidLanguage", "One or more selected languages are invalid", StatusCodes.Status400BadRequest);

        public static readonly Error DuplicateEmail =
            new("TourGuide.DuplicateEmail", "A tour guide with this email already exists", StatusCodes.Status409Conflict);
    }
}
