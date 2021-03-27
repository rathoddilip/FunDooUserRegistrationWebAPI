using AutoMapper;
using CommonLayer.ResponseModel;
using RepositoryLayer.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserModel>();
            CreateMap<RegisterModel, User>();
            CreateMap<UpdateModel, User>();
            CreateMap<AuthenticateModel, User>();
        }
    }
}
