using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEatPizzaSocialNetwork.Domain.Core.Entities
{
    public class Form
    {
        public int Id { get; set; }

        public int CountSentForm { get; set; }

        public DateTime LastDateAndTimeSentForm { get; set; }

        public int UserId { get; set; }
    }
}
