using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using OnePixelBE.EF;
using OnePixelBE.Models;
using OnePixelBE.ViewModel;

namespace OnePixelBE.Services
{
    public class CommonService
    {
        readonly AppDbContext _context;

        public CommonService(AppDbContext context) 
        {
            _context = context;
        }

        internal List<EnumViewModel> EnumToList<T>()
        {
            List<EnumViewModel> enumVMList = new List<EnumViewModel>() { };
            foreach (Enum oneEnum in Enum.GetValues(typeof(T)))
            {
                EnumViewModel newEnum = ReadEnumData(oneEnum);
                enumVMList.Add(newEnum);
            }

            return enumVMList;
        }

        internal EnumViewModel ReadEnumData(Enum oneEnum)
        {
            EnumViewModel newEnum = new EnumViewModel() { };
            newEnum.Id = GetEnumID(oneEnum);
            newEnum.Description = GetEnumDescription(oneEnum);
            newEnum.Value = oneEnum.ToString();
            return newEnum;
        }


        private int GetEnumID(Enum myEnum)
        {
            var field = myEnum.GetType()
                .GetField(myEnum.ToString());

            int attr = (int)field.GetValue(myEnum);

            return attr;
        }


        private string GetEnumDescription(Enum myEnum)
        {
            var description = "None";
            var field = myEnum.GetType()
                .GetField(myEnum.ToString());

            var attr = field.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            if (attr.Length > 0)
            {
                var da =
                    attr[0] as DescriptionAttribute;

                description = da.Description;
            }
            return description;
        }

        internal void TryDeleteMenu(Guid menuPartId)
        {
            MenuPart menuPartDB = _context.MenuParts.FirstOrDefault(x => x.Id == menuPartId);
            _context.Remove(menuPartDB);
            _context.SaveChanges();
        }

        internal void TryUpdateMenu(MenuPart menuPart)
        {
            if(menuPart.Id == Guid.Empty)
            {
                MenuPart menuPartNew = new MenuPart() { };
                menuPartNew.Id = Guid.NewGuid();
                menuPartNew.UserType = menuPart.UserType;
                menuPartNew.UserRole = menuPart.UserRole;
                menuPartNew.MainMenu = menuPart.MainMenu;
                menuPartNew.SubMenu = menuPart.SubMenu;
                menuPartNew.RouterLink = menuPart.RouterLink;

                _context.Add(menuPartNew);
                
            }
            else
            {
                MenuPart menuPartDB = _context.MenuParts.FirstOrDefault(x => x.Id == menuPart.Id);

                menuPartDB.Id = menuPart.Id;
                menuPartDB.UserType = menuPart.UserType;
                menuPartDB.UserRole = menuPart.UserRole;
                menuPartDB.MainMenu = menuPart.MainMenu;
                menuPartDB.SubMenu = menuPart.SubMenu;
                menuPartDB.RouterLink = menuPart.RouterLink;
            }

            _context.SaveChanges();
        }

        internal object FindGunEnumByString<T>(string myString)
        {

            T myEnum = (T)Enum.Parse(typeof(T), myString);

            return myEnum;
        }

        internal List<EnumViewModel> EnumToList2(Type type)
        {
            List<EnumViewModel> enumVMList = new List<EnumViewModel>() { };
            foreach (var oneEnum in type.GetEnumValues())
            {
                EnumViewModel enumVM = new EnumViewModel() { };
                enumVM.Value = oneEnum.ToString();
                enumVM.Id = GetEnumID((System.Enum)oneEnum);
                enumVM.Description = GetEnumDescription((System.Enum)oneEnum);

                enumVMList.Add(enumVM);
            }


            return enumVMList;
        }

        internal List<MenuPart> TryGetMenus()
        {
            var queryMenu = _context.MenuParts.Select(x => x);
            List <MenuPart> menus = queryMenu.ToList();

            return menus;
        }

        internal CustomResponse TryGenDefaultPermission()
        {
            Guid sedziaId = Guid.NewGuid();
            Guid instruktorId = Guid.NewGuid();
            Guid prowadzacyId = Guid.NewGuid();

            List<AvaliblePermissions> permissions = new List<AvaliblePermissions>()
            {
                new AvaliblePermissions(){Id =sedziaId, Name = "Sędzia", HasEnumDetail = true},
                new AvaliblePermissions(){Id = instruktorId, Name = "Instruktor", HasEnumDetail = true},
                new AvaliblePermissions(){Id = prowadzacyId, Name = "Prowadzący", HasEnumDetail = true},
            };


            List<AvaliblePermissionDetail> permissionsDetail = new List<AvaliblePermissionDetail>()
            {
                new AvaliblePermissionDetail(){Id = Guid.NewGuid(), PermissionId = sedziaId, Name = "Klasa - I"},
                new AvaliblePermissionDetail(){Id = Guid.NewGuid(), PermissionId = sedziaId, Name = "Klasa - II"},
                new AvaliblePermissionDetail(){Id = Guid.NewGuid(), PermissionId = sedziaId, Name = "Klasa - III"},
                new AvaliblePermissionDetail(){Id = Guid.NewGuid(), PermissionId = instruktorId, Name = "Sportowy"},
                new AvaliblePermissionDetail(){Id = Guid.NewGuid(), PermissionId = prowadzacyId, Name = "A - Pneumatyczna"},
                new AvaliblePermissionDetail(){Id = Guid.NewGuid(), PermissionId = prowadzacyId, Name = "B - Boczny zapłon"},
                new AvaliblePermissionDetail(){Id = Guid.NewGuid(), PermissionId = prowadzacyId, Name = "C - Centralny zapłon"},
                new AvaliblePermissionDetail(){Id = Guid.NewGuid(), PermissionId = prowadzacyId, Name = "D - Maszynowa (pistolet)"},
                new AvaliblePermissionDetail(){Id = Guid.NewGuid(), PermissionId = prowadzacyId, Name = "E - Samoczynna"},
                new AvaliblePermissionDetail(){Id = Guid.NewGuid(), PermissionId = prowadzacyId, Name = "F - Gładkolufowa (powtarzalna)"},
                new AvaliblePermissionDetail(){Id = Guid.NewGuid(), PermissionId = prowadzacyId, Name = "G - Gładkolufowa"},
                new AvaliblePermissionDetail(){Id = Guid.NewGuid(), PermissionId = prowadzacyId, Name = "H - Czarnoprochowa"}
            };

            foreach (AvaliblePermissions onePermission in permissions)
            {
                var permissionDB = _context.AvaliblePermissions.FirstOrDefault(x => x.Name == onePermission.Name);
                if (permissionDB == null)
                {
                    _context.Add(onePermission);
                }
                else
                {
                    foreach (AvaliblePermissionDetail oneAVD in permissionsDetail)
                    {
                        if(oneAVD.PermissionId == onePermission.Id)
                        {
                            oneAVD.PermissionId = permissionDB.Id;
                        }
                    }

                    onePermission.Id = permissionDB.Id;
                }
            }

            foreach(AvaliblePermissionDetail oneAVD in permissionsDetail)
            {
                var permissionDetailDB = _context.AvaliblePermissionDetail.FirstOrDefault(x => x.Name == oneAVD.Name);
                if (permissionDetailDB == null)
                {
                    _context.Add(oneAVD);
                }
            }


            return new CustomResponse() { Status = true };
        }
    }
}
