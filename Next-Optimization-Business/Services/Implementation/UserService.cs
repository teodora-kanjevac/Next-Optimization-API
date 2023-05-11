using AutoMapper;
using Newtonsoft.Json;
using NextOptimization.Business.DTOs;
using NextOptimization.Business.Middleware;
using NextOptimization.Business.TokenGenerator;
using NextOptimization.Data.Models;
using NextOptimization.Data.Repositories;
using NextOptimization.Shared.Enums;
using System.Net;
using System.Text;
using System.Transactions;

namespace NextOptimization.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository,
            IMapper mapper, ITokenGenerator tokenGenerator)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<List<UserDTO>> GetAll()
        {
            var users = await _userRepository.GetAll();

            return _mapper.Map<List<UserDTO>>(users);
        }

        public async Task<UserDTO> GetById(string id)
        {
            var user = await _userRepository.GetById(id);

            ApiExceptionHandler.ObjectNotNull(user, $"User with id '{id}'");

            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO> GetByEmail(string email)
        {
            var user = await _userRepository.GetByEmail(email);

            ApiExceptionHandler.ObjectNotNull(user, $"User with email '{email}'");

            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO> GetByUsername(string username)
        {
            User user = await GetLoggedInUser(username);

            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO> Create(UserCreateDTO userCreateDTO)
        {
            User user = _mapper.Map<User>(userCreateDTO);
            user.UserName = userCreateDTO.Email;

            if (await _userRepository.GetByEmail(user.Email) != null)
            {
                ApiExceptionHandler.ThrowApiException(HttpStatusCode.BadRequest, $"User with email '{userCreateDTO.Email}' already exists.");
            }

            using (var scope = new TransactionScope(
                     TransactionScopeOption.Required,
                     new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                     TransactionScopeAsyncFlowOption.Enabled))
            {
                await _userRepository.Create(user);
                await _userRepository.AddToRole(user, Roles.User.ToString());

                var token = EncodeUrl(await _tokenGenerator.GenerateEmailToken(user), user);

                scope.Complete();
            }

            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO> Update(string username, string id, UserUpdateDTO userUpdateDTO)
        {
            User? loggedInUser = await GetLoggedInUser(username);

            if (loggedInUser.Id.Equals(id))
            {
                User user = await _userRepository.GetById(id);

                ApiExceptionHandler.ObjectNotNull(user, $"User with id '{id}'");

                _mapper.Map(userUpdateDTO, user);

                await _userRepository.Update(user);
            }

            ApiExceptionHandler.ThrowApiException(HttpStatusCode.BadRequest, "You are not authorized to update this user.");

            return null;
        }

        public async Task<bool> Delete(string id)
        {
            var user = await _userRepository.GetById(id);

            ApiExceptionHandler.ObjectNotNull(user, $"User with id '{id}'");

            return await _userRepository.Delete(user);
        }

        private async Task<User?> GetLoggedInUser(string username)
        {
            var user = await _userRepository.GetByUsername(username);

            if (user == null)
            {
                ApiExceptionHandler.ThrowApiException(HttpStatusCode.BadRequest, $"No user is logged in.");
            }

            return user;
        }

        public string EncodeUrl(string token, User user)
        {
            UserIdAndTokenForEmailDTO userIdAndToken = new()
            {
                UserId = user.Id,
                Token = token
            };

            string text = JsonConvert.SerializeObject(userIdAndToken);
            byte[] encodedBytes = Encoding.Unicode.GetBytes(text);
            string encodedText = Convert.ToBase64String(encodedBytes);

            return encodedText;
        }
    }
}
