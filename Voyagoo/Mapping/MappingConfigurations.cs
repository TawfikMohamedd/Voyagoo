using Mapster;
using Voyagoo.Contracts.Authentication.Register;
using Voyagoo.Contracts.Restaurants;
using Voyagoo.Entities;
using Voyagoo.Entities.Restaurants;

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
    .Map(dest => dest.MainImageUrl,
         src => src.Images.FirstOrDefault(i => i.IsMain) != null
             ? src.Images.First(i => i.IsMain).ImageUrl
             : src.Images.FirstOrDefault() != null
             ? src.Images.First().ImageUrl
             : null);

            config.NewConfig<Restaurant, GetRestaurantDetailsResponse>()
                .Map(dest => dest.ImageUrls, src => src.Images.Select(i => i.ImageUrl).ToList())
                .Map(dest => dest.Features, src => src.Features.Select(f => new FeatureResponse(f.FeatureId, f.Feature.Name, f.Feature.Icon)).ToList())
                .Map(dest => dest.Comments, src => src.Comments.Select(c => new CommentResponse(c.Id, c.User.FirstName + " " + c.User.LastName, c.Content, c.Rating, c.CreatedAt)).ToList());

            config.NewConfig<AddRestaurantRequest, Restaurant>();

            config.NewConfig<AddFeatureRequest, Feature>();

            #endregion







        }
    }
}
