﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo
{
    public class Fotografia
    {
        private int id_foto;
        private int id_dpto;
        private string path_img;
        private string alt;

        public int Id_foto { get => id_foto; set => id_foto = value; }
        public int Id_dpto { get => id_dpto; set => id_dpto = value; }
        public string Path_img { get => path_img; set => path_img = value; }
        public string Alt { get => alt; set => alt = value; }
    }
}
