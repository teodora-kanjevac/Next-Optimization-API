using Microsoft.AspNetCore.Identity;
using NextOptimization.Business.DTOs;
using NextOptimization.Business.Services;
using NextOptimization.Business.TokenGenerator;
using NextOptimization.Data.Models;
using NextOptimization.Data.Repositories;
using NextOptimization.Shared.Enums;
using System.Transactions;

namespace NextOptimization.Business.Seeder
{
    public class Seeder : ISeeder
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IRoleService _roleService;
        private readonly UserManager<User> _userManager;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IAuthenticationService _authenticationService;

        public Seeder(IUserRepository userRepository, IRoleService roleService, UserManager<User> userManager, IAuthenticationService authenticationService, IRoleRepository roleRepository, ITokenGenerator tokenGenerator)
        {
            _userRepository = userRepository;
            _roleService = roleService;
            _userManager = userManager;
            _authenticationService = authenticationService;
            _roleRepository = roleRepository;
            _tokenGenerator = tokenGenerator;
        }

        public async Task Seed()
        {
            foreach (var roleEnum in Enum.GetValues(typeof(Roles)))
            {
                var role = await _roleRepository.GetByName(roleEnum.ToString());

                if (role != null)
                {
                    continue;
                }

                RoleCreateDTO rolesDTO = new()
                {
                    Name = roleEnum.ToString()
                };

                await _roleService.Create(rolesDTO);
            }

            var users = await _userRepository.GetAll();

            if (users == null || users.Count == 0)
            {
                await CreateUser("nextoptimizationadmin@gmail.com");
            }
        }

        private async Task CreateUser(string email)
        {
            User user = new()
            {
                Email = email,
                UserName = email
            };

            await _userManager.CreateAsync(user);
            await _userManager.AddToRoleAsync(user, Roles.Admin.ToString());
            user = await _userRepository.GetByEmail(email);

            string token = await _tokenGenerator.GenerateEmailToken(user);

            using (var scope = new TransactionScope(
                      TransactionScopeOption.Required,
                      new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                      TransactionScopeAsyncFlowOption.Enabled))
                try
                {
                    await _authenticationService.ConfirmEmail(user, token);
                    await _authenticationService.AddPassword(user, "PcOptimizationAdmin003");

                    scope.Complete();
                }
                catch { throw; }
        }
    }
}
