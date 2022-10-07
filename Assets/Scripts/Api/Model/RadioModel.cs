using System;
using System.Collections;
using System.Collections.Generic;

namespace Api
{
    public class RadioModel
    {
        [Serializable]
        public class Radio
        {
            public int id;
            public int room_id;
            public int same_Id;
            public string title;
            public string title_kana;
            public string body;
            public int day_of_week;
            public int radio_station_id;
            public string on_air_start_time;
            public string on_air_end_time;
            public bool is_main_air;
            public int display_order;
            public string performer;
            public bool is_irregular;
        }

        [Serializable]
        public class Radios
        {
            public Radio[] radios;
        }
    }
}
