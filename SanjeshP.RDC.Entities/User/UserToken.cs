﻿using SanjeshP.RDC.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SanjeshP.RDC.Entities.User
{
    public class UserToken : BaseEntity
    {
        [Required(ErrorMessage = "شناسه کاربر الزامی است")]
        [Display(Name = "شناسه کاربر")]
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "شناسه جلسه الزامی است")]
        [MaxLength(128, ErrorMessage = "شناسه جلسه نباید بیش از 128 کاراکتر باشد")]
        [Display(Name = "شناسه جلسه")]
        public string SessionId { get; set; }

        [MaxLength(256, ErrorMessage = "Agent کاربر نباید بیش از 256 کاراکتر باشد")]
        [Display(Name = "Agent کاربر")]
        public string UserAgent { get; set; }

        [Required(ErrorMessage = "تاریخ انقضاء الزامی است")]
        [Display(Name = "تاریخ انقضاء")]
        public DateTime ExpirationDate { get; set; }

        [NotMapped]
        public Guid CreatedBy { get; set; }

        [Display(Name = "کاربر")]
        public virtual User User { get; set; }

        public virtual ICollection<UserVerificationCode> UserVerificationCodes { get; set; } = new List<UserVerificationCode>();

        
    }
}
