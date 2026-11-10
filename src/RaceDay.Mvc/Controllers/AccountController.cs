using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using RaceDay.Mvc.Models;
namespace RaceDay.Mvc.Controllers;
public class AccountController(IHttpClientFactory factory):Controller {
 private HttpClient Api()=>factory.CreateClient("RaceDayApi");
 private void AddCookie(HttpClient c){var cookie=HttpContext.Session.GetString("ApiCookie");if(!string.IsNullOrEmpty(cookie))c.DefaultRequestHeaders.Add("Cookie",cookie);}
 [HttpGet] public IActionResult Login()=>View();
 [HttpPost] public async Task<IActionResult> Login(LoginVm vm){using var c=Api();var r=await c.PostAsJsonAsync("api/auth/login",vm);if(!r.IsSuccessStatusCode){ViewBag.Error="Login failed. Check your details and confirm the API is running.";return View(vm);}if(r.Headers.TryGetValues("Set-Cookie",out var values)){var v=values.FirstOrDefault();if(v!=null)HttpContext.Session.SetString("ApiCookie",v.Split(';')[0]);}var data=await r.Content.ReadFromJsonAsync<LoginResponse>();if(data==null)return View(vm);HttpContext.Session.SetInt32("UserId",data.userId);HttpContext.Session.SetString("Role",data.role);HttpContext.Session.SetString("UserName",data.firstName+" "+data.lastName);return RedirectToAction("Index","Events");}
 [HttpGet] public IActionResult Register()=>View();
 [HttpPost] public async Task<IActionResult> Register(RegisterVm vm){using var c=Api();var r=await c.PostAsJsonAsync("api/auth/register",vm);if(!r.IsSuccessStatusCode){ViewBag.Error="Registration failed. Check the details or email address.";return View(vm);}TempData["Message"]="Account created. Please log in.";return RedirectToAction("Login");}
 public async Task<IActionResult> Logout(){using var c=Api();AddCookie(c);await c.PostAsync("api/auth/logout",null);HttpContext.Session.Clear();return RedirectToAction("Index","Home");}
 private record LoginResponse(int userId,string firstName,string lastName,string role);
}