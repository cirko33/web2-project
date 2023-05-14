﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using OnlineStoreApp.DTOs;
using OnlineStoreApp.Exceptions;
using OnlineStoreApp.Interfaces;
using OnlineStoreApp.Interfaces.IServices;
using BC = BCrypt.Net;

namespace OnlineStoreApp.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProfileService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task EditProfile(int id, EditProfileDTO profile)
        {
            var user = await _unitOfWork.Users.Get(x => x.Id == id);
            if (user == null)
                throw new BadRequestException("Error with id in token. Logout and login again");

            if (profile.Password != null && profile.NewPassword != null)
            {
                if (!BC.BCrypt.Verify(profile.Password, user.Password))
                    throw new BadRequestException("Password doesn't match");

                user.Password = BC.BCrypt.HashPassword(profile.NewPassword);
            }
            user.Address = profile.Address;
            if(user.Email != profile.Email)
                if ((await _unitOfWork.Users.Get(x => x.Email == profile.Email)) != null)
                    throw new BadRequestException("Email already exists.");
            user.Email = profile.Email;

            user.Birthday = profile.Birthday;
            user.FullName = profile.FullName;
            user.Image = profile.Image;

            if (user.Username != profile.Username)
                if ((await _unitOfWork.Users.Get(x => x.Username == profile.Username)) != null)
                    throw new BadRequestException("Username already exists.");
            user.Username = profile.Username;

            _unitOfWork.Users.Update(user);
            await _unitOfWork.Save();
        }

        public async Task<ProfileDTO> GetProfile(int id)
        {
            var user = await _unitOfWork.Users.Get(x => x.Id == id);
            if (user == null)
                throw new BadRequestException("Error with id in token. Logout and login again");

            return _mapper.Map<ProfileDTO>(user);
        }
    }
}
