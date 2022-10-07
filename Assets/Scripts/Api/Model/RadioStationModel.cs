using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Api
{
    public class RadioStationModel
    {
        [Serializable]
        public class RadioStation
        {
            public int id;
            public string name;
        }

        [Serializable]
        public class RadioStations
        {
            public RadioStation[] radio_stations;
        }
    }
}
