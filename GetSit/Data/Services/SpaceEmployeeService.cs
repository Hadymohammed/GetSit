﻿using GetSit.Data.Base;
using GetSit.Models;

namespace GetSit.Data.Services
{
    public class SpaceEmployeeService:EntityBaseRepository<SpaceEmployee>,ISpaceEmployeeService
    {
        private readonly AppDBcontext _context;
        public SpaceEmployeeService(AppDBcontext context):base(context)
        {
            _context = context;
        }

        public SpaceEmployee? GetByEmail(string email)
        {
            return _context.SpaceEmployee.Where(e => e.Email == email).FirstOrDefault();
        }
    }
}
