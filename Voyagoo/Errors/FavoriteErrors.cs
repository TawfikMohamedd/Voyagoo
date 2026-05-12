using Voyagoo.Abstractions;

namespace Voyagoo.Errors
{
    public static class FavoriteErrors
    {
        public static readonly Error AlreadyFavorited =
            new("Favorite.AlreadyExists", "Already added to favorites", StatusCodes.Status409Conflict);

        public static readonly Error FavoriteNotFound =
            new("Favorite.NotFound", "Favorite not found", StatusCodes.Status404NotFound);

        public static readonly Error InvalidFavoriteType =
            new("Favorite.InvalidType", "You must provide exactly one of: restaurantId, tourGuideId, attractionId", StatusCodes.Status400BadRequest);
    }
}
