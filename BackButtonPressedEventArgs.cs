using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digital_Museum_of_Music_and_Artists
{
    public class BackButtonPressedEventArgs : EventArgs
    {
        public RoomState UpdatedRoomState { get; }

        public BackButtonPressedEventArgs(RoomState updatedRoomState)
        {
            UpdatedRoomState = updatedRoomState;
        }
    }
}
