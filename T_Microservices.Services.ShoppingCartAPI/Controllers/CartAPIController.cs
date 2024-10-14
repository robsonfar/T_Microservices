using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using T_Microservices.Services.ShoppingCartAPI.Models.Dto;

using T_Microservices.Services.ShoppingCartAPI.Models;
using T_Microservices.Services.ShoppingCartAPI.Data;
using Microsoft.EntityFrameworkCore;
using T_Microservices.Services.ShoppingCartAPI.Service.IService;

namespace T_Microservices.Services.ShoppingCartAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartAPIController : ControllerBase
    {
        private ResponseDto _response;
        private IMapper _mapper;
        private readonly ApplicationDbContext _dbContext;
        private IProductService _productService;
        private ICouponService _couponService;

        public CartAPIController(ApplicationDbContext dbContext
            , IMapper mapper
            , IProductService productService
            , ICouponService couponService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _productService = productService;
            _couponService = couponService;

            _response = new ResponseDto();
        }


        [HttpGet("GetCart/{userId}")]
        public async Task<ResponseDto> GetCart(string userId)
        {
            try
            {
                CartDto cart = new()
                {
                    CartHeader = _mapper.Map<CartHeaderDto>(_dbContext.CartHeader.First(u => u.UserId == userId))
                };

                cart.CartDetails = _mapper.Map<IEnumerable<CartDetailsDto>>(_dbContext.CartDetails.Where(u => u.CartHeaderId == cart.CartHeader.Id));

                IEnumerable<ProductDto> productDtos = await _productService.GetProducts();

                foreach (var item in cart.CartDetails)
                {
                    item.Product = productDtos.FirstOrDefault(u => u.Id == item.ProductId);
                    cart.CartHeader.CartTotal += (item.Count * item.Product.Price);
                }

                // Apply coupon if any
                if (!string.IsNullOrEmpty(cart.CartHeader.CouponCode))
                {
                    CouponDto coupon = await _couponService.GetCoupon(cart.CartHeader.CouponCode);

                    if (coupon != null && cart.CartHeader.CartTotal > coupon.MinAmount)
                    {
                        cart.CartHeader.CartTotal -= coupon.DiscountAmount;
                        cart.CartHeader.Discount = coupon.DiscountAmount;
                    }
                }

                _response.Result = cart;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpPost("CartUpsert")]
        public async Task<ResponseDto> CartUpsert(CartDto cartDto)
        {
            try
            {
                var cartHeaderFromDb = await _dbContext.CartHeader.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == cartDto.CartHeader.UserId);

                if (cartHeaderFromDb == null)
                {
                    // Create header and details
                    CartHeader cartHeader = _mapper.Map<CartHeader>(cartDto.CartHeader);

                    _dbContext.CartHeader.Add(cartHeader);
                    await _dbContext.SaveChangesAsync();

                    cartDto.CartDetails.First().CartHeaderId = cartHeader.Id;

                    _dbContext.CartDetails.Add(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    // If header is not null check if details has same product
                    var cartDetailsFromDb = await _dbContext.CartDetails.AsNoTracking().FirstOrDefaultAsync(u => 
                        u.ProductId == cartDto.CartDetails.First().ProductId 
                        && u.CartHeaderId == cartHeaderFromDb.Id);

                    if (cartDetailsFromDb == null)
                    {
                        // Create cartdetails
                        cartDto.CartDetails.First().CartHeaderId = cartHeaderFromDb.Id;

                        _dbContext.CartDetails.Add(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                        await _dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        // Update count in cart details
                        cartDto.CartDetails.First().Count += cartDetailsFromDb.Count;
                        cartDto.CartDetails.First().CartHeaderId = cartDetailsFromDb.CartHeaderId;
                        cartDto.CartDetails.First().Id = cartDetailsFromDb.Id;

                        _dbContext.CartDetails.Update(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                        await _dbContext.SaveChangesAsync();
                    }
                }

                _response.Result = cartDto;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message.ToString();
                _response.IsSuccess = false;
            }

            return _response;
        }

        [HttpPost("RemoveCart")]
        public async Task<ResponseDto> RemoveCart([FromBody] int cartDetailsId)
        {
            try
            {
                CartDetails cartDetails = _dbContext.CartDetails.First(u => u.Id == cartDetailsId);

                int totalCountofCartItem = _dbContext.CartDetails.Where(u => u.CartHeaderId == cartDetails.CartHeaderId).Count();

                _dbContext.CartDetails.Remove(cartDetails);

                // Verify if it's the last item
                if (totalCountofCartItem == 1)
                {
                    var cartHeaderToRemove = await _dbContext.CartHeader.FirstOrDefaultAsync(u => u.Id == cartDetails.CartHeaderId);

                    _dbContext.CartHeader.Remove(cartHeaderToRemove);
                }

                await _dbContext.SaveChangesAsync();

                _response.Result = true;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message.ToString();
                _response.IsSuccess = false;
            }

            return _response;
        }

        [HttpPost("ApplyCoupon")]
        public async Task<object> ApplyCoupon([FromBody] CartDto cartDto)
        {
            try
            {
                var cartFromDb = await _dbContext.CartHeader.FirstAsync(u => u.UserId == cartDto.CartHeader.UserId);

                cartFromDb.CouponCode = cartDto.CartHeader.CouponCode;

                _dbContext.CartHeader.Update(cartFromDb);
                await _dbContext.SaveChangesAsync();

                _response.Result = true;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.ToString();
            }

            return _response;
        }

    }
}
