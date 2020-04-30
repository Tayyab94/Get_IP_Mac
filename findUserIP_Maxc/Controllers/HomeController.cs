using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using findUserIP_Maxc.Models;
using System.Net;
using System.Net.NetworkInformation;

namespace findUserIP_Maxc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());

            string IPAddress = Convert.ToString(ipHostInfo.AddressList.FirstOrDefault(address =>
                                   address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork));

            string MachineName = ipHostInfo.HostName;

            string msc = getMachAccdress();

            ViewBag.info = $"IP => {IPAddress} _ Host Name => {MachineName} _ Mac-Address  {msc}";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        // get Mac-Addres 
        public string getMachAccdress()
        {
            string mac_src = "";
            string mac_add = "";

            foreach (NetworkInterface net_interface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (net_interface.OperationalStatus == OperationalStatus.Up)
                {
                    mac_src += net_interface.GetPhysicalAddress().ToString();
                    break;
                }
            }
            while (mac_src.Length < 12)
            {
                mac_src = mac_src.Insert(0, "0");

            }
            for (int i = 0; i < 11; i++)
            {
                if (0 == (i % 2))
                {
                    if (i == 10)
                    {
                        mac_add = mac_add.Insert(mac_add.Length, mac_add.Substring(i, 2));
                    }
                    else
                    {
                        mac_add = mac_add.Insert(mac_add.Length, mac_src.Substring(i, 2)) + "-";
                    }
                }
            }
            return mac_add;
        }
    }
}
