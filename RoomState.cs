using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digital_Museum_of_Music_and_Artists
{
    [Serializable]

    public class RoomState
    {
        public bool IsLightOn { get; set; }
        public int Temperature { get; set; }
        public bool IsVideoPlaying { get; set; }

        public RoomState()
        {
            IsLightOn = false;
            Temperature = 25;
            IsVideoPlaying = false;
        }
    }
}
