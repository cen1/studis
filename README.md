# studis
Studis za TPO

1. Namestitve za mysql in VS  
https://dev.mysql.com/downloads/connector/net/  
http://dev.mysql.com/downloads/windows/visualstudio/

2. Baza  
my_aspnet_applications: seznam aplikacij, imamo samo eno  
my_aspnet_membership: glavna tabela uporabnikov za login  
my_aspnet_roles: vse role za našo aplikacijo  
my_aspnet_users: èe prav razumem ta tabela obstaja zato, da lahko imaš veè uporabnikov z istim imenom èe imaš veè aplikacij, FK userID v membership
my_aspnet_usersinroles: poveže uporabnika z rolo

Torej, vse tabele ki se navezujejo na nekega uporabnika (npr študent) povezujemo z FK na tabelo my_aspnet_users


3. Vstavljanje in iskanje po bazi z Entity Framework 6   
http://www.entityframeworktutorial.net  
Bistvo je da imaš nek globalen kontekst (pri nas se imenuje Studis), ki ga definiraš kot nek private v kontrolerju  
ali pa uporabiš use() sintakso. Nad tem kontekstom se potem vršijo operacije SQL.  
Lahko pišeš direkt SQL, uporabljaš LINQ ali pa Entity SQL, glej poglavje "Querying with EDM"

4. Obrazci  
Za primer glej UserModels.cs.. za vsak obrazec narediš class kjer definiraš vse spremenljivke in validatorje.  
Nato v View (npr Login.cs) bindaš na ta model in vse se avtomagièno samo validira.

5. Vloge in pravice  
Roèno sem vstavil v bazo vloge: Študent, Skrbnik, Profesor, Referent  
Vloge so definirane v my_apsnet_roles in povezane na uporabnike my_aspnet_usersinroles  
Èe hoèete omejiti nek kontroler na doloèeno rolo uporabite nad kontrolerjem: [Authorize(Roles = "Študent")] ipd  
Simpl zadeva. Lahklo omejiš tudi na doloèenega userja ali samo na doloèeno akcijo kontrolerja.