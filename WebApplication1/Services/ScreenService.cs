using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using OnePixelBE.EF;
using OnePixelBE.HubConfig;
using OnePixelBE.Models;
using OnePixelBE.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnePixelBE.Services
{
    public class ScreenService
    {
        List<string> listOfColors = new List<string>() {"#000000", "#800000", "#008000", "#808000", "#000080", "#800080", "#008080", "#c0c0c0", "#808080", "#ff0000", "#00ff00", "#ffff00", "#0000ff", "#ff00ff", "#00ffff", "#ffffff"};

        readonly AppDbContext _context;
        private IMapper _mapper;
        private readonly IHubContext<SignalHub> _ctx;
        public ScreenService(IMapper mapper, AppDbContext context, IHubContext<SignalHub> ctx)
        {
            _mapper = mapper;
            _context = context;
            _ctx = ctx;
        }

        internal CustomResponse TryToGetColors()
        {
            CustomResponse response = new CustomResponse() { Status = true, Data = listOfColors };

            return response;
        }

        internal CustomResponse TryToGetScreen()
        {
            List<FieldModel> fieldModels = _context.FieldModels.ToList();
            List<FieldViewModel> fieldViewModels = _mapper.Map<List<FieldViewModel>>(fieldModels);

            var maxY = fieldViewModels.Max(x => x.CoordinateY);
            var maxX = fieldViewModels.Max(x => x.CoordinateX);

            FieldResponseVM fieldResponseVM = new FieldResponseVM() { fieldVM = fieldViewModels, MaxX = maxX, MaxY = maxY };

            CustomResponse response = new CustomResponse() { Status = true, Data = fieldResponseVM };

            return response;

        }

        internal CustomResponse TryToInitScreen(FieldViewModel[][] screen)
        {
            List<FieldModel> fieldModelList = new List<FieldModel>() { };

            foreach (FieldViewModel[] row in screen)
            {
                foreach(FieldViewModel field in row)
                {
                    if(field.Id == null)
                    {
                        field.Id = Guid.NewGuid();
                    }
                    FieldModel oneFieldModel = _mapper.Map<FieldModel>(field);
                    fieldModelList.Add(oneFieldModel);
                }
            }

            var x = _context.FieldModels.ToList();
            _context.RemoveRange(x);
            _context.AddRange(fieldModelList);
            _context.SaveChanges();

            return new CustomResponse() { Status = true };

        }

        internal CustomResponse TryToUpdateOneField(FieldViewModel fieldVM)
        {
            if (!listOfColors.Contains(fieldVM.Color))
            {
                return new CustomResponse() { Status = false, Message="There is no such color" };
            }
            FieldModel fieldModel = _context.FieldModels.FirstOrDefault(x => x.Id == fieldVM.Id);
            fieldModel.Color = fieldVM.Color;
            _context.SaveChanges();

            return new CustomResponse() { Status = true, Message = "Update success" };
        }

        internal async Task<CustomResponse> TryToUpdateOneFieldAsync(FieldViewModel fieldVM)
        {
            if (!listOfColors.Contains(fieldVM.Color))
            {
                return new CustomResponse() { Status = false, Message = "There is no such color" };
            }
            FieldModel fieldModel = _context.FieldModels.FirstOrDefault(x => x.Id == fieldVM.Id);
            fieldModel.Color = fieldVM.Color;
            _context.SaveChanges();

            await _ctx.Clients.All.SendAsync("Send", fieldVM);
            return new CustomResponse() { Status = true, Message = "Update success" };
        }
    }
}
