using Voyagoo.Abstractions;

namespace Voyagoo.Errors
{
    public static class HotelErrors
    {
        public static readonly Error HotelNotFound =
            new("Hotel.NotFound", "Hotel not found", StatusCodes.Status404NotFound);

        public static readonly Error FeatureNotFound =
            new("HotelFeature.NotFound", "One or more features not found", StatusCodes.Status404NotFound);

        public static readonly Error DuplicateFeature =
            new("HotelFeature.Duplicate", "A feature with the same name already exists", StatusCodes.Status409Conflict);

        public static readonly Error InvalidImageFile =
            new("Hotel.InvalidImage", "Invalid image file. Allowed: jpg, jpeg, png, webp", StatusCodes.Status400BadRequest);

        public static readonly Error ImageNotFound =
            new("Hotel.ImageNotFound", "Image not found", StatusCodes.Status404NotFound);
        public static readonly Error CommentNotFound =
            new("Hotel.CommentNotFound", "Comment not found", StatusCodes.Status404NotFound);

        public static readonly Error CommentNotOwned =
            new("Hotel.CommentNotOwned", "You can only delete your own comments", StatusCodes.Status403Forbidden);

        public static readonly Error BookingFeatureNotFound =
             new("BookingFeature.NotFound", "One or more booking features not found", StatusCodes.Status404NotFound);

        public static readonly Error DuplicateBookingFeature =
            new("BookingFeature.Duplicate", "A booking feature with the same name already exists", StatusCodes.Status409Conflict);
    }
}
