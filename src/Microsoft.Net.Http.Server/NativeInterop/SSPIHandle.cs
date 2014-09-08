// Copyright (c) Microsoft Open Technologies, Inc.
// All Rights Reserved
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// THIS CODE IS PROVIDED *AS IS* BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING
// WITHOUT LIMITATION ANY IMPLIED WARRANTIES OR CONDITIONS OF
// TITLE, FITNESS FOR A PARTICULAR PURPOSE, MERCHANTABLITY OR
// NON-INFRINGEMENT.
// See the Apache 2 License for the specific language governing
// permissions and limitations under the License.

// -----------------------------------------------------------------------
// <copyright file="SSPIHandle.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace Microsoft.Net.Http.Server
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct SSPIHandle
    {
        private IntPtr handleHi;
        private IntPtr handleLo;

        public bool IsZero
        {
            get { return handleHi == IntPtr.Zero && handleLo == IntPtr.Zero; }
        }

        internal void SetToInvalid()
        {
            handleHi = IntPtr.Zero;
            handleLo = IntPtr.Zero;
        }

        public override string ToString()
        {
            return handleHi.ToString("x") + ":" + handleLo.ToString("x");
        }
    }
}