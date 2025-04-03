using AutoMapper;
using PRN222.Milktea.Repository.Models;
using PRN222.Milktea.Service.BusinessModels;

namespace PRN222.Milktea.Service.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Các ánh xạ giữa Model và ViewModel
            CreateMap<AccountModel, Account>().ReverseMap();
            CreateMap<CategoryModel, Category>().ReverseMap();
            CreateMap<ComboModel, Combo>().ReverseMap();
            CreateMap<OrderDetailModel, OrderDetail>().ReverseMap();
            CreateMap<PaymentModel, Payment>().ReverseMap();
            CreateMap<ProductModel, Product>().ReverseMap();

            // Ánh xạ OrderModel với Order (giữa Model và Entity)
            CreateMap<OrderModel, Order>().ReverseMap();

            // Ánh xạ giữa các lớp OrderViewModel và Order
            CreateMap<Order, OrderViewModel>()
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId))
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.OrderDate))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount));

            // Ánh xạ từ OrderDetail (Entity) sang OrderDetailViewModel
            CreateMap<OrderDetail, OrderDetailViewModel>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice));

            // Các ánh xạ khác
            CreateMap<Account, AccountModelAdmin>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive ?? true));
            CreateMap<Account, CustomerProfile>().ReverseMap();
            CreateMap<ProductViewModel, Product>().ReverseMap();
            CreateMap<Payment, PaymentViewModel>();
        }
    }
}
