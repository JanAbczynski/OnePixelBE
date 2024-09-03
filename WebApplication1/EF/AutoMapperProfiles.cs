using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnePixelBE.Models;
using OnePixelBE.ViewModel;

namespace OnePixelBE.EF
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<UserViewModel, User>();
            CreateMap<User, UserViewModel>();

            CreateMap<ShootingRangeViewModel, ShootingRange>();
            CreateMap<ShootingRange, ShootingRangeViewModel>();

            CreateMap<OneRangeViewModel, OneRange>();
            CreateMap<OneRange, OneRangeViewModel>();

            CreateMap<AddressViewModel, Address>();
            CreateMap<Address, AddressViewModel>();

            CreateMap<CustomFileViewModel, CustomFile>();
            CreateMap<CustomFile, CustomFileViewModel>();

            CreateMap<TargetViewModel, Target>();
            CreateMap<Target, TargetViewModel>();

            CreateMap<PointsViewModel, PointModel>();
            CreateMap<PointModel, PointsViewModel>();

            CreateMap<CrewStandViewModel, CrewStand>();
            CreateMap<CrewStand, CrewStandViewModel>();

            CreateMap<UserPermissionViewModel, UserPermission>();
            CreateMap<UserPermission, UserPermissionViewModel>();

            CreateMap<UserPermissionDetailViewModel, UserPermissionDetail>();
            CreateMap<UserPermissionDetail, UserPermissionDetailViewModel>();

            CreateMap<AvaliblePermissionsViewModel, AvaliblePermissions>();
            CreateMap<AvaliblePermissions, AvaliblePermissionsViewModel>();

            CreateMap<AvaliblePermissionDetailViewModel, AvaliblePermissionDetail>();
            CreateMap<AvaliblePermissionDetail, AvaliblePermissionDetailViewModel>();

            CreateMap<FieldModel, FieldViewModel>();
            CreateMap<FieldViewModel, FieldModel>();
        }
    }
}
