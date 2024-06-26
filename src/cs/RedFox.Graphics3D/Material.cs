﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedFox.Graphics3D
{
    /// <summary>
    /// A class to hold a basic <see cref="Material"/>.
    /// </summary>
    public class Material : Graphics3DObject
    {
        /// <summary>
        /// Gets or Sets the textures assigned to this material.
        /// </summary>
        public Dictionary<string, Texture> Textures { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Material"/> class.
        /// </summary>
        /// <param name="name">Name of the material.</param>
        public Material(string name) : base(name)
        {
            Name = name;
            Textures = new();
        }
    }
}
