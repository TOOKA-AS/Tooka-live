using System;
namespace Live2k.Core.Model.Base
{
    public class UserSignature
    {
        private UserSignature()
        {

        }

        public UserSignature(User user)
        {
            UserId = user.Id;
            FullName = user.FullName;
            SignedOn = DateTime.Now;
        }

        /// <summary>
        /// User Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// User full name
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Date of signature
        /// </summary>
        public DateTime SignedOn { get; set; }
    }
}
