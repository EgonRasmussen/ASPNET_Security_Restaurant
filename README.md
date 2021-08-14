# HTTPS

> [Enforce HTTPS in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-3.1&tabs=visual-studio)

## A. Forst� HTTPS i ASP.NET
1. I `Startup.cs` udkommenteres `app.UseHttpsRedirection()`;
2. Start Fiddler, filter = localhost og ingen HTTPS decryption (Tools | Options | HTTPS og deselect Decrypt HTTPS traffic)
3. Web uden at benytte SSL, http://localhost:63919 og v�lg CREATE. Send en submit og se at alle data er synlige i POST request pakken.
4. (virker ikke n�r f�rst Decryption har v�ret aktiveret): Web med SSL, https://localhost:44358/ og v�lg CREATE. Send en submit og se at alle data er **krypterede** i POST request pakken.
5. Sl� Decrypt HTTPS traffic til i Fiddler, genstart Fiddler og web SSL adressen og CREATE. Parametre kan stadig ses - men kun fordi Fiddler er "Man in the middle" og dekrypterer.
&nbsp;
## B. Sikre at HTTPS benyttes
1. I `Startup.cs` tilf�jes `app.UseHttpsRedirection()`;
2. Web uden at benytte SSL, http://localhost:63919 og se omstillingen med `HTTP 307` i Fiddler. L�s om [HTTP 307](https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/307)
3. Bem�rk at kun hvis` UseHttpsRedirection()` kommer f�r `UseStaticFiles()` vil css og html v�re krypteret

&nbsp;

## C. HSTS (HTTPS Strict Transport Security)

Se denne video fra Pluralsight: https://app.pluralsight.com/course-player?clipId=4a5a88b6-d619-45c7-ace6-5e5f15334210
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

3. I `Properties | Debug` fjernes flueben ved `Launch browser` s� den ikke laver en indledende  Redirect inden vi er klar til at se p� kommunikationen.
4. Start og web: http://localhost:63919 med DevTools �ben
5. Bem�rk `HTTP 307` og omdirigering til HTTPS. Se ogs� i `Response Headers` at strict-transport-security er sat til 300 sekunder (5 minutter)

[How to Clear HSTS Settings on Chrome, Firefox and IE Browsers](https://www.ssl2buy.com/wiki/how-to-clear-hsts-settings-on-chrome-firefox-and-ie-browsers)

6. Registrering for [preloading](https://hstspreload.org)

## D. Mixed Content

1. Bem�rk linket til et billede, hvis src peger p� et http-link. Det f�r hele siden til at v�re usikker, men Chrome fors�ger en automatisk omdirigering til en HTTPS ressource. Og en s�dan findes i demo-eksemplet. Demo af Details for Cinnamon Club, hvor et billede fra Apache loades via http!
2. Vigtigt at benytte `relative links` til alle lokale ressourcer, s� scheme automatisk tilpasses.
3. Kig i **DevTools** under **Console** og l�s warning om "Mixed Content".