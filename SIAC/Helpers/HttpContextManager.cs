﻿using System;
using System.Web;

namespace SIAC
{
    public class HttpContextManager
    {
        private static HttpContextBase _context;

        public static HttpContextBase Current
        {
            get
            {
                if (_context != null)
                    return _context;

                if (HttpContext.Current == null)
                    return null;

                return new HttpContextWrapper(HttpContext.Current);
            }
        }

        public static void SetCurrentContext(HttpContextBase context)
        {
            _context = context;
        }
    }
}