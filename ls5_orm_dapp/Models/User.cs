﻿namespace ls5_orm_dapp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<Order> Orders {  get; set; }
    }
}
