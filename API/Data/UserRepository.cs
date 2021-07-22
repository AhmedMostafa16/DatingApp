using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public UserRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<MemberDTO> GetMemberAsync(string username)
        {
            return await _context.Users.Where(x => x.UserName == username)
            .ProjectTo<MemberDTO>(_mapper.ConfigurationProvider).SingleOrDefaultAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<MemberDTO>> GetMembersAsync()
        {
            return await _context.Users
                .ProjectTo<MemberDTO>(_mapper.ConfigurationProvider)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id).ConfigureAwait(false);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.Include(p => p.Photos).SingleOrDefaultAsync(x => x.UserName == username).ConfigureAwait(false);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _context.Users.Include(p => p.Photos).ToListAsync().ConfigureAwait(false);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }
    }
}