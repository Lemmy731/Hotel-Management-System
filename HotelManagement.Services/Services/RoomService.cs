﻿using AutoMapper;
using HotelManagement.Core;
using HotelManagement.Core.Domains;
using HotelManagement.Core.DTOs;
using HotelManagement.Core.IRepositories;
using HotelManagement.Core.IServices;
using HotelManagement.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Services.Services
{
    public class RoomService:IRoomService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly HotelDbContext _hotelDbContext;

        public RoomService(IUnitOfWork unitOfWork, IMapper mapper, HotelDbContext hotelDbContext)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hotelDbContext = hotelDbContext;
        }

        public async Task<Response<string>> AddRoom(string RoomType_ID, string Hotel_Name, AddRoomDto addRoomDto)
        {
            var hotel = await _unitOfWork.hotelRepository.GetByIdAsync(x => x.Name == Hotel_Name);
            if (hotel == null)
                return new Response<string>
                {
                    Data = Hotel_Name,
                    Succeeded = false,
                    StatusCode = 404,
                    Message = "Hotel Name Not found"
                };
            var roomtype = _hotelDbContext.RoomTypes.Where(x => x.HotelId == hotel.Id);
            var newroom = _mapper.Map<Room>(addRoomDto);
            if (newroom == null) return Response<string>.Fail("Operation Not Successful");
                await _unitOfWork.roomRepository.AddAsync(newroom);
            return Response<string>.Success("Room Created Successfully", newroom.RoomNo);
            
        }

        public async Task<Response<GetRoomDto>> GetRoombyId(string Id)
        {
            try
            {
                var room = await _unitOfWork.roomRepository.GetByIdAsync(x => x.Id == Id);
                var data = _mapper.Map<GetRoomDto>(room);
                if (data == null) return Response<GetRoomDto>.Fail("No Room Found");
                return Response<GetRoomDto>.Success(Id, data);
            }
            catch (Exception ex)
            {

                return Response<GetRoomDto>.Fail(ex.Message);
            }
        }
    }
}
