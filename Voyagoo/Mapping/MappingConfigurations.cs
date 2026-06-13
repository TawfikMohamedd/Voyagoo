using Mapster;
using Voyagoo.Contracts.Attractions;
using Voyagoo.Contracts.Authentication.Register;
using Voyagoo.Contracts.Hotels;
using Voyagoo.Contracts.Restaurants;
using Voyagoo.Contracts.TourGuides;
using Voyagoo.Entities;
using Voyagoo.Entities.Attractions;
using Voyagoo.Entities.Hotels;
using Voyagoo.Entities.Restaurants;
using Voyagoo.Entities.TourGuides;

namespace Voyagoo.Mapping
{
    public class MappingConfigurations : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<RegisterRequest, ApplicationUser>()
                .Map(dest => dest.UserName, src => src.Email)
                .Map(dest => dest.PhoneNumber, src => src.PhoneNumber);




            #region Restaurants

            config.NewConfig<Restaurant, GetRestaurantsResponse>()
                .Map(dest => dest.CuisineType, src => src.CuisineType.ToString())
                .Map(dest => dest.MainImageUrl,
                  src => src.Images.FirstOrDefault(i => i.IsMain) != null
                 ? src.Images.First(i => i.IsMain).ImageUrl
                 : src.Images.FirstOrDefault() != null
                 ? src.Images.First().ImageUrl
                 : null);

            config.NewConfig<Restaurant, GetRestaurantDetailsResponse>()
            .Map(dest => dest.CuisineType, src => src.CuisineType.ToString())
            .Map(dest => dest.Images, src => src.Images
                .Select(i => new RestaurantImageResponse(i.Id, i.ImageUrl, i.IsMain))
                .ToList())
            .Map(dest => dest.Features, src => src.Features
                .Select(f => new FeatureResponse(f.FeatureId, f.Feature.Name, f.Feature.Icon))
                .ToList())
            .Map(dest => dest.Comments, src => src.Comments
                .Select(c => new CommentResponse(c.Id, c.User.FirstName + " " + c.User.LastName, c.User.ProfilePictureUrl, c.Content, c.Rating, DateOnly.FromDateTime(c.CreatedAt)))
                .ToList());

            config.NewConfig<AddRestaurantRequest, Restaurant>();

            config.NewConfig<AddFeatureRequest, Feature>();


            config.NewConfig<Restaurant, RestaurantAdminItem>()
                .Map(dest => dest.CuisineType, src => src.CuisineType.ToString())
                .Map(dest => dest.Status, src => src.Status.ToString())
                .Map(dest => dest.PriceRange, src => $"{src.MinPrice} - {src.MaxPrice} LE")
                .Map(dest => dest.TotalTables, src => src.TablesForTwo + src.TablesForFour + src.TablesForSix)
                .Map(dest => dest.MainImageUrl,
                     src => src.Images.FirstOrDefault(i => i.IsMain) != null
                         ? src.Images.First(i => i.IsMain).ImageUrl
                         : src.Images.FirstOrDefault() != null
                         ? src.Images.First().ImageUrl
                         : null);


            #endregion

            #region TourGuide

            config.NewConfig<TourGuide, GetTourGuidesResponse>();

            config.NewConfig<TourGuide, GetTourGuideDetailsResponse>()
                .Map(dest => dest.Languages, src => src.Languages.Select(l => l.ToString()).ToList());
           
            config.NewConfig<TourGuide, TourGuideAdminItem>()
                .Map(dest => dest.Status, src => src.Status.ToString());

            #endregion


            #region Attractions
            config.NewConfig<Attraction, GetAttractionsResponse>()
                .Map(dest => dest.Category, src => src.Category.ToString())
                .Map(dest => dest.MainImageUrl,
                    src => src.Images.FirstOrDefault(i => i.IsMain) != null
                 ? src.Images.First(i => i.IsMain).ImageUrl
                 : src.Images.FirstOrDefault() != null
                 ? src.Images.First().ImageUrl
                 : null);

            config.NewConfig<Attraction, GetAttractionDetailsResponse>()
                .Map(dest => dest.Category, src => src.Category.ToString())
                .Map(dest => dest.Images, src => src.Images
                .Select(i => new AttractionImageResponse(i.Id, i.ImageUrl, i.IsMain))
                .ToList());

            config.NewConfig<AddAttractionRequest, Attraction>();

            config.NewConfig<Attraction, AttractionAdminItem>()
                .Map(dest => dest.Category, src => src.Category.ToString())
                .Map(dest => dest.Status, src => src.Status.ToString())
                .Map(dest => dest.MainImageUrl,     
                    src => src.Images.FirstOrDefault(i => i.IsMain) != null
                    ? src.Images.First(i => i.IsMain).ImageUrl
                    : src.Images.FirstOrDefault() != null
                    ? src.Images.First().ImageUrl
                    : null);

            #endregion


            #region Hotels

            config.NewConfig<Hotel, GetHotelsResponse>()
                .Map(dest => dest.MinPrice, src => new[] { src.SinglePrice, src.DoublePrice, src.TriplePrice, src.SuitePrice }
                     .Where(p => p > 0).DefaultIfEmpty(0).Min())
                .Map(dest => dest.MaxPrice, src => new[] { src.SinglePrice, src.DoublePrice, src.TriplePrice, src.SuitePrice }
                     .Where(p => p > 0).DefaultIfEmpty(0).Max())

                .Map(dest => dest.MainImageUrl,
                    src => src.Images.FirstOrDefault(i => i.IsMain) != null
                        ? src.Images.First(i => i.IsMain).ImageUrl
                        : src.Images.FirstOrDefault() != null
                        ? src.Images.First().ImageUrl
                        : null);

            config.NewConfig<Hotel, GetHotelDetailsResponse>()
                .Map(dest => dest.Images, src => src.Images
                    .Select(i => new HotelImageResponse(i.Id, i.ImageUrl, i.IsMain))
                    .ToList())
                .Map(dest => dest.Features, src => src.Features
                    .Select(f => new HotelFeatureResponse(f.HotelFeatureId, f.HotelFeature.Name, f.HotelFeature.Icon))
                    .ToList())
                .Map(dest => dest.Comments, src => src.Comments
                    .Select(c => new HotelCommentResponse(c.Id, c.User.FirstName + " " + c.User.LastName, c.User.ProfilePictureUrl, c.Content, c.Rating, DateOnly.FromDateTime(c.CreatedAt)))
                    .ToList());

            config.NewConfig<AddHotelRequest, Hotel>();

            config.NewConfig<AddHotelFeatureRequest, HotelFeature>();

            config.NewConfig<HotelFeature, HotelFeatureResponse>();

            config.NewConfig<Hotel, HotelAdminItem>()
                .Map(dest => dest.Status, src => src.Status.ToString())
                .Map(dest => dest.PriceRange, src => $"{new[] { src.SinglePrice, src.DoublePrice, src.TriplePrice, src.SuitePrice }.Where(p => p > 0).DefaultIfEmpty(0).Min()} - {new[] { src.SinglePrice, src.DoublePrice, src.TriplePrice, src.SuitePrice }.Max()} LE")
                .Map(dest => dest.TotalRooms, src => src.SingleRooms + src.DoubleRooms + src.TripleRooms + src.SuiteRooms)
                .Map(dest => dest.MainImageUrl,
                    src => src.Images.FirstOrDefault(i => i.IsMain) != null
                        ? src.Images.First(i => i.IsMain).ImageUrl
                        : src.Images.FirstOrDefault() != null
                        ? src.Images.First().ImageUrl
                        : null);




            #endregion

        }
    }
}
