using Voyagoo.Abstractions;

namespace Voyagoo.Errors
{
    public static class RestaurantErrors
    {
        public static readonly Error RestaurantNotFound =
            new("Restaurant.NotFound", "Restaurant not found", StatusCodes.Status404NotFound);

        public static readonly Error FeatureNotFound =
            new("Feature.NotFound", "One or more features not found", StatusCodes.Status404NotFound);

        public static readonly Error DuplicateFeature =
            new("Feature.Duplicate", "A feature with the same name already exists", StatusCodes.Status409Conflict);

        public static readonly Error InvalidImageFile =
            new("Restaurant.InvalidImage", "Invalid image file", StatusCodes.Status400BadRequest);

        public static readonly Error NoMainImage =
            new("Restaurant.NoMainImage", "Restaurant must have at least one main image", StatusCodes.Status400BadRequest);
        
        public static readonly Error ImageNotFound =
            new("Restaurant.ImageNotFound", "Image not found", StatusCodes.Status404NotFound);

        public static readonly Error CommentNotFound =
            new("Restaurant.CommentNotFound", "Comment not found", StatusCodes.Status404NotFound);
    }
}
