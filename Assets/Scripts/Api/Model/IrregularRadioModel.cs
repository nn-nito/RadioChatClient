using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Api.Model
{
    public class IrregularRadioModel
    {
        [Serializable]
        public class IrregularRadio
        {
            public int id;
            public int radio_id;
            public string day_of_week;
            public string on_air_start_time;
            public string on_air_end_time;
        }

        [Serializable]
        public class IrregularRadios
        {
            public IrregularRadio[] irregularRadios;
        }
    }
}
