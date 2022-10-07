using System;
using System.Collections;
using System.Collections.Generic;

namespace Api.Model
{
    [Serializable]
    public class RadioWithUserFavoriteModel
    {
        public RadioModel.Radio[] radios;
        public UserFavoriteRadioModel.UserFavoriteRadio[] user_favorite_radios;
        public IrregularRadioModel.IrregularRadio[] irregular_radios;
    }
}
