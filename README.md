# HTTPS

> [Enforce HTTPS in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-3.1&tabs=visual-studio)

## A. Forstå HTTPS i ASP.NET
1. I `Startup.cs` udkommenteres `app.UseHttpsRedirection()`;
2. Start Fiddler, filter = localhost og ingen HTTPS decryption (Tools | Options | HTTPS og deselect Decrypt HTTPS traffic)
3. Web uden at benytte SSL, http://localhost:63919 og vælg CREATE. Send en submit og se at alle data er synlige i POST request pakken.
4. (virker ikke når først Decryption har været aktiveret): Web med SSL, https://localhost:44358/ og vælg CREATE. Send en submit og se at alle data er **krypterede** i POST request pakken.
5. Slå Decrypt HTTPS traffic til i Fiddler, genstart Fiddler og web SSL adressen og CREATE. Parametre kan stadig ses - men kun fordi Fiddler er "Man in the middle" og dekrypterer.
&nbsp;
## B. Sikre at HTTPS benyttes
1. I `Startup.cs` tilføjes `app.UseHttpsRedirection()`;
2. Web uden at benytte SSL, http://localhost:63919 og se omstillingen med `HTTP 307` i Fiddler. Læs om [HTTP 307](https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/307)
3. Bemærk at kun hvis` UseHttpsRedirection()` kommer før `UseStaticFiles()` vil css og html være krypteret
4. Bemærk linket til et billede, hvis src peger på et http-link. Det får hele siden til at være usikker. Demo af Details for Cinnamon Club, hvor et billede fra Apache loades via http!
5. Vigtigt at benytte `relative links` til alle lokale ressourcer, så scheme automatisk tilpasses.
6. Kig i **DevTools** under **Console** og læs warning om "Mixed Content".

&nbsp;

## C. HSTS (HTTPS Strict Transport Security)

Se denne video fra Pluralsight: https://app.pluralsight.com/course-player?clipId=8e64eaa2-9d1f-491c-a0da-01846cd43c59
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

