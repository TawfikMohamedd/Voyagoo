using Voyagoo.Abstractions;

namespace Voyagoo.Errors
{
    public static class AttractionErrors
    {
        public static readonly Error AttractionNotFound =
            new("Attraction.NotFound", "Attraction not found", StatusCodes.Status404NotFound);

        public static readonly Error InvalidImageFile =
            new("Attraction.InvalidImage", "Invalid image file. Allowed: jpg, jpeg, png, webp", StatusCodes.Status400BadRequest);

        public static readonly Error ImageNotFound =
             new("Attraction.ImageNotFound", "Image not found", StatusCodes.Status404NotFound);
    }
}
