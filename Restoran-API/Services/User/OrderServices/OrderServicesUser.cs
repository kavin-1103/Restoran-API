using AutoMapper;
using Restaurant_Reservation_Management_System_Api.Data;
using Restaurant_Reservation_Management_System_Api.Dto.User.Order;
using Restaurant_Reservation_Management_System_Api.Model;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Globalization;
using Restaurant_Reservation_Management_System_Api.Dto.Auth;
using EmailService;
using System.Text;

namespace Restaurant_Reservation_Management_System_Api.Services.User.OrderServices
{
    public class OrderServicesUser : IOrderServicesUser
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly RestaurantDbContext _context;
		private readonly IEmailSender _emailSender;

		private readonly IMapper _mapper;

        public OrderServicesUser(RestaurantDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _emailSender = emailSender; 

        }
      
        //service to place order
        public async Task<ServiceResponse<GetOrderDtoUser>> AddOrder(string customerIdClaim , AddOrderDtoUser addOrderDtoUser)
        {
            var serviceResponse = new ServiceResponse<GetOrderDtoUser>();

            var customerId = customerIdClaim;

             
                if (customerIdClaim == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "The User Not Authorized!";
                    return serviceResponse;
                }
                

                var order = new Order()
                {
                    ApplicationUserId = customerId ,
                    OrderDate = DateTime.Now,
                    TableId = addOrderDtoUser.TableId,

                };
                _context.Orders.Add(order);

                await _context.SaveChangesAsync();
                
                //finding the user with id
			    var applicationUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == customerId);

                //getting the table with id
			    var table = await _context.Tables.FirstOrDefaultAsync(t => t.TableId == order.TableId);






			foreach (var orderItemDto in addOrderDtoUser.OrderItems)
                {
                    var orderItem = new OrderItem()
                    {
                        

                        FoodItemId = orderItemDto.FoodItemId,
                        OrderId = order.OrderId,
                        Quantity = orderItemDto.Quantity,
                    };
                    await _context.OrderItems.AddAsync(orderItem);
                }
                await _context.SaveChangesAsync();

            //creating new GetOrderDto

            var getOrderDtoUser = new GetOrderDtoUser()
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                CustomerId = order.ApplicationUserId,
                CustomerName = applicationUser?.Name,
                TableNumber = table?.TableNumber,         
                TableId = order.TableId ,
                OrderItems = addOrderDtoUser.OrderItems ,

                };

                serviceResponse.Data = getOrderDtoUser;
                serviceResponse.Success = true;
			    serviceResponse.Message = "Ordered Successfully";

			var orderedTable = await _context.Tables.FirstOrDefaultAsync(t=>t.TableId == addOrderDtoUser.TableId);

			var orderedTableNumber = orderedTable?.TableNumber;


			var customerName = applicationUser.Name;


			//message to display in email
			var employeeMessage = new Message(new string[] { applicationUser.Email }, "Booking Successful", $"\n\nDear {customerName},\n\n" +
							 "Your reservation at 'Restoran' has been successful!\n\n" +
							 $"Table Number: {orderedTableNumber}\n" +						 
							 "If you have any questions or need assistance, please don't hesitate to reach out to us." + "\n\n" +
							 "Best regards,\n" +
							 "Restoran");
			//sending email to the registered customer

			_emailSender.SendEmail(employeeMessage);


			return serviceResponse;

        }

        //service to get the order details of particular customer

        public async Task<ServiceResponse<IEnumerable<GetAllOrderDto>>> OrderDetails(string customerIdClaim)
        {
            var serviceResponse = new ServiceResponse<IEnumerable<GetAllOrderDto>>();

            var customerId = customerIdClaim;

            // Get all orders for the given customer
            var orders = await _context.Orders
                .Include(o => o.Table)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.FoodItem)
                .Where(o => o.ApplicationUserId == customerId)
                .ToListAsync();

            // Map the Order entities to GetAllOrderDto using AutoMapper
            var getOrderDtos = _mapper.Map<IEnumerable<GetAllOrderDto>>(orders);

            foreach (var orderDto in getOrderDtos)
            {
                var customer = await _userManager.FindByIdAsync(orderDto.CustomerId);
                orderDto.CustomerName = customer?.Name;
            }

            serviceResponse.Data = getOrderDtos;
            return serviceResponse;
        }
        
        public async Task<ServiceResponse<IEnumerable<GetAllOrderDto>>> GetAllOrders()
        {
            var serviceResponse = new ServiceResponse<IEnumerable<GetAllOrderDto>>();

            // Get all orders from the database
            var orders = await _context.Orders
                .Include(o => o.Table)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.FoodItem)
                .ToListAsync();

            var getOrderDtos = _mapper.Map<IEnumerable<GetAllOrderDto>>(orders);

            // Retrieve the ApplicationUser for each Order and set the CustomerName property
            foreach (var orderDto in getOrderDtos)
            {
                var customer = await _userManager.FindByIdAsync(orderDto.CustomerId);
                orderDto.CustomerName = customer?.Name;
            }

            serviceResponse.Data = getOrderDtos;
            return serviceResponse;
        }


		public async Task<ServiceResponse<(List<string> Dates, List<int> Counts)>> GetOrderCountForLast7Days()
		{
			var serviceResponse = new ServiceResponse<(List<string>, List<int>)>();

			// Calculate the date 7 days ago from today
			var last7Days = DateTime.Now.Date.AddDays(-6); // To include today, we need to subtract 6 instead of 7.

			// Get all orders from the database for the last 7 days
			var orders = await _context.Orders
				.Where(o => o.OrderDate >= last7Days)
				.ToListAsync();

			// Group orders by order date and count the number of orders in each group
			var ordersPerDay = orders
				.GroupBy(o => o.OrderDate.Date) // Group by the date part only, ignoring the time
				.Select(g => new { OrderDate = g.Key, Count = g.Count() })
				.ToList();

			// Create a list of dates and counts for the last seven days
			var datesForLast7Days = Enumerable.Range(0, 7)
				.Select(offset => last7Days.AddDays(offset).ToString("MM-dd-yyyy"))
				.ToList();

			var countsForLast7Days = datesForLast7Days
				.GroupJoin(
					ordersPerDay,
					date => DateTime.ParseExact(date, "MM-dd-yyyy", CultureInfo.InvariantCulture),
					order => order.OrderDate.Date,
					(date, orderGroup) => orderGroup.Any() ? orderGroup.First().Count : 0
				)
				.ToList();

			// Set the dates and counts as the Data property of the ServiceResponse
			serviceResponse.Data = (datesForLast7Days, countsForLast7Days);

			return serviceResponse;
		}


		public async Task<ServiceResponse<IEnumerable<OrderDto>>> GetOrdersForCustomer(string customerId)
		{

			var serviceResponse = new ServiceResponse<IEnumerable<OrderDto>>();

			// Retrieve the orders and their associated order items, food items, and reservation dates
			var orders = await _context.Orders
				.Include(o => o.OrderItems)
				.ThenInclude(oi => oi.FoodItem)
				.Include(o => o.Table)
				.ThenInclude(t => t.Reservations)
				.Where(o => o.ApplicationUserId == customerId)
				.ToListAsync();

			// Map the orders to the DTO
			var orderDtos = new List<OrderDto>();

			foreach (var order in orders)
			{
				var reservationDate = order.Table.Reservations.FirstOrDefault()?.ReservationDate;
				var orderDto = new OrderDto
				{
					OrderId = order.OrderId,
					OrderDate = order.OrderDate,
					ReservationDate = reservationDate,
					TableNumber = order.Table.TableNumber,
					FoodItems = order.OrderItems.Select(oi => new FoodItemDto
					{
						FoodItem = oi.FoodItem.ItemName,
						Quantity = oi.Quantity,
						Price = oi.FoodItem.Price
					}).ToList()
				};

				orderDtos.Add(orderDto);
			}

			serviceResponse.Data = orderDtos;
			serviceResponse.Message = "All the Orders Of You!";
			serviceResponse.Success = true;
			return serviceResponse;

		}

		public async Task<ServiceResponse<int>> GetTotalOrderCount()
		{
			var serviceResponse = new ServiceResponse<int>();

			// Get the total count of orders from the database
			var totalCount = await _context.Orders.CountAsync();

			// Set the total order count in the Data property of the ServiceResponse
			serviceResponse.Data = totalCount;

			return serviceResponse;
		}


	}
}
