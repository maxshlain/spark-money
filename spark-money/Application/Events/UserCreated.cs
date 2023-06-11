﻿using spark_money.Application.Models;
using Coravel.Events.Interfaces;

namespace spark_money.Application.Events
{

    public class UserCreated : IEvent
    {
        public User User { get; set; }

        public UserCreated(User user)
        {
            this.User = user;
        }
    }
}
