# 1. HTTPS

> [Enforce HTTPS in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-3.1&tabs=visual-studio)

## A. Forst� HTTPS i ASP.NET
1. I `Startup.cs` udkommenteres `app.UseHttpsRedirection()`;
2. Start Fiddler, filter = localhost og ingen HTTPS decryption (Tools | Options | HTTPS og deselect Decrypt HTTPS traffic)
3. Web uden at benytte SSL, http://localhost:63919 og v�lg CREATE. Send en submit og se at alle data er synlige i POST request pakken.
4. (virker ikke n�r f�rst Decryption har v�ret aktiveret): Web med SSL, https://localhost:44358/ og v�lg CREATE. Send en submit og se at alle data er **krypterede** i POST request pakken.
5. Sl� Decrypt HTTPS traffic til i Fiddler, genstart Fiddler og web SSL adressen og CREATE. Parametre kan stadig ses - men kun fordi Fiddler er "Man in the middle" og dekrypterer.
&nbsp;
## B. Sikrer at HTTPS benyttes
1. I `Startup.cs` tilf�jes `app.UseHttpsRedirection()`;
2. Web uden at benytte SSL, http://localhost:63919 og se omstillingen med `HTTP 307` i Fiddler.
3. Bem�rk at kun hvis` UseHttpsRedirection()` kommer f�r `UseStaticFiles()` vil css og html v�re krypteret
4. Vigtigt at benytte `relative links` til alle lokale ressourcer, s� scheme automatisk tilpasses
5. Bem�rk linket til et billede, hvis src peger p� et http-link. Det f�r hele siden til at v�re usikker.
6. Portnummer kan konfigurers i servicen, men er normalt styret af webserveren:
```c#
services.AddHttpsRedirection((opts) =>
{
    opts.HttpsPort = 443;
});
```
&nbsp;
## C. HSTS
1. Inkomment�r `UseHSTS();`
2. Konfigur�r service med:
```c#
services.AddHsts(opts =>
{
    opts.IncludeSubDomains = true;
    opts.MaxAge = TimeSpan.FromMinutes(5);
});
```
F�lgende kr�ver at siten er deployet til andet end localhost (localhost er excluded i browserens HSTS)
3. I `Properties | Debug` fjernes flueben ved `Launch browser`
4. Start og web: http://localhost:63919 med DevTools �ben
5. Bem�rk `HTTP 307` og omdirigering til HTTPS. Se ogs� i `Response Headers` at strict-transport-security er sat til 300 sekunder (5 minutter)

[How to Clear HSTS Settings on Chrome, Firefox and IE Browsers](https://www.ssl2buy.com/wiki/how-to-clear-hsts-settings-on-chrome-firefox-and-ie-browsers)

6. Registrering for [preloading](https://hstspreload.org)



# 2. Prevent Cross-Site Request Forgery (XSRF/CSRF)
 Se denne Pluralsight video https://app.pluralsight.com/course-player?clipId=9cbe2b8b-8ead-43e2-97a5-7faaa13744d9
> [Prevent Cross-Site Request Forgery (XSRF/CSRF) attacks in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/anti-request-forgery?view=aspnetcore-3.1)

1. I DevTools studeres en POST pakke i Element, hvor der browses til form. Pr�v at redigere hidden attributten `__RequestVerificationToken`
    og se at en ny Submit nu giver en HTTP 400 error. 
2. I DevTools sammenlignes cookie under *Application | Cookies* med den kode, der ligger i hidden field og ses i fanen *Elements*

Denne kode s�ttes i html af serveren samtidigt med at den ogs� gemmer den i en cookie. Koden er ny for hver request.
    
XCSS er bygget default ind i RazorPages. I ASP.NET Core MVC
    skal man manuelt tilf�je [ValidateAntiForgeryToken] til samtlige POST-actions.
   


# 3. Cross Site Scripting (XSS)
> [Prevent Cross-Site Scripting (XSS) in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/cross-site-scripting?view=aspnetcore-3.1)

1. Skriv f�lgende i et tekstfelt: `<script>alert('Hi!')</script>`. Bem�rk at det ikke eksekveres, men blot vises. �bn *View Source* i browseren og se at 
koden er blevet HTML-encodet.
2. I List-pagen �ndres koden for restaurant-name til f�lgende: `<td>@Html.Raw(restaurant.Name)</td>`. N�r siden k�res, er det en JavaScript pop-up som vises!
&nbsp;

# 4. Open Redirection Attack
- Et link modtages i en Phishing email: http://bank.com/Account/LogOn?returnUrl=http://bank.net/Account/LogOn
- Logger ind p� korrekt site, omdrigeres til fake
- Tror han har fejlindtastet password og pr�ver igen.
- Nu er password fisket!
- 
Ingen demo her. Vis blot koden:
```c#
if(!Url.IsLocalUrl(returnUrl))
{
    // throw new Exception();
}
```