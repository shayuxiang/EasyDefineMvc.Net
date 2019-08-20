using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EasyDefine.Net.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 快速上手
        /// </summary>
        /// <returns></returns>
        public IActionResult Fast()
        {
            return View();
        }

        public IActionResult Document()
        {
            return View();
        }

        /// <summary>
        /// 示例
        /// </summary>
        /// <returns></returns>
        public IActionResult Demo()
        {
            return View();
        }

        /// <summary>
        /// 关于
        /// </summary>
        /// <returns></returns>
        public IActionResult About() {
            return View();
        }
    }
}
