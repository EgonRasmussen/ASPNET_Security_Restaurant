# 1. HTTPS

> [Enforce HTTPS in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-3.1&tabs=visual-studio)

## A. Forstå HTTPS i ASP.NET
1. I `Startup.cs` udkommenteres `app.UseHttpsRedirection()`;
2. Start Fiddler, filter = localhost og ingen HTTPS decryption (Tools | Options | HTTPS og deselect Decrypt HTTPS traffic)
3. Web uden at benytte SSL, http://localhost:63919 og vælg CREATE. Send en submit og se at alle data er synlige i POST request pakken.
4. (virker ikke når først Decryption har været aktiveret): Web med SSL, https://localhost:44358/ og vælg CREATE. Send en submit og se at alle data er **krypterede** i POST request pakken.
5. Slå Decrypt HTTPS traffic til i Fiddler, genstart Fiddler og web SSL adressen og CREATE. Parametre kan stadig ses - men kun fordi Fiddler er "Man in the middle" og dekrypterer.
&nbsp;
## B. Sikrer at HTTPS benyttes
1. I `Startup.cs` tilføjes `app.UseHttpsRedirection()`;
2. Web uden at benytte SSL, http://localhost:63919 og se omstillingen med `HTTP 307` i Fiddler.
3. Bemærk at kun hvis` UseHttpsRedirection()` kommer før `UseStaticFiles()` vil css og html være krypteret
4. Vigtigt at benytte `relative links` til alle lokale ressourcer, så scheme automatisk tilpasses
5. Bemærk linket til et billede, hvis src peger på et http-link. Det får hele siden til at være usikker.
6. Portnummer kan konfigurers i servicen, men er normalt styret af webserveren:
```c#
services.AddHttpsRedirection((opts) =>
{
    opts.HttpsPort = 443;
});
```
&nbsp;
## C. HSTS
1. Inkommentér `UseHSTS();`
2. Konfigurér service med:
```c#
services.AddHsts(opts =>
{
    opts.IncludeSubDomains = true;
    opts.MaxAge = TimeSpan.FromMinutes(5);
});
```
Følgende kræver at siten er deployet til andet end localhost (localhost er excluded i browserens HSTS)
3. I `Properties | Debug` fjernes flueben ved `Launch browser`
4. Start og web: http://localhost:63919 med DevTools åben
5. Bemærk `HTTP 307` og omdirigering til HTTPS. Se også i `Response Headers` at strict-transport-security er sat til 300 sekunder (5 minutter)

[How to Clear HSTS Settings on Chrome, Firefox and IE Browsers](https://www.ssl2buy.com/wiki/how-to-clear-hsts-settings-on-chrome-firefox-and-ie-browsers)

6. Registrering for [preloading](https://hstspreload.org)



# 2. Prevent Cross-Site Request Forgery (XSRF/CSRF)
 Se denne Pluralsight video https://app.pluralsight.com/course-player?clipId=9cbe2b8b-8ead-43e2-97a5-7faaa13744d9
> [Prevent Cross-Site Request Forgery (XSRF/CSRF) attacks in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/anti-request-forgery?view=aspnetcore-3.1)

1. I DevTools studeres en POST pakke i Element, hvor der browses til form. Prøv at redigere hidden attributten `__RequestVerificationToken`
    og se at en ny Submit nu giver en HTTP 400 error. 
2. I DevTools sammenlignes cookie under *Application | Cookies* med den kode, der ligger i hidden field og ses i fanen *Elements*

Denne kode sættes i html af serveren samtidigt med at den også gemmer den i en cookie. Koden er ny for hver request.
    
XCSS er bygget default ind i RazorPages. I ASP.NET Core MVC
    skal man manuelt tilføje [ValidateAntiForgeryToken] til samtlige POST-actions.
   


# 3. Cross Site Scripting (XSS)
> [Prevent Cross-Site Scripting (XSS) in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/cross-site-scripting?view=aspnetcore-3.1)

1. Skriv følgende i et tekstfelt: `<script>alert('Hi!')</script>`. Bemærk at det ikke eksekveres, men blot vises. Åbn *View Source* i browseren og se at 
koden er blevet HTML-encodet.
2. I List-pagen ændres koden for restaurant-name til følgende: `<td>@Html.Raw(restaurant.Name)</td>`. Når siden køres, er det en JavaScript pop-up som vises!
&nbsp;

# 4. Open Redirection Attack
- Et link modtages i en Phishing email: http://bank.com/Account/LogOn?returnUrl=http://bank.net/Account/LogOn
- Logger ind på korrekt site, omdrigeres til fake
- Tror han har fejlindtastet password og prøver igen.
- Nu er password fisket!
- 
Ingen demo her. Vis blot koden:
```c#
if(!Url.IsLocalUrl(returnUrl))
{
    // throw new Exception();
}
```