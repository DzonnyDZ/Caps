![Převor 10%](https://download-codeplex.sec.s-msft.com/Download?ProjectName=caps&DownloadId=82319&Build=21031)
**Welcome to the Caps project!**
I'm collecting various caps from bottles - mainly from drinks. My collection currently contains about 2000 caps and is growing. I store the caps in boxes. When a new cap arrives it's very hard to determine if I already have the cap in collection or not.
* First goal is to *help organize collection of caps* by allowing search using various criteria. (Of curse, I must first fill the database.)
* Once I'll have information about all the caps in computer I'd be great to present my collection to entire world. So, second goal is *web site of the caps collection*.
* Finally, nice to have: I usually add caps I find on the street (lucky that people make mess) to my collection. It'll be nice to examine the cap right on the street, if it is forth taking. So, I plan to create *Windows Mobile version of caps catalog application*.

# Current status
* **Caps Console** _(WPF application with SQL Express 2008 database)_ - under development (beta version available)
* **Caps Web** _(Web site to show caps to public, maybe with add/edit functionality for admins)_ - awaiting development
* **Caps Mobile** _(.NET CF application with read-only SQL CE database)_ - planned

! Details
The most important part of the application is database. Database structure is highly specialized for purposes of Caps collecting. Maybe it can be used for some similar products like cans, bottles, corks etc. It will not fit for collection of post marks!
The central part of database is table _Cap_ storing various details of caps like texts, colors, size etc. Then there are several helper tables like _CapType_, _Product_, _Keyword_, _Storage_ or _Image_. The database is designed as something between full caps details (very time consuming to fill-in) and only few details (easy to fill but uselles for search and presentation purposes). It should be possible for example to make queries as 'All caps from beer' or 'All red caps', 'All caps without text', 'All caps with image of house', 'All caps from Coca-Cola', 'All caps from cola', 'All metal caps', 'All round caps with 1cm diameter' etc.

This CodePlex project is set up to provide the code and later binary of applications mentioned above to everybody who may find it useful, and to provide me source control, or course.

Application code is written in Visual Basic 9 and I'll switch 10 VB 10 as soon as there will be some more stable release (maybe Beta 2).

![main](http://download-codeplex.sec.s-msft.com/Download?ProjectName=Caps&DownloadId=87872)
Main screen
![edit](http://download-codeplex.sec.s-msft.com/Download?ProjectName=Caps&DownloadId=87870)
Cap editor
![db](http://download-codeplex.sec.s-msft.com/Download?ProjectName=Caps&DownloadId=87869)
Database setup
![search](http://download-codeplex.sec.s-msft.com/Download?ProjectName=Caps&DownloadId=87873)
Search results / cap details view
[lists](http://download-codeplex.sec.s-msft.com/Download?ProjectName=Caps&DownloadId=87871)
Edit various lists

[caps](http://download-codeplex.sec.s-msft.com/Download?ProjectName=Caps&DownloadId=82318)
