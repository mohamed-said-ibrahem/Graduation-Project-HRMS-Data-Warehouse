using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR_System.Models
{
    public class Client
    {
        private int clientId;
        private string name;
        private string email;
        private string address;
        private string phoneNumber;

        public int ClientId { get => clientId; set => clientId = value; }
        public string Name { get => name; set => name = value; }
        public string Email { get => email; set => email = value; }
        public string Address { get => address; set => address = value; }
        public string PhoneNumber { get => phoneNumber; set => phoneNumber = value; }

    }
}