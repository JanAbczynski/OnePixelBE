using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using OnePixelBE.CommonStuff;
using OnePixelBE.EF;
using OnePixelBE.Models;
using OnePixelBE.ViewModel;

namespace OnePixelBE.Services
{
    public class ShootingRangeService
    {
        readonly AppDbContext _context;
        private IMapper _mapper;
        private CommonService _commonService;
        public ShootingRangeService(IMapper mapper, AppDbContext context, CommonService commonService)
        {
            _mapper = mapper;
            _context = context;
            _commonService = commonService;
        }

        public string GeneralFilePath = "E:\\ProjektyVS\\ProjWithSidebar\\ProjWithSideBar\\src\\";
        public string GeneralFilePath2 = "E:\\ProjektyVS\\ProjWithSidebar\\ProjWithSideBar\\src\\assets\\graph\\targets";

        internal void TryAddShootingRange(ShootingRangeViewModel shootingRangeVM)
        {
            shootingRangeVM.Id = Guid.NewGuid();
            shootingRangeVM.Active = true;
            shootingRangeVM.IsPublic = false;
            foreach (OneRangeViewModel OneOneRangeVM in shootingRangeVM.OneRange)
            {
                OneOneRangeVM.Active = true;
                OneOneRangeVM.Id = Guid.NewGuid();
                OneOneRangeVM.GunsAsJson = JsonSerializer.Serialize(OneOneRangeVM.Guns);

            }
            ShootingRange shootingRange = _mapper.Map<ShootingRange>(shootingRangeVM);

            _context.Add(shootingRange);
        }

        internal CustomResponse TryGetShootingRangeData(Guid id)
        {
            ShootingRange shootingRange = _context.ShootingRanges
                .Include(a => a.Address)
                .Include(o => o.OneRange.Where(or => or.IsDeleted != true).OrderBy(or => or.Description))
                .FirstOrDefault(x => x.Id == id);


            if (shootingRange == null)
            {
                return new CustomResponse() { Status = false, Message = "Strzelnica nie została znaleziona" };
            }
            else
            {

                ShootingRangeViewModel shootingRangeViewModel = _mapper.Map<ShootingRangeViewModel>(shootingRange);
                shootingRangeViewModel.IsEditable = !CheckIfCouldEditSR(shootingRange).Status;



                foreach (OneRangeViewModel oneRange in shootingRangeViewModel.OneRange)
                {
                    oneRange.Guns = JsonSerializer.Deserialize<List<string>>(oneRange.GunsAsJson);
                    oneRange.GunsFriendly = GiveGunsFriendly(oneRange.Guns);
                }
                return new CustomResponse() { Status = true, Data = shootingRangeViewModel };
            }
        }

        private string GiveGunsFriendly(List<string> guns)
        {
            string gunsFriendly = "";
            if(guns == null)
            {
                return gunsFriendly;
            }

            List<string> description = new List<string>() { };

            foreach (var gun in guns)
            {
                Guns gunEnum = (Guns)_commonService.FindGunEnumByString<Guns>(gun);
                EnumViewModel gunDetail = _commonService.ReadEnumData(gunEnum);
                description.Add(gunDetail.Description);
            }

                int counter = 1;
            foreach(var gun in description.OrderBy(x => x))
            {
                gunsFriendly = gunsFriendly + gun;
                if(counter != guns.Count())
                {
                    gunsFriendly = gunsFriendly + ", ";
                }
            }
            return gunsFriendly;
        }



        internal CustomResponse TryEditShootingRange(ShootingRangeViewModel shootingRange)
        {
            ShootingRange shootingRangeModelDB = _context.ShootingRanges
                .Include(a => a.Address)
                .Include(o => o.OneRange)
                .FirstOrDefault(x => x.Id == shootingRange.Id);


            foreach (OneRangeViewModel oneRange in shootingRange.OneRange)
            {
                oneRange.GunsAsJson = JsonSerializer.Serialize(oneRange.Guns);
            }

            CustomResponse SRResponse = CheckIfCouldEditSR(shootingRangeModelDB);

            if (!SRResponse.Status)
            {
                return SRResponse;
            }

            ShootingRange shootingRangeModelNew = _mapper.Map<ShootingRange>(shootingRange);

            if (shootingRange == null)
            {
                return new CustomResponse() { Status = false, Message = "Błąd" };
            }

            shootingRangeModelDB.Name = shootingRangeModelNew.Name;
            shootingRangeModelDB.Description = shootingRangeModelNew.Description;

            shootingRangeModelDB.Address.Country = shootingRangeModelNew.Address.Country;
            shootingRangeModelDB.Address.ZipCode = shootingRangeModelNew.Address.ZipCode;
            shootingRangeModelDB.Address.City = shootingRangeModelNew.Address.City;
            shootingRangeModelDB.Address.Street = shootingRangeModelNew.Address.Street;
            shootingRangeModelDB.Address.BuildingNumber = shootingRangeModelNew.Address.BuildingNumber;
            shootingRangeModelDB.Address.LocalNumber = shootingRangeModelNew.Address.LocalNumber;

            shootingRangeModelDB.OneRange = DeleteFromList(shootingRangeModelDB.OneRange, shootingRangeModelNew.OneRange);
            shootingRangeModelDB.OneRange = EditExisiting(shootingRangeModelDB.OneRange, shootingRangeModelNew.OneRange);

            AddToList(shootingRangeModelDB, shootingRangeModelNew.OneRange);


            ShootingRangeViewModel newShootingRangeViewModel = _mapper.Map<ShootingRangeViewModel>(shootingRangeModelDB);
            return new CustomResponse() { Status = true, Data = newShootingRangeViewModel, Message = "Zapisano zmiany" };
        }

        private CustomResponse CheckIfCouldEditSR(ShootingRange shootingRangeModelDB)
        {
            if (shootingRangeModelDB.IsPublic)
            {
                return new CustomResponse() { Status = false, Message = "Strzelnica stanowi dobro ogólnodostępne", Message_2 = "Nie można dokonać zmiany" };
            }
            return new CustomResponse() { Status = true };
        }

        internal CustomResponse TrySubmitTarget(TargetViewModel targetViewModel, User user)
        {

            Target targetModel = CustomTargetMapping(targetViewModel, user);


            _context.Add(targetModel);

            CustomResponse response = new CustomResponse() { Status = true };
            return response;
        }

        private Target CustomTargetMapping(TargetViewModel targetViewModel, User user)
        {
            Target targetModel = _mapper.Map<Target>(targetViewModel);
            targetModel.SizeListAsJSON = JsonSerializer.Serialize(targetViewModel.Size);
            targetViewModel.Points = AddPoint(targetViewModel.Points);
            var pointsIds = targetViewModel.Points.Select(x => x.Id);
            targetModel.PointsIdList = JsonSerializer.Serialize(targetViewModel.Points);
            targetModel.OwnerId = user.Id;
            targetModel.AttachmentFileId = targetModel.AttachmentFile?.Id;
            targetModel.AttachmentFile = null;


            return targetModel;
        }

        internal CustomResponse TrySetActive(User user, Guid Id)
        {
            Target target = _context.Targets.FirstOrDefault(x => x.Id == Id && (x.OwnerId == user.Id || x.OwnerId == Guid.Empty));
            if (target == null)
            {
                return new CustomResponse() { Status = false, Message = "Nie odnaleziono tarczy" };
            }

            if (target.OwnerId == Guid.Empty)
            {
                return new CustomResponse() { Status = false, Message = "Nie można tego uczynić gdyż jest to cel publiczny"};
            }

            target.IsActive = !target.IsActive;

            if (target.IsActive)
            {
                return new CustomResponse() { Status = true, Message = "Tarcza aktywna", Message_2 = "Cel będzie widoczny i dostępny" };
            }
            else
            {
                return new CustomResponse() { Status = true, Message = "Tarcza nieaktywna", Message_2 = "Cel nie będzie widoczny i dostępny" };
            }

        }

        internal CustomResponse TryGetTargets(User user)
        {
            var x = _context.Targets.Include(x => x.AttachmentFile).Where(x => x.OwnerId == user.Id || x.OwnerId == Guid.Empty);
            List<Target> targets = x.ToList();

            List<TargetViewModel> targetsVM = _mapper.Map<List<TargetViewModel>>(targets);

            foreach(TargetViewModel targetVM in targetsVM)
            {
                targetVM.Points = JsonSerializer.Deserialize<List<PointsViewModel>>(targetVM.PointsIdList);
                targetVM.Size = JsonSerializer.Deserialize<List<SizeViewModel>>(targetVM.SizeListAsJSON);
                targetVM.FriendlyPoints = "";

                targetVM.IsPublic = targetVM.OwnerId == Guid.Empty;
                

                foreach(var point in targetVM.Points)
                {
                    if(targetVM.FriendlyPoints == "")
                    {
                        if (point.Special)
                        {
                            targetVM.FriendlyPoints = $"{point.Value} X";
                        }
                        else
                        {
                            targetVM.FriendlyPoints = $"{point.Value}";
                        }
                        
                    }
                    else
                    {
                        if (point.Special)
                        {
                            targetVM.FriendlyPoints = $"{point.Value} X, {targetVM.FriendlyPoints}";
                        }
                        else
                        {
                            targetVM.FriendlyPoints = $"{point.Value}, {targetVM.FriendlyPoints}";
                        }
                    }
                    
                }
            }
            CustomResponse response = new CustomResponse() { Status = true, Data = targetsVM };
            return response;
        }

        internal CustomResponse TryDeleteTarget(Guid id, User user)
        {
            Target targetDB = _context.Targets.FirstOrDefault(x => x.Id == id);

            if (Guid.Empty == targetDB.OwnerId)
            {
                return new CustomResponse() { Status = false, Message = "Nie można tego uczynić gdyż jest to cel publiczny" };
            }

            if (user.Id != targetDB.OwnerId)
            {
                return new CustomResponse() { Status = false, Message = "Nie jesteś właścicielem tarczy" };
            }


            if (!CheckIfAllowToChangeTarget(id))
            {
                return new CustomResponse() { Status = false, Message = "Nie można edytować celu" };
            }
            var fileId = (Guid?) targetDB.AttachmentFileId ?? null;

            _context.Targets.Remove(targetDB);
            List<Guid> except = new List<Guid>() { };
            except.Add(id);
            if (CheckIfAllowToChangeTargetFile(fileId, except))
            {
                DeleteFile(fileId);
                var fileDB = _context.Targets.FirstOrDefault(x => x.Id == fileId);
                if(fileDB != null)
                {
                    _context.Targets.Remove(fileDB);
                }
            }


            return new CustomResponse() { Status = true, Message = "Tarcza została usunięta" };

        }

        private void DeleteFile(Guid? fileId)
        {
            WebFile file = _context.WebFile.FirstOrDefault(x => x.Id == fileId);

            var path = GeneralFilePath + "assets/graph/targets/";

            var newFileName = file.NewName;
            var extension = file.Extension;
            string filePath = Path.Combine(path, $"{newFileName}{extension}");

            if (File.Exists(filePath))
            {
                // If file found, delete it    
                File.Delete(filePath);
                Console.WriteLine("File deleted.");
            }


        }

        private bool CheckIfAllowToChangeTargetFile(Guid? fileId, List<Guid> except)
        {
            var res = _context.Targets.Where(x => !except.Contains(x.Id) && x.AttachmentFileId == fileId).ToList();
            if(res.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        internal CustomResponse TryEditTarget(TargetViewModel targetViewModel, User user)
        {



            CustomResponse response = new CustomResponse() { };
            Target dbTarget = _context.Targets.FirstOrDefault(x => x.Id == targetViewModel.Id);

            if (Guid.Empty == dbTarget.OwnerId)
            {
                return new CustomResponse() { Status = false, Message = "Nie można tego uczynić gdyż jest to cel publiczny" };
            }

            TargetViewModel dbTargetViewModel = _mapper.Map<TargetViewModel>(dbTarget);
            dbTargetViewModel.Points = JsonSerializer.Deserialize<List<PointsViewModel>>(dbTargetViewModel.PointsIdList);

            response = ComparePoints(dbTargetViewModel.Points, targetViewModel.Points);
            if (!response.Status)
            {
                return response;
            }


            Target newTarget = CustomTargetMapping(targetViewModel, user);



            dbTarget.Name = newTarget.Name;
            dbTarget.SizeListAsJSON = newTarget.SizeListAsJSON;
            dbTarget.PointsIdList = newTarget.PointsIdList;
            dbTarget.AttachmentFile = newTarget.AttachmentFile;
            dbTarget.AttachmentFileId = newTarget.AttachmentFileId;


            response = new CustomResponse() { Status = true };

            return response;
        }

        internal CustomResponse TryCopyTarget(Guid id, User user)
        {

            CustomResponse targetResponse =  TryGetTarget(id, user);
            TargetViewModel oldTargetVM = new TargetViewModel() { };
            if (targetResponse.Status)
            {
                oldTargetVM = (TargetViewModel) targetResponse.Data;
            }
            else
            {
                return new CustomResponse() { Status = false, Message = "Nie odnaleziono celu" };
            }

            oldTargetVM.Name = oldTargetVM.Name + " - kopia";
            Guid newFileName = CopyFile(oldTargetVM.AttachmentFile);

            WebFile webFile = new WebFile() { };
            webFile.Id = Guid.NewGuid();
            webFile.OriginalName = oldTargetVM.AttachmentFile.OriginalName;
            webFile.NewName = newFileName.ToString();
            webFile.Folder = "assets/graph/targets/";
            webFile.Extension = oldTargetVM.AttachmentFile.Extension;
            webFile.FileStatus = FileStatus.temp;
            webFile.CreateDateUTC = DateTime.UtcNow;
            webFile.isTemp = true;

            _context.WebFile.Add(webFile);

            oldTargetVM.AttachmentFile = webFile;
            oldTargetVM.Id = new Guid();

            CustomResponse response = TrySubmitTarget(oldTargetVM, user);

            return response;
        }

        private Guid CopyFile(WebFile attachmentFile)
        {
            Guid newFileName = Guid.NewGuid();

            var oldFile = $"{GeneralFilePath}{attachmentFile.Folder}{attachmentFile.NewName}{attachmentFile.Extension}";
            var newFile = $"{GeneralFilePath}{attachmentFile.Folder}{newFileName}{attachmentFile.Extension}";

            
            File.Copy(oldFile, newFile);

            return newFileName;
        }

        private CustomResponse ComparePoints(List<PointsViewModel> oldPoints, List<PointsViewModel> newPoints)
        {
            oldPoints = CheckIfAllowRemovePoint(oldPoints);

            foreach(var point in oldPoints)
            {
                if (!point.AllowToRemove)
                {
                    var xx = newPoints.FirstOrDefault(x => x.Value == point.Value && x.Special == point.Special);
                    if(xx == null)
                    {
                        return new CustomResponse() { Status = false, Message = "Dokonano niedozolonej próby usunięcia punktu" };
                    }
                }
            }
            return new CustomResponse() { Status = true};
        }

        internal CustomResponse TryGetTarget(Guid id, User data)
        {
            Target target = _context.Targets.Include(x => x.AttachmentFile).FirstOrDefault(x => x.Id == id);
            TargetViewModel targetVM = _mapper.Map<TargetViewModel>(target);
            targetVM.Points = JsonSerializer.Deserialize<List<PointsViewModel>>(targetVM.PointsIdList);
            targetVM.Size = JsonSerializer.Deserialize<List<SizeViewModel>>(targetVM.SizeListAsJSON);
            targetVM.IsPublic = targetVM.OwnerId == Guid.Empty;

            targetVM.Points = CheckIfAllowRemovePoint(targetVM.Points);
            targetVM.AllowToChange = CheckIfAllowToChangeTarget(targetVM.Id);

            CustomResponse response = new CustomResponse { Status = true, Data = targetVM };
            return response;
        }


        private List<PointsViewModel> CheckIfAllowRemovePoint(List<PointsViewModel> points)
        {
            foreach (var point in points)
            {
                point.AllowToRemove = CheckIfAllowToRemoveOnePoint(point);
            }
            return points;
        }


        private bool CheckIfAllowToRemoveOnePoint(PointsViewModel point)
        {
            if (point.Value == 5)
            {
                return false;
            }
            return true;
        }

        private bool CheckIfAllowToChangeTarget(Guid id)
        {
            return true;
        }

        private List<PointsViewModel> AddPoint(List<PointsViewModel> points)
        {
            foreach(var point in points)
            {
                var existingPoint = _context.Points.FirstOrDefault(x => x.Value == point.Value && x.Special == point.Special);
                if(existingPoint != null)
                {
                    point.Id = existingPoint.Id;
                }
                else
                {
                    PointModel pointModel = _mapper.Map<PointModel>(point);
                    _context.Add(pointModel);
                }
            }

            return points;
        }

        internal CustomResponse TryGetCrewStands(User user)
        {
            List<CrewStand> crewStands = _context.CrewStands.Where(x => x.OwnerId == user.Id && x.IsDeleted == false).OrderBy(xx => xx.Name).ToList();
            List<CrewStandViewModel> crewStandsVM = _mapper.Map<List<CrewStandViewModel>>(crewStands);

            return new CustomResponse() { Status = true, Data = crewStandsVM };
        }

        internal CustomResponse TryGetCrewStand(Guid id)
        {
            throw new NotImplementedException();
        }

        internal CustomResponse UploadTarget(IFormFile file)
        {

            var path = GeneralFilePath + "assets/graph/targets/";
            var extension = Path.GetExtension(file.FileName);
            string newFileName = Guid.NewGuid().ToString();

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            WebFile webFile = new WebFile() { };
            webFile.Id = Guid.NewGuid();
            webFile.OriginalName = file.FileName;
            webFile.NewName = newFileName;
            webFile.Folder = "assets/graph/targets/";
            webFile.Extension = extension;
            webFile.FileStatus = FileStatus.temp;
            webFile.CreateDateUTC = DateTime.UtcNow;
            webFile.isTemp = true;


            string filePath = Path.Combine(path, $"{newFileName}{extension}");
            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            _context.Add(webFile);

            return new CustomResponse() { Status = true, Data = webFile };

        }

        internal CustomResponse TryAddCrewStand(CrewStandViewModel crewStand, User user, bool forceAdd = false)
        {
            crewStand.Id = Guid.NewGuid();
            crewStand.OwnerId = user.Id;

            CrewStand standModel = _mapper.Map<CrewStand>(crewStand);

            CrewStand standModelDB = _context.CrewStands.FirstOrDefault(x => x.Name == standModel.Name && x.OwnerId == standModel.OwnerId);
            if (standModelDB != null && false)
            {
                if (standModelDB.IsDeleted && forceAdd)
                {
                    _context.Add(standModel);
                    return new CustomResponse() { Status = true, Message = "Dodano wpis" };
                }
                else if(standModelDB.IsDeleted)
                {
                    return new CustomResponse() { Status = false, Message = $"Stanowisko \"{crewStand.Name}\" zostało usunięte. Czy chcesz je przywrócić?", ResponseCode = ResponseCode.conflict };
                }
                else
                {
                    return new CustomResponse() { Status = false, Message = $"Stanowisko \"{crewStand.Name}\" jest już na liście" };
                }
            }
            else
            {
                _context.Add(standModel);
                return new CustomResponse() { Status = true, Message = "Dodano wpis" };
            }

        }


        internal CustomResponse TryDeleteCrewStand(Guid id, User user)
        {
            CrewStand crewStand = _context.CrewStands.FirstOrDefault(x => x.Id == id && x.OwnerId == user.Id);
            if(crewStand == null)
            {
                return new CustomResponse() { Status = false, Message = "Nie odnaleziono wpisu." };
            }
            crewStand.IsDeleted = true;

            return new CustomResponse() { Status = true, Message = "Usunięto stanowisko." };
        }


        internal CustomResponse TryRemoveTempFile(Guid id)
        {
            var path = GeneralFilePath + "assets/graph/targets/";
            WebFile file = _context.WebFile.FirstOrDefault(x => x.Id == id && x.FileStatus == FileStatus.temp);

            if(file == null)
            {
                return new CustomResponse() { Status = false, Message = "Nie odnaleziono pliku" };
            }


            CustomResponse res = CheckIfCouldDeleteFile(file);
            if (!res.Status)
            {
                return res;
            }
            file.isDeleted = true;
            
            return new CustomResponse() { Status = false, Message = "Nie można usunąć pliku", Message_2 = "Plik jest używany przez inne wydarzenie" };
        }

        internal CustomResponse TryEditCrewStand(CrewStandViewModel crewStand, User user)
        {
            var crewStandDB = _context.CrewStands.FirstOrDefault(x => x.Id == crewStand.Id && x.OwnerId == user.Id);

            if(crewStand == null)
            {
                return new CustomResponse() { Status = false, Message = "Nie odnalezionow wpisu" };
            }

            crewStandDB.Name = crewStand.Name;

            return new CustomResponse() { Status = true, Message = "Dokonano zmiany" };
        }

        internal CustomResponse TryRestoreCrewStand(CrewStandViewModel crewStand, User user)
        {
            CrewStand crewStandDB = _context.CrewStands.FirstOrDefault(x => x.Name == crewStand.Name && x.OwnerId == user.Id && x.IsDeleted == true);
            if(crewStand == null)
            {
                return new CustomResponse() { Status = false, Message = "Nie udało się przywrócić wpisu" };
            }

            crewStandDB.IsDeleted = false;

            return new CustomResponse() { Status = true, Message = "Przywrócono wpis" };
        }

        private CustomResponse CheckIfCouldDeleteFile(WebFile file)
        {
            var target = _context.Targets.FirstOrDefault(x => x.AttachmentFileId == file.Id && x.OwnerId == Guid.Empty);
            if(target != null)
            {
                return new CustomResponse() { Status = false, Message = "Nie można usunąć pliku", Message_2 = "Plik jest używany przez tarczę publiczną", ResponseCode = ResponseCode.conflict };
            }
            //ToDo Sprawdzanie czy można usunąć  plik
            return new CustomResponse() { Status = true, };
        }

        internal CustomResponse TryGetOneRangeData(List<Guid> shootingRangesId)
        {
           List<ShootingRange> oneRanges =  _context.ShootingRanges
    .Include(o => o.OneRange)
    .Where(x => shootingRangesId.Contains(x.Id)).ToList();

            var y = "";

            List<OneRangeIndexViewModel> oneRangesViewModel = OneRangeManualMapping(oneRanges);
            return new CustomResponse() { Status = true, Data = oneRangesViewModel };
        }

        private List<OneRangeIndexViewModel> OneRangeManualMapping(List<ShootingRange> oneShootingRanges)
        {
            List<OneRangeIndexViewModel> oneRangesViewModel = new List<OneRangeIndexViewModel>() { };
            foreach (var oneShootingRange in oneShootingRanges)
            {
                foreach(var oneOneRange in oneShootingRange.OneRange)
                {
                    OneRangeIndexViewModel oneRangeVM = new OneRangeIndexViewModel() { };
                    oneRangeVM.Id = oneOneRange.Id;
                    oneRangeVM.ShootingRangeId = oneShootingRange.Id;
                    oneRangeVM.ShootingRangeName = oneShootingRange.Name;
                    oneRangeVM.Description = oneOneRange.Description;
                    oneRangeVM.GunsAsJson = oneOneRange.GunsAsJson;
                    oneRangeVM.NoOfTargets = oneOneRange.NoOfTargets;
                    oneRangeVM.Distance = oneOneRange.Distance;

                    oneRangesViewModel.Add(oneRangeVM);
                }
            }

            return oneRangesViewModel;
        }

        internal CustomResponse TryGetOneShootingRange(Guid id)
        {
            ShootingRange shootingRangeModelDB = _context.ShootingRanges
    .Include(a => a.Address)
    .Include(o => o.OneRange)
    .FirstOrDefault(x => x.Id == id);

            ShootingRangeViewModel shootingRangeIndexViewModel = _mapper.Map<ShootingRangeViewModel>(shootingRangeModelDB);

            CustomResponse response = new CustomResponse() { };
            response.Status = true;
            response.Data = shootingRangeIndexViewModel;

            return response;


        }

        internal CustomResponse TryDeleteShootingRange(Guid id)
        {
            ShootingRange shootingRange = _context.ShootingRanges.FirstOrDefault(x => x.Id == id);
            if(shootingRange == null)
            {
                return new CustomResponse() { Status = false, Message = "Nie odnaleziono strzelnicy" };
            }

            CustomResponse response = CheckIfCouldEditSR(shootingRange);
            if (!response.Status)
            {
                return response;
            }
            shootingRange.isDeleted = true;

            return new CustomResponse() { Status = true, Message = "Strzelnica została usunięta" };

        }

        private void AddToList(ShootingRange shootingRangeModelDB, List<OneRange> newList)
        {
            List<Guid> idExisting = new List<Guid>() { };
            foreach (var oneOneRange in shootingRangeModelDB.OneRange)
            {
                idExisting.Add(oneOneRange.Id);
            }

            foreach (var oneOneRange in newList)
            {
                if(oneOneRange.Id == Guid.Empty)
                {
                    oneOneRange.Id = Guid.NewGuid();
                }
            }


            foreach (var oneOneRange in newList)
            {
                if (!idExisting.Contains(oneOneRange.Id))
                {
                    OneRange newOneRange = newList.FirstOrDefault(x => x.Id == oneOneRange.Id);
                    newOneRange.ShootingRangeId = shootingRangeModelDB.Id;
                    _context.Add(newOneRange);

                }
            }

        }

        private List<OneRange> EditExisiting(List<OneRange> primaryList, List<OneRange> newList)
        {
            List<Guid> idToEdit = new List<Guid>() { };
            foreach (var oneOneRange in newList)
            {
                idToEdit.Add(oneOneRange.Id);
            }

            foreach (var oneOneRange in primaryList)
            {
                if (idToEdit.Contains(oneOneRange.Id))
                {
                    OneRange newOneRange = newList.FirstOrDefault(x => x.Id == oneOneRange.Id);

                    oneOneRange.Description = newOneRange.Description;
                    oneOneRange.Distance = newOneRange.Distance;
                    oneOneRange.GunsAsJson = newOneRange.GunsAsJson;
                    oneOneRange.NoOfTargets = newOneRange.NoOfTargets;

                }
            }

            return primaryList;
        }

        private List<OneRange> DeleteFromList(List<OneRange> primaryList, List<OneRange> newList)
        {
            List<Guid> idToKeep = new List<Guid>() { };
            foreach (var oneOneRange in newList)
            {
                idToKeep.Add(oneOneRange.Id);
            }
            foreach (var oneOneRange in primaryList)
            {
                if (!idToKeep.Contains(oneOneRange.Id))
                {
                    oneOneRange.IsDeleted = true;
                }
            }
            return primaryList;
        }


        internal List<ShootingRangeIndexViewModel> TryGetShootingRanges()
        {
            var x = _context.ShootingRanges.Include(a => a.Address).Include(o => o.OneRange).Select(x => x).Where(a => a.isDeleted == false);

            List<ShootingRangeIndexViewModel> shootingRangeIndexList = new List<ShootingRangeIndexViewModel>() { };
            foreach(var oneShootingRange in x)
            {

                ShootingRangeIndexViewModel shootingRangeIndex = new ShootingRangeIndexViewModel() { };

                shootingRangeIndex = ShootingRangeManualMapping(oneShootingRange);

                shootingRangeIndexList.Add(shootingRangeIndex);
            }

            return shootingRangeIndexList;

        }

        private ShootingRangeIndexViewModel ShootingRangeManualMapping(ShootingRange oneShootingRange)
        {
            ShootingRangeIndexViewModel shootingRangeIndex = new ShootingRangeIndexViewModel() { };

            shootingRangeIndex.Id = oneShootingRange.Id;
            shootingRangeIndex.Name = oneShootingRange.Name;
            shootingRangeIndex.ZipCode = oneShootingRange.Address.ZipCode;
            shootingRangeIndex.City = oneShootingRange.Address.City;
            shootingRangeIndex.Country = oneShootingRange.Address.Country;
            shootingRangeIndex.IsPublic = oneShootingRange.IsPublic;
            shootingRangeIndex.OneRanges = GiveOneRangeFriendly(oneShootingRange.OneRange);

            return shootingRangeIndex;
        }

        private string GiveOneRangeFriendly(List<OneRange> oneRange)
        {
            string friendlyName = "";

            if(oneRange == null)
            {
                return friendlyName;
            }
            var sortedList = oneRange.OrderBy(x => x.Distance);
            int counter = 1;
            foreach(OneRange oneOneRange in sortedList)
            {
                friendlyName = friendlyName + oneOneRange.Distance.ToString();
                if(counter != sortedList.Count())
                {
                    friendlyName = friendlyName + ", ";
                }
                counter++;
            }

            return friendlyName;
        }
    }
}
