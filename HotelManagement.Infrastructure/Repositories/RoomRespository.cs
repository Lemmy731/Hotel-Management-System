﻿using HotelManagement.Core.Domains;
using HotelManagement.Core.IRepositories;
using HotelManagement.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Infrastructure.Repositories
{
    public class RoomRespository: GenericRepository<Room>,IRoomRepository
    {
        

        public RoomRespository(HotelDbContext hotelDbContext):base(hotelDbContext)
        {
            
        }

        public async void Add(string Roomtype_ID, string Hotel_Name, Room room)
        {
            room.RoomTypeId = Roomtype_ID;
            await AddAsync(room);
        }
    }
}
