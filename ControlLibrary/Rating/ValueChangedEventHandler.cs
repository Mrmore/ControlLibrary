﻿//
// Copyright (c) 2012 Tim Heuer
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlLibrary
{
    public delegate void ValueChangedEventHandler<T>(object sender, ValueChangedEventArgs<T> e);
}
