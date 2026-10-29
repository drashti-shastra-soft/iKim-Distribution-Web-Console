# i'Kim Web Console (Admin Panel)

**Live URL:** [https://ikim-web-console.shastrasoft.net/](https://ikim-web-console.shastrasoft.net/)

The **i'Kim Web Console** is the browser-based administration panel for the i'Kim platform. It lets authorized staff manage master data (stores, users, categories, items, banners, units of measure, vendors) and operational data (purchases, orders, party orders, stock, stock updates, stock transfers, notifications), and view dashboards and reports.

The Console does **not** access a database directly. It is a presentation layer that signs users in and performs all reads and writes by calling the i'Kim REST API over HTTP.

---

## Table of Contents

- [Live Environment](#live-environment)
- [Technology Stack](#technology-stack)
- [Solution Structure](#solution-structure)
- [Console Web Project Layout](#console-web-project-layout)
- [Functional Modules](#functional-modules)
- [Architecture and Request Flow](#architecture-and-request-flow)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
- [Configuration](#configuration)
- [Authentication Flow](#authentication-flow)
- [Build and Publish](#build-and-publish)
- [Troubleshooting](#troubleshooting)
- [Known Observations](#known-observations)

---

## Live Environment

| Item | Value |
|------|-------|
| Console URL | https://ikim-web-console.shastrasoft.net/ |
| API Base URL used in production | `https://ikim-web-service.shastrasoft.net/api` (from `Config/App.Prod.config`) |

The site opens on the login page (`Authentication/Index`).

---

## Technology Stack

All versions below are taken from the project files (`*.csproj`, `packages.config`, `Web.config`).

| Area | Technology |
|------|------------|
| Framework | ASP.NET MVC 5.2.9 on .NET Framework 4.8 |
| Language | C# with Razor views (`.cshtml`) |
| Dependency injection | Unity 5.11.10 with Unity.Mvc5 1.4.0 |
| HTTP / JSON | `System.Net.Http.HttpClient`, Newtonsoft.Json 13.0.3 |
| Bundling | Microsoft.AspNet.Web.Optimization 1.1.3, WebGrease 1.6.0 |
| Client validation | jQuery.Validation 1.19.5, Microsoft.jQuery.Unobtrusive.Validation 3.2.11 |
| Front-end packages | jQuery 3.7.0 (NuGet; the layout loads `Scripts/plugins/jquery/jquery.min.js`), Bootstrap 5.2.3 (NuGet), Modernizr 2.8.3 |
| UI theme and plugins | AdminLTE (`adminlte.min.css` / `adminlte.min.js`), Font Awesome, Toastr, Dropzone, Chart.js, Daterangepicker / Tempus Dominus, Moment.js, jQuery Mask, jQuery Unobtrusive Ajax |
| Hosting (development) | IIS Express |
| Session state | ASP.NET session (in-process; no `sessionState` override in `Web.config`) |

---

## Solution Structure

The Console solution (`IKimWebConsole.sln`) contains four projects, all targeting .NET Framework 4.8:

| Project | Type | Responsibility |
|---------|------|----------------|
| `IKimWebConsole` | ASP.NET MVC web application | Controllers, Razor views, filters, startup configuration, static assets, client-side scripts |
| `IKimWebConsole.Service` | Class library | One service class per module; wraps calls to the i'Kim API using `HttpClientService` |
| `IKimWebConsole.Infrastructure` | Class library | Service interfaces (`IService/`), HTTP client provider, session helper, custom exceptions, extension methods, audit helper |
| `IKimWebConsole.Domain` | Class library | Models (e.g. `User`, `Item`, `Order`, `Stock`) and list/pagination models (`*Lister`) |

```text
Console/
├── IKimWebConsole.sln
├── IKimWebConsole/                  # MVC web application (startup project)
├── IKimWebConsole.Domain/           # Models and lister/pagination classes
├── IKimWebConsole.Infrastructure/   # Interfaces, HTTP provider, helpers
├── IKimWebConsole.Service/          # API-consuming service implementations
├── packages/                        # NuGet packages (restored)
├── .gitignore
└── .gitattributes
```

---

## Console Web Project Layout

```text
IKimWebConsole/
├── App_Start/
│   ├── BundleConfig.cs        # Script/style bundle definitions
│   ├── FilterConfig.cs        # Global filters (HandleErrorAttribute)
│   ├── RouteConfig.cs         # Default route: {controller}/{action}/{id}
│   └── UnityConfig.cs         # Registers every I*Service -> *Service mapping
├── Config/
│   ├── App.Dev.config         # appSettings for development (active by default)
│   └── App.Prod.config        # appSettings for production
├── Controllers/               # One controller per module
├── Filter/
│   └── Authentication.cs      # [Authentication] action filter
├── Helper/
│   └── PaginationHelper.cs    # Builds the pagination summary text
├── Views/
│   ├── Shared/                # _Layout, Error, _Unauthorized
│   └── <Module>/              # Index.cshtml + partials (_List, _Create, ...)
├── custom-js/                 # Page-specific JavaScript, one folder per module
├── Content/                   # CSS, AdminLTE theme, plugin styles, images
├── Scripts/                   # jQuery, AdminLTE JS, plugins (Chart.js, Toastr, Dropzone, ...)
├── Properties/PublishProfiles/# Folder publish profiles
├── Global.asax / .cs          # Application_Start
├── Web.config                 # Main configuration
├── Web.Debug.config           # Config transform (Debug)
└── Web.Release.config         # Config transform (Release)
```

### Important Files

| File | Purpose |
|------|---------|
| `Global.asax.cs` | On start, registers areas, Unity, global filters, routes and bundles |
| `App_Start/RouteConfig.cs` | Default route; the landing page is `Authentication/Index` |
| `App_Start/UnityConfig.cs` | Maps service interfaces to implementations for constructor injection |
| `Filter/Authentication.cs` | Redirects to the login page (with `returnUrl`) when no user is in session |
| `Web.config` | Selects the active `appSettings` file; sets request size and timeout limits |
| `Config/App.Dev.config`, `Config/App.Prod.config` | Contain the `APIBaseUrl` setting |
| `Views/Shared/_Layout.cshtml` | AdminLTE layout, sidebar menu, shared scripts and styles |
| `Views/Shared/_Unauthorized.cshtml` | Partial returned when the API responds with 401 |
| `custom-js/layout/*.js` | Shared notification, helper and document-event scripts |
| `IKimWebConsole.Service/HttpClientService.cs` | Wrapper for GET, POST, PUT, DELETE and PATCH with bearer token support |
| `IKimWebConsole.Infrastructure/HttpContextHelper.cs` | Stores/reads the logged-in user in session |

### Controllers

Controllers live in `IKimWebConsole/Controllers/`:

`Authentication`, `Logout`, `ForgotPassword`, `ChangePassword`, `Dashboard`, `Store`, `User`, `Category`, `Item`, `Banner`, `UnitOfMeasure`, `Vendor`, `Purchase`, `PurchaseItem`, `Order`, `OrderLine`, `DailyOrder`, `PartyOrder`, `PartyOrderLine`, `Stock`, `StockItem`, `StockUpdate`, `StockTransfer`, `Notification`, `StockReport`, `DailySales`.

Each module follows the same pattern: `Index` renders the page shell, `List` returns a paged `_List` partial, and `Create` / `Get` / `Delete` return the `_Create` partial or a JSON result. Modules rendered through partials load their behaviour from the matching file in `custom-js/`.

---

## Functional Modules

The sidebar menu in `_Layout.cshtml` exposes the following:

| Menu item | Entries |
|-----------|---------|
| Dashboard | Dashboard (sales chart plus store, vendor and item lookups) |
| Master (group) | Store, User, Category, Item, Banner, Unit Of Measure, Vendor |
| Top-level items | Purchase, Order, Party Order, Stock, Stock Update, Notification, Stock Transfer |
| Reports (group) | Stock Report, Daily Sales |
| Account | Change Password, Logout |

Additional controllers support these screens (for example `OrderLine`, `PartyOrderLine`, `PurchaseItem`, `StockItem`, `DailyOrder`).

---

## Architecture and Request Flow

```text
Browser
   │  (Razor views + jQuery/AJAX from custom-js/)
   ▼
MVC Controller            IKimWebConsole/Controllers
   │  (injected via Unity)
   ▼
I<Module>Service          IKimWebConsole.Infrastructure/IService
   ▼
<Module>Service           IKimWebConsole.Service
   │  reads APIBaseUrl and the user's token from session
   ▼
HttpClientService  ──►  i'Kim REST API  ({APIBaseUrl}/<Module>/...)
```

- Service classes read `APIBaseUrl` with `ConfigurationManager.AppSettings` and build URLs such as `{APIBaseUrl}/Category/GetAll`, `{APIBaseUrl}/Category`, `{APIBaseUrl}/Authentication`.
- Data-changing actions stamp audit fields (`CreatedBy`, `LastModifiedBy`) from the logged-in user via `StaticMethods.SetAuditOnUpsert`.
- Controllers translate API outcomes into UI results: a 401 from the API becomes an `Unauthorized` JSON result or `_Unauthorized` partial with a redirect to the login page; 400 errors surface the API message; other failures show a generic error.

---

## Prerequisites

| Requirement | Notes |
|-------------|-------|
| Windows | ASP.NET MVC 5 on .NET Framework 4.8 requires Windows |
| Visual Studio | With the **ASP.NET and web development** workload (the project is a classic `.csproj` web application using IIS Express) |
| .NET Framework 4.8 Developer Pack / Targeting Pack | Required to build (projects target `v4.8`) |
| IIS Express | Installed with Visual Studio; configured in the web project settings (`UseIISExpress`) |
| NuGet | Included with Visual Studio; used to restore packages |
| A running i'Kim API | Must be reachable at the configured `APIBaseUrl` (see [Configuration](#configuration)) |
| Internet access | The layout loads the Source Sans Pro font from Google Fonts |

---

## Getting Started

### 1. Clone the repository

```bash
git clone <your-repository-url>
cd <repository-folder>/Console
```

> Replace `<your-repository-url>` and `<repository-folder>` with your actual repository location. The Console solution file is `IKimWebConsole.sln`.

### 2. Start the API

Run the i'Kim API first so the Console has a backend. For local development the Console expects it at `https://localhost:44342/api`. Refer to the API's own documentation for how to run it.

### 3. Select the API environment

Open `IKimWebConsole/Web.config`. The active `appSettings` file is selected here:

```xml
<!-- <appSettings configSource="Config\App.Prod.config"></appSettings> -->
<appSettings configSource="Config\App.Dev.config"></appSettings>
```

Development is active by default. To use production settings, comment out the Dev line and uncomment the Prod line.

### 4. Restore NuGet packages

In Visual Studio, packages restore automatically on build. From a Developer Command Prompt:

```bash
nuget restore IKimWebConsole.sln
```

### 5. Build

```bash
msbuild IKimWebConsole.sln /p:Configuration=Debug
```

Or in Visual Studio: **Build > Build Solution** (`Ctrl+Shift+B`).

### 6. Run

1. In Visual Studio, set `IKimWebConsole` as the startup project.
2. Press `F5` (debug) or `Ctrl+F5` (without debugging).

The project is configured for IIS Express over HTTPS:

```text
https://localhost:44349/
```

The site opens on the login page (`Authentication/Index`).

---

## Configuration

### API Base URL

The Console reads a single `APIBaseUrl` application setting. It is defined in two files under `IKimWebConsole/Config/`, and `Web.config` decides which one is loaded.

| File | `APIBaseUrl` value |
|------|--------------------|
| `Config/App.Dev.config` (active by default) | `https://localhost:44342/api` |
| `Config/App.Prod.config` | `https://ikim-web-service.shastrasoft.net/api` |

Example (`Config/App.Dev.config`):

```xml
<appSettings>
  <add key="webpages:Version" value="3.0.0.0" />
  <add key="webpages:Enabled" value="false" />
  <add key="ClientValidationEnabled" value="true" />
  <add key="UnobtrusiveJavaScriptEnabled" value="true" />
  <add key="APIBaseUrl" value="https://localhost:44342/api" />
</appSettings>
```

To point the Console at a different API, change only the `APIBaseUrl` value in the relevant config file. Do not include a trailing slash; service classes append paths such as `/Authentication` directly.

### Other Application Settings

| Setting | Value | Purpose |
|---------|-------|---------|
| `webpages:Version` | `3.0.0.0` | Razor web pages version |
| `webpages:Enabled` | `false` | Disables Web Pages routing so MVC handles requests |
| `ClientValidationEnabled` | `true` | Enables client-side validation |
| `UnobtrusiveJavaScriptEnabled` | `true` | Enables unobtrusive validation/AJAX |

### Request Limits (`Web.config`)

| Setting | Value |
|---------|-------|
| `httpRuntime maxRequestLength` | `1048576` (KB) |
| `httpRuntime executionTimeout` | `3600` (seconds) |
| `requestLimits maxAllowedContentLength` | `2147483647` (bytes) |

These large limits support file uploads (for example category images, which are sent to the API as base64).

### Compilation Mode

`Web.config` sets `<compilation debug="true" targetFramework="4.8" />`. The `Web.Release.config` transform removes the `debug` attribute when publishing in the Release configuration.

---

## Authentication Flow

Confirmed from `AuthenticationController`, `AuthenticationService`, `HttpContextHelper`, the `Authentication` filter and `HttpClientService`:

1. A user opens the site and lands on `Authentication/Index` (the default route).
2. The login form submits a `Credential` (`EmailAddress`, `Password`, `RememberMe`).
3. `AuthenticationService` POSTs the credential to `{APIBaseUrl}/Authentication` and receives a `User` object, which includes a `Token`.
4. The Console rejects the login if the returned user is missing (`Id <= 0`) or inactive (`IsActive = false`), and shows an error message.
5. On success, the `User` is stored in ASP.NET session (`HttpContextHelper.LoginUser`), and the user is redirected to the original `returnUrl`, or to `Dashboard/Index` if none was supplied.
6. Every module service reads the token from the session user and sends it as `Authorization: Bearer <token>` on API calls.
7. Controllers marked with the `[Authentication]` filter redirect to the login page (preserving `returnUrl`) when there is no user in session.
8. If the API returns `401 Unauthorized`, the Console reports the session as expired and redirects to the login page.
9. `Logout/Index` clears the session user and redirects to the login page.

**Remember me:** when `RememberMe` is set, the login credential is serialized into the `IKimLoginUser` cookie (expiring in one year) to pre-fill the login form on the next visit.

**Related screens:** `ForgotPassword` posts to `{APIBaseUrl}/ForgotPassword/` (the UI message states a new password is sent to the registered email) and `ChangePassword` posts to `{APIBaseUrl}/ChangePassword`.

---

## Build and Publish

Folder publish profiles are provided in `IKimWebConsole/Properties/PublishProfiles/` (`FolderProfile.pubxml` through `FolderProfile5.pubxml`).

In Visual Studio: right-click `IKimWebConsole` > **Publish** > choose a profile.

Command line example:

```bash
msbuild IKimWebConsole/IKimWebConsole.csproj /p:Configuration=Release /p:DeployOnBuild=true /p:PublishProfile=FolderProfile
```

Before publishing for production:

- Switch `Web.config` to load `Config\App.Prod.config`.
- Confirm that the `APIBaseUrl` in that file points to the intended API.
- Build with the `Release` configuration so debug compilation is disabled.

---

## Troubleshooting

| Symptom | Likely cause and fix |
|---------|----------------------|
| Login always fails or shows a generic error | The API is not running or `APIBaseUrl` is wrong. Verify the URL in the active config file |
| SSL/TLS errors when calling the API locally | Trust the IIS Express development certificate for both the Console and API |
| Redirected to the login page repeatedly | Session expired, or the API returned 401 (invalid or expired token). Sign in again |
| Build errors about missing packages | Restore NuGet packages (`nuget restore IKimWebConsole.sln`) |
| Targeting pack errors | Install the .NET Framework 4.8 Developer Pack |
| Wrong environment in use | Check which `appSettings configSource` line is uncommented in `Web.config` |

---

## Known Observations

These were found while reviewing the source. They are documented for awareness only; no code was changed.

- `UnitOfMeasureController`, unlike the other module controllers, is not decorated with `[Authentication]`. Its API calls would still require a valid token, but the page itself is not guarded by the filter.
- The "Remember me" cookie stores the serialized `Credential` object, which includes the password field. Consider reviewing this approach before production use.
- Session state uses the ASP.NET default (in-process), so sessions are lost on application pool recycle and are not shared across multiple servers.
- `Config/App.Prod.config` is the file that determines the production API address; keep any environment-specific values out of source control if they should remain private.

---

© 2025–2026 i'Kim. All rights reserved.
