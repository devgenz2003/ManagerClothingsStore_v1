using AutoMapper;
using AutoMapper.QueryableExtensions;
using CHERRY.BUS.Services._1_Interfaces;
using CHERRY.BUS.ViewModels.Cart;
using CHERRY.DAL.ApplicationDBContext;
using CHERRY.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CHERRY.BUS.Services._2_Implements
{
    public class CartService : ICartService
    {
        private readonly CHERRY_DBCONTEXT _dbcontext;
        private readonly IMapper _mapper;
        private readonly ICartProductVariantsService _cartProductVariantsService;
        private readonly UserManager<User> _userManager;

        public CartService(CHERRY_DBCONTEXT dbcontext, IMapper mapper,
            ICartProductVariantsService cartProductVariantsService,
            UserManager<User> usermanager)
        {
            _userManager = usermanager;
            _dbcontext = dbcontext;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _cartProductVariantsService = cartProductVariantsService ?? throw new ArgumentNullException(nameof(cartProductVariantsService));
        }
        public async Task<bool> CreateAsync(CartCreateVM request)
        {
            using (var transaction = _dbcontext.Database.BeginTransaction())
            {
                try
                {
                    // Sử dụng _userManager để kiểm tra xem người dùng có tồn tại không
                    var user = await _userManager.FindByIdAsync(request.ID_User);

                    if (user != null)
                    {
                        var hasCart = await _dbcontext.Cart.AnyAsync(c => c.ID_User == request.ID_User);
                        if (!hasCart)
                        {
                            var cart = new Cart()
                            {
                                ID = Guid.NewGuid(),
                                ID_User = request.ID_User,
                                Status = 1,
                                CreateBy = request.CreateBy
                            };
                            await _dbcontext.Cart.AddAsync(cart);

                            await _dbcontext.SaveChangesAsync();
                            transaction.Commit();
                            return true;
                        }
                    }
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }


        public async Task<List<CartVM>> GetAllActiveAsync()
        {
            var list = await _dbcontext.Cart
                .Where(c => c.Status != 0)
                .ProjectTo<CartVM>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return list;
        }

        public async Task<List<CartVM>> GetAllAsync()
        {
            var list = await _dbcontext.Cart
                .ProjectTo<CartVM>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return list;
        }

        public async Task<CartVM> GetByIDAsync(Guid ID)
        {
            var obj = await _dbcontext.Cart
                .AsQueryable()
                .SingleOrDefaultAsync(c => c.ID == ID);

            var objVM = _mapper.Map<CartVM>(obj);

            return objVM;
        }

        public async Task<CartVM> GetCartByUserIDAsync(string ID_USER)
        {
            var user = await _userManager.FindByIdAsync(ID_USER);

            if (user == null)
            {
                return null;
            }

            var cart = await _dbcontext.Cart.SingleOrDefaultAsync(c => c.ID_User == ID_USER);

            if (cart == null)
            {
                return null;
            }

            var cartVM = _mapper.Map<CartVM>(cart);

            cartVM.Items = await _cartProductVariantsService.GetAllByCartIDAsync(cartVM.ID);

            return cartVM;
        }

        public async Task<bool> RemoveAsync(Guid ID, string IDUserdelete)
        {
            using (var transaction = _dbcontext.Database.BeginTransaction())
            {
                try
                {
                    var obj = await _dbcontext.Cart.FirstOrDefaultAsync(c => c.ID == ID);

                    if (obj != null)
                    {
                        obj.Status = 0;
                        obj.DeleteDate = DateTime.Now;
                        obj.DeleteBy = IDUserdelete;

                        _dbcontext.Cart.Attach(obj);
                        await _dbcontext.SaveChangesAsync();


                        transaction.Commit();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }

        public async Task<bool> UpdateAsync(Guid ID, CartUpdateVM request)
        {
            try
            {
                var listObj = await _dbcontext.Cart.ToListAsync();
                var objForUpdate = listObj.FirstOrDefault(c => c.ID == ID);
                objForUpdate.ID_User = request.ID_User;

                objForUpdate.Status = request.Status;
                objForUpdate.ModifieDate = DateTime.Now;
                objForUpdate.ModifieBy = request.ModifieBy;
                _dbcontext.Cart.Attach(objForUpdate);
                await Task.FromResult<Cart>(_dbcontext.Cart.Update(objForUpdate).Entity);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
