using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using RaceDay.Mvc.Models;
namespace RaceDay.Mvc.Controllers;
public class EventsController(IHttpClientFactory factory):Controller {
 private HttpClient Api(){var c=factory.CreateClient("RaceDayApi");var cookie=HttpContext.Session.GetString("ApiCookie");if(!string.IsNullOrEmpty(cookie))c.DefaultRequestHeaders.Add("Cookie",cookie);return c;}
 private bool Logged=>HttpContext.Session.GetInt32("UserId")!=null;
 public async Task<IActionResult> Index(){using var c=Api();var data=await c.GetFromJsonAsync<List<EventVm>>("api/events")??[];return View(data);}
 public async Task<IActionResult> Details(int id){using var c=Api();var e=await c.GetFromJsonAsync<EventVm>($"api/events/{id}");if(e==null)return NotFound();ViewBag.Categories=await c.GetFromJsonAsync<List<CategoryVm>>($"api/categories?eventId={id}")??[];return View(e);}
 [HttpPost] public async Task<IActionResult> Enter(int eventId,int categoryId){if(!Logged)return RedirectToAction("Login","Account");using var c=Api();var r=await c.PostAsJsonAsync("api/enrolments",new EnrolVm(eventId,categoryId));TempData["Message"]=r.IsSuccessStatusCode?"Your event entry was submitted.":"Unable to enter this event. Check that you are logged in as a Participant and have not already entered.";return RedirectToAction("Details",new{id=eventId});}
 public async Task<IActionResult> Mine(){if(!Logged)return RedirectToAction("Login","Account");using var c=Api();var r=await c.GetAsync("api/enrolments/mine");ViewBag.Error=r.IsSuccessStatusCode?null:"Could not load enrolments. Please check your Participant access.";return View(r.IsSuccessStatusCode?await r.Content.ReadAsStringAsync():"[]");}
 public async Task<IActionResult> Results(){if(!Logged)return RedirectToAction("Login","Account");using var c=Api();var r=await c.GetAsync("api/results/mine");var list=r.IsSuccessStatusCode?await r.Content.ReadFromJsonAsync<List<ResultVm>>()??[]:[];return View(list);}
}