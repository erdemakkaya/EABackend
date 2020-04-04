﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EA.Application.Common.Enums
{
    /// <summary>
    /// Projemiz içerisinde kullanacağımız tüm durumları bu class içerisinde yazarak hepsini bir araya toplamış olacağız.
    /// Bu sayede bir durum ataması yaparken ya da kontrol gerçekleştirirken sadece bu enum'ı kullanmak yeterli olacak.
    /// </summary>
    public enum AppStatus
    {
        //Durumlarınızı nasıl tanımlamak isterseniz o şekilde bu kısma yazabilirsiniz ben örnek olması açısından 4 adet tanımladım
        Aktif = 1,
        OnayBekliyor = 2,
        AskiyaAlindi = 3,
        Silindi = 4
    }
}
