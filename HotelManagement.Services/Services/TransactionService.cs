﻿using AutoMapper;
using HotelManagement.Core;
using HotelManagement.Core.Domains;
using HotelManagement.Core.DTOs;
using HotelManagement.Core.IRepositories;
using HotelManagement.Core.IServices;
using HotelManagement.Infrastructure.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Services.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IMapper _mapper;
        private readonly ITransactionsRepository _transRepo;
        protected DbSet<Payment> _dbSet;
        private readonly IUnitOfWork _unitOfWork;

        public TransactionService(IMapper mapper, IUnitOfWork unitOfWork, ITransactionsRepository transRepo)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _transRepo = transRepo;
        }
        
        public async Task<Response<List<RoomTransactionDTO>>> GetRoomTransactionsByManger(string managerId)
        {

            var manager = await _transRepo.GetHotelManager(managerId);

                if (manager == null)
                {
                    return Response<List<RoomTransactionDTO>>.Fail("Manager not found.", 404);
                }

                // Create a list to store the room transaction DTOs
                var roomTransactionDtos = new List<RoomTransactionDTO>();

                // Loop through each hotel associated with the manager
                foreach (var hotel in manager.Hotels)
                {
                    // Loop through each room in the hotel
                    foreach (var roomtypes in hotel.RoomTypes)
                    {
                        // Find the booking for the current room (if it exists)
                        var booking = hotel.Bookings.FirstOrDefault(b => b.RoomTypeId == roomtypes.Id);

                        // Create a new room transaction DTO for the current room
                        var roomTransactionDto = new RoomTransactionDTO
                        {
                            HotelName = hotel.Name,
                            RoomType = roomtypes.Name,
                            Price = roomtypes.Price,
                            Discount = roomtypes.Discount,
                           
                        };

                        // If there is a booking for the current room, add the booking details to the room transaction DTO
                        if (booking != null)
                        {
                            roomTransactionDto.CheckInDate = booking.CheckIn;
                            roomTransactionDto.BookingReference = booking.BookingReference;
                            roomTransactionDto.PaymentStatus = booking.PaymentStatus;
                        }

                        // Add the room transaction DTO to the list
                        roomTransactionDtos.Add(roomTransactionDto);
                    }
                }

                return Response<List<RoomTransactionDTO>>.Success("Room transactions retrieved successfully.", roomTransactionDtos, 200);
            }

        public async Task<Response<List<RoomTransactionDTO>>> GetAllRoomsTransactions(string hotelId)
        {
            var hotel = await _transRepo.GetAllRoomsTransaction(hotelId);


            //check if tyhe hotelid is null
            if (hotel == null)
            {
                return Response<List<RoomTransactionDTO>>.Fail("Hotel not found.", 404);
            }
            // Create a list to store the roomType transaction DTOs
            var roomTransactionDtos = new List<RoomTransactionDTO>();
            // Loop through each room in the hotel
            foreach (var roomtype in hotel.RoomTypes)
            {
                

                // Find the booking for the current room (if it exists)
                var booking = hotel.Bookings.FirstOrDefault(b => b.RoomTypeId == roomtype.Id);
                // Create a new room transaction DTO for the current room
                var roomTransactionDto = new RoomTransactionDTO
                {
                    HotelName = hotel.Name,
                    RoomType = roomtype.Name,
                    Price = roomtype.Price,
                    Discount = roomtype.Discount,
                };


                // If there is a booking for the current room, add the booking details to the room transaction DTO
                if (booking != null)
                {
                    roomTransactionDto.CheckInDate = booking.CheckIn;
                    roomTransactionDto.BookingReference = booking.BookingReference;
                    roomTransactionDto.PaymentStatus = booking.PaymentStatus;
                }

                // Add the room transaction DTO to the list
                roomTransactionDtos.Add(roomTransactionDto);
            }

            return Response<List<RoomTransactionDTO>>.Success("Room transactions retrieved successfully.", roomTransactionDtos, 200);
        }

    }
}
    

