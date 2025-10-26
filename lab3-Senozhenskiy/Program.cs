/*using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SciencePortalWebApp;
using SciencePortalWebApp.Data;
using SciencePortalWebApp.Infrastructure;
using SciencePortalWebApp.Models;
using SciencePortalWebApp.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --- 1. РЕГИСТРАЦИЯ СЕРВИСОВ ---

// Получаем строку подключения из appsettings.json
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Регистрируем DbContext
builder.Services.AddDbContext<SciencePortalDbContext>(options => options.UseSqlServer(connectionString));

// Регистрируем сервисы кэширования и сессий
builder.Services.AddMemoryCache();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

// Регистрируем наш универсальный сервис кэширования для каждой модели
builder.Services.AddScoped<ICachedService<Department>, CachedService<Department>>();
builder.Services.AddScoped<ICachedService<Teacher>, CachedService<Teacher>>();
builder.Services.AddScoped<ICachedService<Project>, CachedService<Project>>();
builder.Services.AddScoped<ICachedService<Publication>, CachedService<Publication>>();
builder.Services.AddScoped<ICachedService<JournalsConference>, CachedService<JournalsConference>>();
builder.Services.AddScoped<ICachedService<ScientificDirection>, CachedService<ScientificDirection>>();

var app = builder.Build();

// --- 2. НАСТРОЙКА КОНВЕЙЕРА ОБРАБОТКИ ЗАПРОСОВ ---

// Включаем поддержку сессий
app.UseSession();

// Middleware для /info
app.Map("/info", (HttpContext context) =>
{
    var html = new StringBuilder();
    html.Append("<!DOCTYPE html><html><head><meta charset='utf-8' /></head><body>");
    html.Append("<h1>Client Information</h1>");
    html.Append($"<p>Host: {context.Request.Host}</p>");
    html.Append($"<p>Path: {context.Request.Path}</p>");
    html.Append($"<p>Protocol: {context.Request.Protocol}</p>");
    html.Append("<br/><a href='/'>Home</a>");
    html.Append("</body></html>");
    return context.Response.WriteAsync(html.ToString());
});

// Middleware для /table/{tableName}
MapTable<Department>(app, "/departments", "Departments");
MapTable<Teacher>(app, "/teachers", "Teachers");
MapTable<Project>(app, "/projects", "Projects");
MapTable<Publication>(app, "/publications", "Publications");
MapTable<JournalsConference>(app, "/journals", "Journals");
MapTable<ScientificDirection>(app, "/directions", "Directions");

// Middleware для /searchform1 (куки)
app.Map("/searchform1", (HttpContext context) =>
{
    var formData = context.Session.Get<FormData>("form1_session_data") ?? new FormData();

    if (context.Request.Query.ContainsKey("searchText"))
    {
        formData.SearchText = context.Request.Query["searchText"];
        formData.Category = context.Request.Query["category"];
        formData.IsActive = context.Request.Query["isActive"];

        var cookieOptions = new CookieOptions { Expires = DateTimeOffset.Now.AddMinutes(10) };
        context.Response.Cookies.Append("form1_cookie_data", System.Text.Json.JsonSerializer.Serialize(formData), cookieOptions);
    }

    var html = BuildSearchFormHtml("/searchform1", formData);
    return context.Response.WriteAsync(html);
});

// Middleware для /searchform2 (сессия)
app.Map("/searchform2", (HttpContext context) =>
{
    var jsonFromCookie = context.Request.Cookies["form1_cookie_data"];
    var formData = !string.IsNullOrEmpty(jsonFromCookie)
        ? System.Text.Json.JsonSerializer.Deserialize<FormData>(jsonFromCookie)
        : new FormData();

    if (context.Request.Query.ContainsKey("searchText"))
    {
        formData.SearchText = context.Request.Query["searchText"];
        formData.Category = context.Request.Query["category"];
        formData.IsActive = context.Request.Query["isActive"];

        context.Session.Set("form1_session_data", formData);
    }

    var html = BuildSearchFormHtml("/searchform2", formData);
    return context.Response.WriteAsync(html);
});


// Middleware по умолчанию (главная страница и 404)
// Middleware по умолчанию (главная страница и 404)
app.Run(async (context) =>
{
    // Если запрос дошел до сюда, значит, ни один app.Map не сработал.
    // Поэтому сначала устанавливаем статус 404.
    context.Response.StatusCode = 404;

    // Если это запрос на главную страницу, то показываем ее.
    if (context.Request.Path == "/")
    {
        // Предварительное кэширование данных
        var services = context.RequestServices;
        services.GetService<ICachedService<Department>>()?.AddEntities("Departments20");
        services.GetService<ICachedService<Teacher>>()?.AddEntities("Teachers20");
        services.GetService<ICachedService<Project>>()?.AddEntities("Projects20");
        services.GetService<ICachedService<Publication>>()?.AddEntities("Publications20");
        services.GetService<ICachedService<JournalsConference>>()?.AddEntities("Journals20");
        services.GetService<ICachedService<ScientificDirection>>()?.AddEntities("Directions20");

        context.Response.StatusCode = 200; // Статус ОК для главной страницы
        context.Response.ContentType = "text/html;charset=utf-8";
        var html = new StringBuilder();
        html.Append("<!DOCTYPE html><html><head><meta charset='utf-8' /></head><body>");
        html.Append("<h1>Lab 3 - Science Portal</h1>");
        html.Append("<h2>Cached Data (20 records each):</h2>");
        html.Append("<a href='/departments'>Departments</a><br/>");
        html.Append("<a href='/teachers'>Teachers</a><br/>");
        html.Append("<a href='/projects'>Projects</a><br/>");
        html.Append("<a href='/publications'>Publications</a><br/>");
        html.Append("<a href='/journals'>Journals/Conferences</a><br/>");
        html.Append("<a href='/directions'>Scientific Directions</a><br/>");
        html.Append("<h2>Other pages:</h2>");
        html.Append("<a href='/info'>Client Info</a><br/>");
        html.Append("<a href='/searchform1'>Search Form 1 (Cookies)</a><br/>");
        html.Append("<a href='/searchform2'>Search Form 2 (Session)</a><br/>");
        html.Append("</body></html>");

        await context.Response.WriteAsync(html.ToString());
    }
    else // Если это любой другой путь, который не был обработан, показываем 404.
    {
        await context.Response.WriteAsync("<h1>Page Not Found</h1><a href='/'>Go Home</a>");
    }
});


// --- 3. ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ ---

// Метод для создания middleware для отображения таблиц
static void MapTable<T>(WebApplication app, string path, string title) where T : class
{
    app.Map(path, (HttpContext context) => {
        var cachedService = context.RequestServices.GetService<ICachedService<T>>();
        var entities = cachedService?.GetEntities($"{title}20");
        var html = BuildTableHtml(entities, title);
        return context.Response.WriteAsync(html);
    });
}

// Метод для построения HTML-таблицы
static string BuildTableHtml<T>(IEnumerable<T> entities, string tableName)
{
    var html = new StringBuilder();
    html.Append($"<!DOCTYPE html><html><head><meta charset='utf-8' /><title>{tableName}</title></head><body>");
    html.Append($"<h1>{tableName}</h1>");

    if (entities == null || !entities.Any())
    {
        html.Append("<p>No data in cache. Refresh the home page and try again.</p>");
    }
    else
    {
        html.Append("<table border='1' style='border-collapse: collapse; width: 100%;'>");
        var properties = typeof(T).GetProperties().Where(p => !p.PropertyType.IsGenericType || p.PropertyType.GetGenericTypeDefinition() != typeof(ICollection<>)).ToArray();

        // Заголовки
        html.Append("<thead><tr>");
        foreach (var prop in properties)
        {
            html.Append($"<th>{prop.Name}</th>");
        }
        html.Append("</tr></thead>");

        // Строки
        html.Append("<tbody>");
        foreach (var entity in entities)
        {
            html.Append("<tr>");
            foreach (var prop in properties)
            {
                var value = prop.GetValue(entity);
                html.Append($"<td>{value?.ToString() ?? "NULL"}</td>");
            }
            html.Append("</tr>");
        }
        html.Append("</tbody>");
        html.Append("</table>");
    }

    html.Append("<br/><a href='/'>Home</a>");
    html.Append("</body></html>");
    return html.ToString();
}

// Метод для построения HTML-формы поиска
static string BuildSearchFormHtml(string action, FormData formData)
{
    var html = new StringBuilder();
    html.Append($"<!DOCTYPE html><html><head><meta charset='utf-8' /><title>Search Form</title></head><body>");
    html.Append("<h1>Search Form</h1>");
    html.Append($"<form action='{action}' method='GET'>");

    html.Append($"<label>Search Text:</label><br/><input type='text' name='searchText' value='{formData.SearchText}'/><br/><br/>");

    html.Append("<label>Category:</label><br/>");
    html.Append("<select name='category'>");
    html.Append($"<option value='publication' {(formData.Category == "publication" ? "selected" : "")}>Publication</option>");
    html.Append($"<option value='project' {(formData.Category == "project" ? "selected" : "")}>Project</option>");
    html.Append($"<option value='teacher' {(formData.Category == "teacher" ? "selected" : "")}>Teacher</option>");
    html.Append("</select><br/><br/>");

    html.Append("<label>Is Active:</label><br/>");
    html.Append($"<input type='radio' name='isActive' value='yes' {(formData.IsActive == "yes" ? "checked" : "")}> Yes ");
    html.Append($"<input type='radio' name='isActive' value='no' {(formData.IsActive == "no" ? "checked" : "")}> No<br/><br/>");

    html.Append("<input type='submit' value='Search'/>");
    html.Append("</form>");
    html.Append("<br/><a href='/'>Home</a>");
    html.Append("</body></html>");
    return html.ToString();
}*/
using Microsoft.EntityFrameworkCore;
using SciencePortalWebApp;
using SciencePortalWebApp.Data;
using SciencePortalWebApp.Infrastructure;
using SciencePortalWebApp.Middleware;
using SciencePortalWebApp.Models;
using SciencePortalWebApp.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --- 1. РЕГИСТРАЦИЯ СЕРВИСОВ ---
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddMemoryCache();

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<SciencePortalDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped<ICachedService<Department>, CachedService<Department>>();
builder.Services.AddScoped<ICachedService<Teacher>, CachedService<Teacher>>();
builder.Services.AddScoped<ICachedService<Project>, CachedService<Project>>();
builder.Services.AddScoped<ICachedService<Publication>, CachedService<Publication>>();
builder.Services.AddScoped<ICachedService<JournalsConference>, CachedService<JournalsConference>>();
builder.Services.AddScoped<ICachedService<ScientificDirection>, CachedService<ScientificDirection>>();

var app = builder.Build();

// --- 2. НАСТРОЙКА КОНВЕЙЕРА ОБРАБОТКИ ЗАПРОСОВ ---
app.UseSession();
app.UseDbInitializer();

// Middleware для /info
app.Map("/info", appBuilder =>
{
    appBuilder.Run(async context =>
    {
        context.Response.ContentType = "text/html;charset=utf-8";
        var html = new StringBuilder();
        html.Append("<!DOCTYPE html><html><head><meta charset='utf-8' /></head><body>");
        html.Append("<h1>Client Information</h1>");
        html.Append($"<p>Host: {context.Request.Host}</p>");
        html.Append($"<p>Path: {context.Request.Path}</p>");
        html.Append($"<p>Protocol: {context.Request.Protocol}</p>");
        html.Append("<br/><a href='/'>Home</a>");
        html.Append("</body></html>");
        await context.Response.WriteAsync(html.ToString());
    });
});

// Middleware для отображения таблиц
MapTable<Department>(app, "/departments", "Departments");
MapTable<Teacher>(app, "/teachers", "Teachers");
MapTable<Project>(app, "/projects", "Projects");
MapTable<Publication>(app, "/publications", "Publications");
MapTable<JournalsConference>(app, "/journals", "Journals");
MapTable<ScientificDirection>(app, "/directions", "Directions");

// Middleware для /searchform1 (куки)
app.Map("/searchform1", appBuilder => {
    appBuilder.Run(async context => {
        var formData = context.Session.Get<FormData>("form1_session_data") ?? new FormData();
        if (context.Request.Query.ContainsKey("searchText"))
        {
            formData.SearchText = context.Request.Query["searchText"];
            formData.Category = context.Request.Query["category"];
            formData.IsActive = context.Request.Query["isActive"];
            var cookieOptions = new CookieOptions { Expires = DateTimeOffset.Now.AddMinutes(10) };
            context.Response.Cookies.Append("form1_cookie_data", System.Text.Json.JsonSerializer.Serialize(formData), cookieOptions);
        }
        context.Response.ContentType = "text/html;charset=utf-8";
        var html = BuildSearchFormHtml("/searchform1", formData);
        await context.Response.WriteAsync(html);
    });
});

// Middleware для /searchform2 (сессия)
app.Map("/searchform2", appBuilder => {
    appBuilder.Run(async context => {
        var jsonFromCookie = context.Request.Cookies["form1_cookie_data"];
        var formData = !string.IsNullOrEmpty(jsonFromCookie)
            ? System.Text.Json.JsonSerializer.Deserialize<FormData>(jsonFromCookie)
            : new FormData();

        if (context.Request.Query.ContainsKey("searchText"))
        {
            formData.SearchText = context.Request.Query["searchText"];
            formData.Category = context.Request.Query["category"];
            formData.IsActive = context.Request.Query["isActive"];
            context.Session.Set("form1_session_data", formData);
        }
        context.Response.ContentType = "text/html;charset=utf-8";
        var html = BuildSearchFormHtml("/searchform2", formData);
        await context.Response.WriteAsync(html);
    });
});

// Middleware по умолчанию (главная страница и 404)
app.Run(async (context) =>
{
    // Предварительное кэширование данных
    var services = context.RequestServices;
    services.GetService<ICachedService<Department>>()?.AddEntities("Departments20");
    services.GetService<ICachedService<Teacher>>()?.AddEntities("Teachers20");
    services.GetService<ICachedService<Project>>()?.AddEntities("Projects20");
    services.GetService<ICachedService<Publication>>()?.AddEntities("Publications20");
    services.GetService<ICachedService<JournalsConference>>()?.AddEntities("Journals20");
    services.GetService<ICachedService<ScientificDirection>>()?.AddEntities("Directions20");

    context.Response.ContentType = "text/html;charset=utf-8";
    var html = new StringBuilder();
    html.Append("<!DOCTYPE html><html><head><meta charset='utf-8' /></head><body>");
    html.Append("<h1>Lab 3 - Science Portal</h1>");
    html.Append("<h2>Cached Data (20 records each):</h2>");
    html.Append("<a href='/departments'>Departments</a><br/>");
    html.Append("<a href='/teachers'>Teachers</a><br/>");
    html.Append("<a href='/projects'>Projects</a><br/>");
    html.Append("<a href='/publications'>Publications</a><br/>");
    html.Append("<a href='/journals'>Journals/Conferences</a><br/>");
    html.Append("<a href='/directions'>Scientific Directions</a><br/>");
    html.Append("<h2>Other pages:</h2>");
    html.Append("<a href='/info'>Client Info</a><br/>");
    html.Append("<a href='/searchform1'>Search Form 1 (Cookies)</a><br/>");
    html.Append("<a href='/searchform2'>Search Form 2 (Session)</a><br/>");
    html.Append("</body></html>");

    await context.Response.WriteAsync(html.ToString());
});

app.Run();

// --- 3. ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ ---
static void MapTable<T>(WebApplication app, string path, string title) where T : class
{
    app.Map(path, appBuilder => {
        appBuilder.Run(async context => {
            context.Response.ContentType = "text/html;charset=utf-8";
            var cachedService = context.RequestServices.GetService<ICachedService<T>>();
            var entities = cachedService?.GetEntities($"{title}20");
            var html = BuildTableHtml(entities, title);
            await context.Response.WriteAsync(html);
        });
    });
}

// Метод для построения HTML-таблицы
static string BuildTableHtml<T>(IEnumerable<T>? entities, string tableName)
{
    var html = new StringBuilder();
    html.Append($"<!DOCTYPE html><html><head><meta charset='utf-8' /><title>{tableName}</title></head><body>");
    html.Append($"<h1>{tableName}</h1>");

    if (entities == null || !entities.Any())
    {
        html.Append("<p>No data in cache. Refresh the home page and try again.</p>");
    }
    else
    {
        html.Append("<table border='1' style='border-collapse: collapse; width: 100%;'>");
        var properties = typeof(T).GetProperties().Where(p => !p.PropertyType.IsGenericType || p.PropertyType.GetGenericTypeDefinition() != typeof(ICollection<>)).ToArray();

        html.Append("<thead><tr>");
        foreach (var prop in properties)
        {
            html.Append($"<th>{prop.Name}</th>");
        }
        html.Append("</tr></thead>");

        html.Append("<tbody>");
        foreach (var entity in entities)
        {
            html.Append("<tr>");
            foreach (var prop in properties)
            {
                var value = prop.GetValue(entity);
                html.Append($"<td>{value?.ToString() ?? "NULL"}</td>");
            }
            html.Append("</tr>");
        }
        html.Append("</tbody></table>");
    }

    html.Append("<br/><a href='/'>Home</a>");
    html.Append("</body></html>");
    return html.ToString();
}

// Метод для построения HTML-формы поиска
static string BuildSearchFormHtml(string action, FormData formData)
{
    var html = new StringBuilder();
    html.Append($"<!DOCTYPE html><html><head><meta charset='utf-8' /><title>Search Form</title></head><body>");
    html.Append("<h1>Search Form</h1>");
    html.Append($"<form action='{action}' method='GET'>");

    html.Append($"<label>Search Text:</label><br/><input type='text' name='searchText' value='{formData.SearchText}'/><br/><br/>");

    html.Append("<label>Category:</label><br/>");
    html.Append("<select name='category'>");
    html.Append($"<option value='publication' {(formData.Category == "publication" ? "selected" : "")}>Publication</option>");
    html.Append($"<option value='project' {(formData.Category == "project" ? "selected" : "")}>Project</option>");
    html.Append($"<option value='teacher' {(formData.Category == "teacher" ? "selected" : "")}>Teacher</option>");
    html.Append("</select><br/><br/>");

    html.Append("<label>Is Active:</label><br/>");
    html.Append($"<input type='radio' name='isActive' value='yes' {(formData.IsActive == "yes" ? "checked" : "")}> Yes ");
    html.Append($"<input type='radio' name='isActive' value='no' {(formData.IsActive == "no" ? "checked" : "")}> No<br/><br/>");

    html.Append("<input type='submit' value='Search'/>");
    html.Append("</form>");
    html.Append("<br/><a href='/'>Home</a>");
    html.Append("</body></html>");
    return html.ToString();
}