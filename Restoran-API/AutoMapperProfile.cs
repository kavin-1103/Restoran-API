
using AutoMapper;

using Restaurant_Reservation_Management_System_Api.Model;

using Restaurant_Reservation_Management_System_Api.Dto.Admin.Table;
using Restaurant_Reservation_Management_System_Api.Dto.Admin.MenuCategory;
using Restaurant_Reservation_Management_System_Api.Dto.Admin.FoodItem;
using Restaurant_Reservation_Management_System_Api.Dto.User.Order;

namespace Restaurant_Reservation_Management_System_Api
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {

            CreateMap<Table, GetAllTableDtoAdmin>();

            CreateMap<GetAllTableDtoAdmin,Table>();

            CreateMap<AddTableDtoAdmin, Table>().ReverseMap();

            CreateMap<GetMenuCategoryDtoAdmin, MenuCategory>().ReverseMap();

            CreateMap<GetFoodItemDtoAdmin,FoodItem>().ReverseMap(); 

            CreateMap<AddFoodItemDtoAdmin ,FoodItem>().ReverseMap();

            CreateMap<Order, GetAllOrderDto>()
               .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.ApplicationUserId))
               .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.ApplicationUser.UserName))
               .ForMember(dest => dest.TableNumber, opt => opt.MapFrom(src => src.Table.TableNumber))
               .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));

            CreateMap<OrderItem, GetOrderItemDto>()
               .ForMember(dest => dest.FoodItemId, opt => opt.MapFrom(src => src.FoodItemId))
               .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.FoodItem.ItemName));

        }
    }
}
