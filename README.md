# commentsys

Što se tiče generalnih smjernica komentari su:
Obavezno koriscenje MVC patern - ASP.NET MVC pattern (Razor syntax) 
Koristiti servise za komunikaciju sa AJAX-om  - Naravno
Kod mora biti napisan u OOP - OK
Implementacija binarnog stabla umjesto rekruzivnog pristupa - Ovdje nisam siguran da li sam promašio temu jer si pominjao neke "začkoljice" i rekao si binarno stablo što, ako sam ja dobro shvatio, ne odgovara potrebama comment sistema (jedan komentar moze imati vise od dva odgovora). Ovo pominjem jer me je to malo bunilo tj. ne znam da li sam dobro shvatio šta treba. Implementirao sam  tzv. preorder tree traversal algorithm za n-arno stablo. U svakom slučaju rekurzije su izbjegnute i overload  je prebacen na CRUD operacije. 
Koriscenje database DBL (Database Abstraction Layer) - korišćen  nHibernate (mapping xmls) i Entity framework
Koriscenje GIT alata i postavljanje na google code.  - github link pomenut (google code se više ne korisiti tj. ne dozvoljavaju kreiranje novih repozitorija)
Vezano za ostale tehničke smjernice:
C# ASP.NET - da
MS SQL Server - da
NHiberante - da
Javascript-a - da
Kendo UI - idealno za adrministratorski panel  -  da - administratorski panel (tree list widget)
LINQ - da 
OAuth - idealno za logovanje korisnika prilikom komentarisanja - da za logovanje koristeći facebook nalog
Rest WS / RestSharp / SOAP WS - osim standardnih MVC servisa koristio sam webApi pristup (noviji je bazira se na REST-u)
Azure SDK - Nisam stigao da ovo a planirao sam. Problem je što moram da upgrade čitavo okruženje da bih instalirao SDK pa ne stigoh oko toga
Rad sa Tweeter, Facebook, Linkedin apijima - idealno za logovanje korisnika prilikom komentarisanja - već pomenuto
HTML i CSS - ok
NLog - bridged with nHibernate to trace generated queries 
SignalR - websockets - ovo valjda ne treba za comment sistem ali sam video da Kendo ima super proste widget-e za ovo.
iTextSharp 
Twilio 
Lucane Search 
AD LDAP

commentsys
